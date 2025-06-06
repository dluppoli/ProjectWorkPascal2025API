#region  Setup
using BooksLibrary.Controllers;
using Microsoft.EntityFrameworkCore;
using ProjectWorkAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectWorkAPI.Dtos;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;

SecurityController.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var tokenLifetimeManager = new JwtTokenLifetimeManager();
builder.Services.AddSingleton<JwtTokenLifetimeManager>(tokenLifetimeManager);

builder.Services.AddDbContext<RestaurantContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        IssuerSigningKey = SecurityController.JwtSigningKey,
        LifetimeValidator = tokenLifetimeManager.ValidateTokenLifetime
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("api",
        corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("api");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
#endregion

#region Cameriere
app.MapGet("/waiter/tables", async (RestaurantContext context) =>
{
    return await context.Tables.ToListAsync();
}).RequireAuthorization();

app.MapGet("/waiter/tables/{id}", async (int id, RestaurantContext context) =>
{
    var candidate = await context.Tables.FirstOrDefaultAsync(q => q.Id == id);

    if (candidate == null)
        return Results.NotFound();

    return Results.Ok(candidate);

}).RequireAuthorization();

app.MapPut("/waiter/tables/{id}", async (int id, TableEditDto tableEdit, RestaurantContext context) =>
{
    if (id != tableEdit.Id) return Results.BadRequest();

    var candidate = await context.Tables.FirstOrDefaultAsync(q => q.Id == id);
    if (candidate == null)
        return Results.NotFound();

    if (candidate.Occupied == tableEdit.Occupied) return Results.BadRequest();

    if (tableEdit.Occupied == false)
    {
        foreach(var order in context.Orders.Where(q => q.TableId == candidate.Id && q.TableKey == candidate.TableKey && q.CompletionDate == null))
        {
            order.CompletionDate = DateTime.Now;
        }
        candidate.close();
    }
    else if (tableEdit.Occupied == true && tableEdit.Occupants != null && tableEdit.Occupants>0)
    {
        candidate.open((int)tableEdit.Occupants);
        var menuProduct = await context.Products.FirstOrDefaultAsync(q => q.Id == RestaurantController.MenuId);
        if( menuProduct == null)
        {
            return Results.BadRequest();
        }   
        context.Orders.Add( new Order
        {
            TableId = candidate.Id,
            TableKey = candidate.TableKey!,
            ProductId = menuProduct.Id,
            Qty = (int)candidate.Occupants!,
            Price = menuProduct.Price,
            OrderDate = DateTime.Now
        });
    }
    else
    {
        return Results.BadRequest();
    }

    await context.SaveChangesAsync();
    return Results.Ok(candidate);

}).RequireAuthorization();

app.MapGet("/waiter/tables/{id}/bill", async (int id, RestaurantContext context) =>
{
    return await RestaurantController.preconto(id, context, true, false);
}).RequireAuthorization();

app.MapGet("/waiter/tables/{id}/order", async (int id, RestaurantContext context) =>
{    
    return await RestaurantController.preconto(id, context, false, true);
}).RequireAuthorization();

app.MapPost("/waiter/tables/{id}/order", async (int id, List<OrderDto> orders, RestaurantContext context) =>
{
    return await RestaurantController.aggiungiPiatto(id, orders, context);

}).RequireAuthorization();

app.MapDelete("/waiter/tables/{id}/order", async (int id, RestaurantContext context) =>
{
    var candidate = await context.Tables.FirstOrDefaultAsync(q => q.Id == id);

    if (candidate == null)
        return Results.NotFound();

    if (!candidate.Occupied || candidate.Occupants == null || candidate.Occupants <= 0)
        return Results.BadRequest();

    context.Orders.RemoveRange(context.Orders.Where(q => q.TableId == id && q.TableKey == candidate.TableKey && q.ProductId!=RestaurantController.MenuId && q.CompletionDate==null));

    await context.SaveChangesAsync();
    return Results.Ok();

}).RequireAuthorization();

app.MapDelete("/waiter/tables/{id}/order/{idc}", async (int id, int idc, RestaurantContext context) =>
{
    var candidate = await context.Tables.FirstOrDefaultAsync(q => q.Id == id);

    if (candidate == null)
        return Results.NotFound();

    if (!candidate.Occupied || candidate.Occupants == null || candidate.Occupants <= 0)
        return Results.BadRequest();

    var order = await context.Orders.Where(q => q.TableId == id && q.TableKey == candidate.TableKey && q.Id == idc && q.ProductId != RestaurantController.MenuId).FirstOrDefaultAsync();
    if( order == null)
        return Results.NotFound();
    context.Orders.Remove(order);
    
    await context.SaveChangesAsync();
    return Results.Ok();

}).RequireAuthorization();

app.MapGet("/waiter/products", async (RestaurantContext context) =>
{
    return await RestaurantController.getPiatti(context);
}).RequireAuthorization();

app.MapGet("/waiter/products/{id}", async (int id, RestaurantContext context) =>
{
    return await RestaurantController.getPiatto(id, context);
}).RequireAuthorization();

app.MapGet("/waiter/categories", async (RestaurantContext context) =>
{
    return await RestaurantController.getCategorie(context);
}).RequireAuthorization();

app.MapGet("/waiter/categories/{id}", async (int id, RestaurantContext context) =>
{
    return await RestaurantController.getCategoria(id, context);
}).RequireAuthorization();

app.MapGet("/waiter/categories/{id}/products", async (int id, RestaurantContext context) =>
{   
    return await RestaurantController.getPiattiCategoria(id, context);
}).RequireAuthorization();
#endregion

#region Cliente
app.MapGet("/customer/products", async (HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();
    return await RestaurantController.getPiatti(context);
});

app.MapGet("/customer/products/{id}", async (int id, HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();
    return await RestaurantController.getPiatto(id, context);
});

app.MapGet("/customer/categories", async (HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();
    return await RestaurantController.getCategorie(context);
});

app.MapGet("/customer/categories/{id}", async (int id, HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();    
    return await RestaurantController.getCategoria(id, context);
});

app.MapGet("/customer/categories/{id}/products", async (int id, HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();
    return await RestaurantController.getPiattiCategoria(id, context);
});

app.MapGet("/customer/bill/{includiTutti}", async (int includiTutti, HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();
    return await RestaurantController.preconto(apiKey, context,true,includiTutti==1);
});

/*app.MapGet("/customer/order", async (HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();
    return await RestaurantController.preconto(apiKey, context, false, true);
});*/

app.MapPost("/customer/order", async (List<OrderDto> orders, HttpRequest request, RestaurantContext context) =>
{
    var apiKey = await SecurityController.GetApiKeyFromHttpContext(request, context);
    if (string.IsNullOrEmpty(apiKey))
        return Results.Unauthorized();

    return await RestaurantController.aggiungiPiatto(apiKey, orders, context);
});
#endregion

#region Cucina
app.MapGet("/kitchen", async (RestaurantContext context) =>
{
    var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
    return await context.ProductPrepStations.ProjectTo<ProductPrepStationDto>(mapper).ToListAsync();
}).RequireAuthorization();

app.MapGet("/kitchen/{id}", async (int id, RestaurantContext context) =>
{
    var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
    var candidate = await context.ProductPrepStations.ProjectTo<ProductPrepStationDto>(mapper).FirstOrDefaultAsync(q => q.Id == id);

    if (candidate == null)
        return Results.NotFound();

    return Results.Ok(candidate);

}).RequireAuthorization();

app.MapGet("/kitchen/{id}/order", async (int id, RestaurantContext context) =>
{
    var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
    var candidate = await context.Orders.Where(w => w.Product.IdPostazionePreparazione == id && w.CompletionDate==null && w.ProductId!=RestaurantController.MenuId).ProjectTo<OrderPrepDto>(mapper).ToListAsync();

    if (candidate == null)
        return Results.NotFound();

    return Results.Ok(candidate);

}).RequireAuthorization();

app.MapPost("/kitchen/{id}/order", async (List<OrderDto> orders, HttpRequest request, RestaurantContext context) =>
{
    foreach (var order in orders)
    {
        var candidate = await context.Orders.FirstOrDefaultAsync(q => q.Id == order.Id);

        if (candidate == null || candidate.CompletionDate != null)
            return Results.BadRequest("");

        candidate.CompletionDate = DateTime.Now;
        
    }
    await context.SaveChangesAsync();
    return Results.Ok();
}).RequireAuthorization();
#endregion

#region Statistiche
app.MapGet("/stats/tables", async (RestaurantContext context) =>
{
    return await context.Tables.ToListAsync();
}).RequireAuthorization();

app.MapGet("/stats/order", async (RestaurantContext context) =>
{
    var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
    return await context.Orders
        .Where(q => q.ProductId != RestaurantController.MenuId && q.OrderDate.DayOfYear == DateTime.Today.DayOfYear)
        .ProjectTo<OrderStatsDto>(mapper)
        .ToListAsync();
}).RequireAuthorization();

#endregion
#region  Autenticazione
app.MapPost("/login", async (UserCredentialsDto credentials, RestaurantContext context) =>
{
    User? user = await context.Users.FirstOrDefaultAsync(q => q.Username == credentials.Username && q.Password != "");

    if (user != null)
    {
        string candidatePassword = SecurityController.CalculatePasswordHash(credentials.Password.Trim(), user.Salt);

        if (user.Password.ToUpper() == candidatePassword.ToUpper())
        {
            if (!string.IsNullOrEmpty(user.SessionToken)) await SecurityController.InvalidateToken(context, user.Id);
            DateTime now = DateTime.Now;
            user.LastLogin = now;

            int tokenDaysValidity = 1;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(tokenDaysValidity),
                SigningCredentials = new SigningCredentials(SecurityController.JwtSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            if (token is JwtSecurityToken jwt)
            {
                user.SessionToken = SecurityController.CalculateTokenHash(jwt);
            }

            await context.SaveChangesAsync();

            return Results.Ok(new AuthTokenDto { Token = tokenHandler.WriteToken(token) });
        }
    }

    return Results.Unauthorized();
});

app.MapPost("/logout", async (HttpContext httpContext, RestaurantContext context) =>
{
    try
    {
        await SecurityController.InvalidateToken(context, httpContext);
        User? user = await SecurityController.GetUserFromToken(context, httpContext);
        if (user == null) return Results.NotFound();

        user.LastLogout = DateTime.Now;
        user.SessionToken = null;
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    catch (Exception)
    {
        return Results.Problem();
    }
}).RequireAuthorization();
#endregion

app.Run();

