using Microsoft.EntityFrameworkCore;
using _4Dorms.Persistance;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.implementation;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<_4DormsDbContext>((serviceProvider, options) =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

//    var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//    options.UseLoggerFactory(loggerFactory);
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<_4DormsDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, _4Dorms.Repositories.implementation.UserService>();
builder.Services.AddScoped<IDormitoryService, _4Dorms.Repositories.implementation.DormitoryService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(_4Dorms.GenericRepo.GenericRepository<>));
builder.Services.AddScoped<IDormitoryOwnerService, _4Dorms.Repositories.implementation.DormitoryOwnerService>();
builder.Services.AddScoped<IAdministratorService, _4Dorms.Repositories.implementation.AdministratorService>();
builder.Services.AddScoped<IFavoriteListService, _4Dorms.Repositories.implementation.FavoriteListService>();
builder.Services.AddScoped<IReviewService, _4Dorms.Repositories.implementation.ReviewService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IGenericRepository<Room>, GenericRepository<Room>>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IGenericRepository<Booking>, GenericRepository<Booking>>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
