using Microsoft.EntityFrameworkCore;
using HashidsNet;
using PickTimeTogether;
using PickTimeTogether.Models;


var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<IHashids>(s => new Hashids(salt: "TODO", minHashLength: 11));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("AppDbContext"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

var api = app.MapGroup("/api");
api.MapGet("/polls/{slug}", 
    async (IHashids hashids, AppDbContext db, string slug) =>
{
    if (!hashids.TryDecodeSingle(slug, out var id))
        return Results.NotFound();

    var obj = await db.Polls.FindAsync(id);
    if (obj is null)
        return Results.NotFound();

    var dto = new PollDto(obj);
    return Results.Ok(dto);
});
api.MapPost("/polls", 
    async (IHashids hashids, AppDbContext db, PollDto poll) =>
{
    var timeStepMinutes = 15;
    poll.Description = poll.Description?.Trim();
    if (poll.From >= poll.To)
        return Results.BadRequest("Poll duration should be valid and not zero.");
    if (poll.From.TimeOfDay.TotalMinutes % timeStepMinutes != 0)
        return Results.BadRequest($"'From' time should be adjusted to {timeStepMinutes}-minute intervals.");
    if (poll.To.TimeOfDay.TotalMinutes % timeStepMinutes != 0)
        return Results.BadRequest($"'To' time should be adjusted to {timeStepMinutes}-minute intervals.");

    var domainPoll = poll.ToDomainObject();
    db.Polls.Add(domainPoll);
    await db.SaveChangesAsync();
    var slug = hashids.Encode(domainPoll.Id);
    return Results.Created($"/api/polls/{slug}", poll);
});
api.MapPost("/polls/{slug}/respond", 
    async (IHashids hashids, AppDbContext db, string slug, PollResponseDto response) =>
{
    if (!hashids.TryDecodeSingle(slug, out var id))
        return Results.NotFound();

    var poll = await db.Polls.FindAsync(id);
    if (poll is null)
        return Results.NotFound();

    if (response.Name is null || response.Name.Length == 0)
        return Results.BadRequest("Name should be specified.");

    if (response.SelectedTime is null || response.SelectedTime.Count == 0)
        return Results.BadRequest("SelectedTime should be specified and have at least 1 element.");

    response.Name = response.Name.Trim();

    var domainResponse = response.ToDomainObject();
    domainResponse.Poll = poll;
    db.PollResponses.Add(domainResponse);

    await db.SaveChangesAsync();

    return Results.Ok();
});

api.Map("/{**page}", () => Results.NotFound());

app.MapRazorPages();

app.Run();
