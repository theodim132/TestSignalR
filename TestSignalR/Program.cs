using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestSignalR.Data;
using TestSignalR.Hubs;
using TestSignalR.Models;
using TestSignalR.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<ApplicationUser>()
        .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddSingleton<ServiceBusListener>(sp =>
{
    // Replace these with your actual Service Bus connection string and queue name
    string serviceBusConnectionString = "Endpoint=sb://my-argo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Qrokrlyxg/QF0qLjnX3TUbhOS3UG3tE5Q+ASbGjOh54=";
    string queueName = "create";

    return new ServiceBusListener(serviceBusConnectionString, queueName);
});

builder.Services.AddSingleton<ServiceBusHub>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<ChatHub>("/chathub"); // Add this line
app.MapHub<TestHub>("/testhub"); // Add this line
app.MapHub<ServiceBusHub>("/servicebushub"); // Add this line
app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
