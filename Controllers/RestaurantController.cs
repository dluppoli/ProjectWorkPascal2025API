using System.Reflection.Metadata;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectWorkAPI.Dtos;
using ProjectWorkAPI.Models;

public static class RestaurantController
{
    public static int MenuId = 1;
    public static async Task<IResult> getPiatti(RestaurantContext context)
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
        return Results.Ok(await context.Products.Where(w => w.Id != MenuId).ProjectTo<ProductDto>(mapper).ToListAsync());
    }

    public static async Task<IResult> getPiatto(int id, RestaurantContext context)
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
        var candidate = await context.Products.ProjectTo<ProductDto>(mapper).FirstOrDefaultAsync(q => q.Id == id && id != MenuId);
        if (candidate == null)
            return Results.NotFound();
        return Results.Ok(candidate);
    }

    public static async Task<IResult> getCategorie(RestaurantContext context)
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
        return Results.Ok(await context.Categories.Where(w => w.Id != MenuId).OrderBy(ob => ob.OrderIndex).ProjectTo<CategoryDto>(mapper).ToListAsync());
    }

    public static async Task<IResult> getCategoria(int id, RestaurantContext context)
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
        var candidate = await context.Categories.ProjectTo<CategoryDto>(mapper).FirstOrDefaultAsync(q => q.Id == id && id != MenuId);
        if (candidate == null)
            return Results.NotFound();
        return Results.Ok(candidate);
    }

    public static async Task<IResult> getPiattiCategoria(int id, RestaurantContext context)
    {
        var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
        return Results.Ok(await context.Products.Where(q => q.IdCategory == id && id != MenuId).ProjectTo<ProductDto>(mapper).ToListAsync());
    }

    public static async Task<IResult> preconto(int id, RestaurantContext context, bool includiMenu, bool includiPrezzi0)
    {
        var candidate = await context.Tables.FirstOrDefaultAsync(q => q.Id == id);
        return await preconto(candidate, context, includiMenu, includiPrezzi0);
    }

    public static async Task<IResult> preconto(string tableKey, RestaurantContext context, bool includiMenu, bool includiPrezzi0)
    {
        var candidate = await context.Tables.FirstOrDefaultAsync(q => q.TableKey == tableKey);
        return await preconto(candidate, context, includiMenu, includiPrezzi0);
    }

    private static async Task<IResult> preconto(Table? candidate, RestaurantContext context, bool includiMenu, bool includiPrezzi0)
    {
        if (candidate == null)
            return Results.NotFound();

        if (!candidate.Occupied || candidate.Occupants == null || candidate.Occupants <= 0)
            return Results.BadRequest();

        var mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
        var order = await context.Orders.Include(i => i.Product)
            .Where(q => q.TableId == candidate.Id && q.TableKey == candidate.TableKey && (includiMenu || q.ProductId != MenuId))
            //.Where(q => comandaCompleta == true || q.Price > 0)
            .OrderBy(ob => ob.OrderDate)
            .ProjectTo<OrderDto>(mapper)
            .ToListAsync();

        var OrderSummary = new OrderSummary
        {
            TableId = candidate.Id,
            Occupants = candidate.Occupants ?? 0,
            TotalPrice = order.Sum(q => q.Price * q.Qty),
            Orders = order.Where(q => includiPrezzi0 == true || q.Price > 0).ToList(),
        };

        return Results.Ok(OrderSummary);
    }

    public static async Task<IResult> aggiungiPiatto(int id, List<OrderDto> orders, RestaurantContext context)
    {
        var candidate = await context.Tables.FirstOrDefaultAsync(q => q.Id == id);
        return await aggiungiPiatto(candidate, orders, context);
    }
    public static async Task<IResult> aggiungiPiatto(string tableKey, List<OrderDto> orders, RestaurantContext context)
    {
        var candidate = await context.Tables.FirstOrDefaultAsync(q => q.TableKey == tableKey);
        return await aggiungiPiatto(candidate, orders, context);
    }

    private static async Task<IResult> aggiungiPiatto(Table? candidate, List<OrderDto> orders, RestaurantContext context)
    {
        if (candidate == null)
            return Results.NotFound();

        if (!candidate.Occupied || candidate.Occupants == null || candidate.Occupants <= 0)
            return Results.BadRequest();

        foreach (var order in orders)
        {
            if (order.Qty <= 0 || order.ProductId == MenuId)
                return Results.BadRequest();

            var product = await context.Products.FirstOrDefaultAsync(q => q.Id == order.ProductId);
            if (product == null)
                return Results.BadRequest();

            context.Orders.Add(new Order
            {
                TableId = candidate.Id,
                TableKey = candidate.TableKey!,
                ProductId = product.Id,
                Qty = order.Qty,
                Price = product.Price,
                OrderDate = DateTime.Now
            });
        }
        await context.SaveChangesAsync();
        return Results.Ok();
    }
}