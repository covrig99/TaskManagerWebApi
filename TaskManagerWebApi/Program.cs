using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TaskManagerWebApi.AuthorizationPolicy;
using TaskManagerWebApi.AutoMapper;
using TaskManagerWebApi.DataAccessLayer;
using TaskManagerWebApi.DataAccessLayer.Implementation;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.UserRoles;
using TaskManagerWebApi.Service.Implementation;
using TaskManagerWebApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// ✅ Register DbContext (ONLY ONCE)
builder.Services.AddDbContext<TaskManageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Configure Identity
builder.Services.AddIdentity<User, IdentityRole<int>>() // Ensure roles are configured
    .AddRoles<IdentityRole<int>>() // Explicitly add roles
    .AddEntityFrameworkStores<TaskManageDbContext>()
    .AddDefaultTokenProviders();

// ✅ Configure Authentication with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing!"))
        )
    };
});

// ✅ Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPoilicyConstants.MANAGER_OR_USER_POLICY,
    policy => policy.RequireAuthenticatedUser().RequireRole(UserContatnts.MANAGER_ROLE, UserContatnts.USER_ROLE));
    
    options.AddPolicy(AuthorizationPoilicyConstants.MANAGER_POLICY,
        policy => policy.RequireAuthenticatedUser()
        .RequireClaim("role", UserContatnts.MANAGER_ROLE));
    options.AddPolicy(AuthorizationPoilicyConstants.MANAGER_POLICY, policy =>
       policy.RequireRole(UserContatnts.MANAGER_ROLE));



});

// ✅ Register Controllers
builder.Services.AddControllers();

// ✅ Add Swagger (for API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Register Services & Repositories
builder.Services.AddScoped<ITaskService, TaskService>(); // Changed to Scoped
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TaskManageDbContext>();
    var accountRepository = services.GetRequiredService<IAccountRepository>();
    var config = services.GetRequiredService<IConfiguration>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    await SeedAdminUser(accountRepository, userManager,config );
}

// ✅ Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ✅ Authentication should come before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
async Task SeedAdminUser(IAccountRepository accountRepository, UserManager<User> userManager, IConfiguration config)
{
    var adminEmail = config["AdminUser:Email"];
    var adminPassword = config["AdminUser:Password"];
    var adminRole = config["AdminUser:Role"];

    // Check if user already exists
    var existingUser = await accountRepository.FindByEmail(adminEmail);
    if (existingUser != null)
    {
        Console.WriteLine("Admin user already exists. Skipping creation.");
        return;
    }

    var adminUser = new User
    {
        UserName = adminEmail,
        Email = adminEmail,
        Role = adminRole
    };
    var result = await userManager.CreateAsync(adminUser, adminPassword);
    
    Console.WriteLine("Admin user created successfully.");
}