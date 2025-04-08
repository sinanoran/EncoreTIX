using EncoreTIX.Models.TicketMaster;
using EncoreTIX.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<APIOptions>(builder.Configuration.GetSection("TicketMaster"));
builder.Services.AddScoped<ITicketMasterService, TicketMasterService>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("TicketMaster", client =>
{
    client.BaseAddress = new Uri("https://app.ticketMaster.com/discovery/v2/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Splash}/{id?}");

app.Run();
