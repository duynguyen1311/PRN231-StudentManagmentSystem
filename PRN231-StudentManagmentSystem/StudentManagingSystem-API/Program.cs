using BusinessObject.Model;
using BusinessObject.Model.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagingSystem_API.Configuration;
using StudentManagingSystem_API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SmsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext")));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<SmsDbContext>().AddRoles<IdentityRole>().AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<SignInManager<AppUser>>();
builder.Services.AddScoped<ISmsDbContext, SmsDbContext>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
app.UseApplicationDatabase<SmsDbContext>(serviceProvider);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
