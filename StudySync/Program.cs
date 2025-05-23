using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudySync.Data;
using StudySync.Models;
using StudySync.Repositories;
using StudySync.Services;

var builder = WebApplication.CreateBuilder(args); //Middlewear where we inject dependencies:

// Add services to the container.

builder.Services.AddControllers();

//HERE ADD THE DEPENDENCY INJECTION TO DB, THAN TO appsetings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//HERE ADD THE REPISIOTRY BEFORE IT BUILDS
builder.Services.AddScoped<ITaskItemRepository,TaskItemRepository>(); // now i have new service, so when in other layers we can use it , so we can pass only the interface
builder.Services.AddScoped<IUserReposiotry, UserRepository>(); 
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); 
builder.Services.AddScoped<ISubtaskRepository, SubtaskRepository>(); 
builder.Services.AddScoped<IReminderRepository, ReminderRepository>(); 

//HERE ADD SERVICES ,so we can access what service does
builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<ISubtaskService, SubtaskService>();


//ADDING AUTOMAPPER as a package: I need also automapper to run
builder.Services.AddAutoMapper(typeof(Program));

//Add Identity 
//Step 1:
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Step 2: Add authentication on top of idenity 
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }
).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //check if request came from trusted souce
        ValidateAudience = true, //check if token is meant for my server/app
        ValidateLifetime = true, // make sure the token  isnt expired
        ValidateIssuerSigningKey = true, //make sure the token was not modified
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //pulled issuer from appsetings json
        ValidAudience = builder.Configuration["Jwt:Audience"], //pull audience from appsettings json
        IssuerSigningKey =
                  new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Step 3: First Authenticate than Authorize
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
