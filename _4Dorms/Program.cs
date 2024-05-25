using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using _4Dorms.Persistance;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Implementation;
using _4Dorms.Repositories.implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS services and configure the policy to allow only the specified origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOriginsPolicy", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://127.0.0.1:5500") // Replace with your frontend origin
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
    });
});

var key = Encoding.ASCII.GetBytes("Rama-Issam-Boujeh-backend-project"); // Replace with your secret key
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "YourIssuer", // Replace with your issuer
        ValidAudience = "YourAudience", // Replace with your audience
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<_4DormsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDormitoryService, DormitoryService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDormitoryOwnerService, DormitoryOwnerService>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IFavoriteListService, FavoriteListService>();
builder.Services.AddScoped<IGenericRepository<FavoriteList>, GenericRepository<FavoriteList>>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IGenericRepository<Room>, GenericRepository<Room>>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IGenericRepository<Booking>, GenericRepository<Booking>>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    // Add other logging providers if needed
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestHeadersTotalSize = 32768; // Increase as necessary
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowedOriginsPolicy"); // Use the CORS policy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
