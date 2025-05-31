
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ProjectWorkAPI.Models;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BooksLibrary.Controllers;

public class SecurityController
{
    public static readonly string[] LowPrivilegesRoles = ["10", "20"];
    private static SymmetricSecurityKey? jwtSigningKey;
    public static SecurityKey JwtSigningKey
    {
        get
        {
            if (jwtSigningKey != null) return jwtSigningKey;
            throw new SecurityTokenEncryptionKeyNotFoundException();
        }
    }


    public static void Init()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var mk = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build().GetSection("MasterKey");

        if (mk != null && mk.Value != null)
        {
            SHA512 sha512 = SHA512.Create();
            var key = sha512.ComputeHash(Encoding.ASCII.GetBytes(mk.Value));
            jwtSigningKey = new SymmetricSecurityKey(key);
        }
    }

    public static async Task<User?> GetUserFromToken(RestaurantContext dbContext, HttpContext httpContext)
    {
        try
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return null;

            IEnumerable<Claim> claim = identity.Claims;

            Claim userId_Claim = claim.First(x => x.Type == "id");
            int userId = int.Parse(userId_Claim.Value);
            User? user = await dbContext.Users.FirstOrDefaultAsync(q => q.Id == userId && q.Password != "");

            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task InvalidateToken(RestaurantContext dbContext, int userId)
    {
        User? user = dbContext.Users.FirstOrDefault(q => q.Id == userId);
        if (user != null && user.SessionToken != null && user.LastLogin != null)
        {
            await InvalidateToken(dbContext, user.SessionToken, ((DateTime)user.LastLogin).AddDays(2));
        }
    }

    public static async Task InvalidateToken(RestaurantContext dbContext, HttpContext httpContext)
    {
        JwtSecurityToken jwt = new JwtSecurityToken(await httpContext.GetTokenAsync("access_token"));
        string tokenHash = CalculateTokenHash(jwt.RawData);
        if (jwt.Payload.Exp != null)
            await InvalidateToken(dbContext, tokenHash, new DateTime(1970, 01, 01).AddDays(1).AddSeconds(Convert.ToDouble(jwt.Payload.Exp)));
    }
    public static async Task InvalidateToken(RestaurantContext dbContext, string tokenHash, DateTime tokenExpire)
    {
        try
        {
            if (dbContext.RevokedTokens.Where(q => q.Token == tokenHash).Count() != 0) return;

            RevokedToken token = new RevokedToken
            {
                Token = tokenHash,
                Expire = tokenExpire
            };

            dbContext.RevokedTokens.Add(token);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return;
        }
    }

    public static string CalculateTokenHash(JwtSecurityToken jwt) => CalculateTokenHash(jwt.RawData);

    public static string CalculateTokenHash(string token)
    {
        SHA256 sha256 = SHA256.Create();
        return BitConverter.ToString(sha256.ComputeHash(Encoding.ASCII.GetBytes(token))).Replace("-", "");
    }

    public static string CalculatePasswordHash(string password, string salt)
    {
        SHA512 sha512 = SHA512.Create();
        return BitConverter.ToString(sha512.ComputeHash(Encoding.ASCII.GetBytes(password + salt))).Replace("-", "");
    }

    public static string CalculateNewSalt()
    {
        MD5 md5 = MD5.Create();
        Random rng = new Random();
        return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(rng.Next().ToString()))).Replace("-", "");
    }

    public static bool CheckPasswordPolicy(string password)
    {
        bool lenghtOk = false;
        bool uppercaseOk = false;
        bool lowercaseOk = false;
        bool digitsOk = false;
        bool otherOk = false;

        //1- Password più lunghe di 12 caratteri
        //2- Almeno una maiuscola e una minuscola
        //3- Almeno un numero
        //4- Almeno un carattere speciale

        if (password.Length >= 12) lenghtOk = true;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) uppercaseOk = true;
            if (char.IsLower(c)) lowercaseOk = true;
            if (char.IsDigit(c)) digitsOk = true;
            if (!char.IsLetterOrDigit(c)) otherOk = true;
        }
        return lenghtOk && uppercaseOk && lowercaseOk && digitsOk && otherOk;
    }

    public static string CalculateNewApiKey()
    {
        return CalculateNewSalt();
    }

    public static async Task<string> GetApiKeyFromHttpContext(HttpRequest request, RestaurantContext context)
    {
        var found = request.Headers.TryGetValue("ApiKey", out var potentianKey);
        if (!found)
        {
            return string.Empty;
        }

        var tavolo = await context.Tables.Where(q => q.TableKey == potentianKey[0] && q.Occupied == true).FirstOrDefaultAsync();
        if (tavolo == null)
        {
            return string.Empty;
        }

        return potentianKey;
    }
}

public class JwtTokenLifetimeManager
{
    private static readonly ConcurrentDictionary<string, DateTime> DisavowedSignatures = new();

    public bool ValidateTokenLifetime(DateTime? notBefore,
                                        DateTime? expires,
                                        SecurityToken securityToken,
                                        TokenValidationParameters validationParameters)
    {
        using (RestaurantContext _context = new RestaurantContext())
        {
            if (securityToken is JsonWebToken token)
            {
                try
                {
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }
    }
}

public class RoleValidationRequirement : IAuthorizationRequirement
{
    public string[] RestrictedRoles { get; }

    public RoleValidationRequirement(params string[] restrictedRoles)
    {
        RestrictedRoles = restrictedRoles;
    }
}

public class RoleValidationHandler : AuthorizationHandler<RoleValidationRequirement>
{
    private readonly IHttpContextAccessor contextAccessor;

    public RoleValidationHandler(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleValidationRequirement requirement)
    {
        var httpContext = contextAccessor.HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var path = httpContext.Request.Path;
        var pathPart = path.ToString().Split('/');

        var userRole = context.User.FindFirst(ClaimTypes.Role);
        if (userRole == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var userRoleNumeric = int.Parse(userRole.Value);
        if (userRoleNumeric > requirement.RestrictedRoles.Select(s => int.Parse(s)).Max(m => m))
            context.Succeed(requirement);
        else
        {
            if (userRoleNumeric == 10 && (pathPart.Contains("fattureV") || pathPart.Contains("rapportini")))
                context.Succeed(requirement);
            else
                context.Fail();
        }
        return Task.CompletedTask;
    }
}

