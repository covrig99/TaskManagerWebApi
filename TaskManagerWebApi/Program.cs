using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TaskManagerWebApi.AuthorizationPolicy;
using TaskManagerWebApi.AutoMapper;
using TaskManagerWebApi.DataAccessLayer;
using TaskManagerWebApi.DataAccessLayer.Implementation;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Service.Implementation;
using TaskManagerWebApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<TaskManageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<TaskManageDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();



builder.Services.AddDbContext<TaskManageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPoilicy.AdminUserPolicyName,
        policy => policy.RequireClaim(AuthorizationPoilicy.AdminUserClaimName,"true"));
    options.AddPolicy(AuthorizationPoilicy.UserPolicyName,
        policy => policy.RequireClaim(AuthorizationPoilicy.UserClaimName, "true"));

});
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
