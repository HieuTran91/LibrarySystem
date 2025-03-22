using LibraryProject.DTOs;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryProject.Services.LibraryService;
using LibraryProject.Repositories.GenericRepository;
using LibraryProject.Services.AuthService;
using LibraryProject.Repositories.LoginRepository;
using Microsoft.AspNetCore.Identity;
using LibraryProject.Repositories.UserRepository;
using LibraryProject.Services.BorrowingService;
using System.Security.Claims;
using LibraryProject.Services.UserService;
using LibraryProject.Services.FirebaseService;
using LibraryProject.Services.NotificationService;
using LibraryProject.Services.PaymentService;
using LibraryProject.Services.ReviewService;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddRazorPages();

//Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile));


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<Role>, GenericRepository<Role>>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IRepository<Genre>, GenericRepository<Genre>>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IRepository<Book>, GenericRepository<Book>>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

builder.Services.AddScoped<IRepository<Notification>, GenericRepository<Notification>>();
builder.Services.AddHostedService<OverdueNotificationService>();
builder.Services.AddScoped<INotificationObserver, NotificationService>();

builder.Services.AddScoped<IRepository<Borrowing>, GenericRepository<Borrowing>>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();

builder.Services.AddScoped<IRepository<PaymentMethod>, GenericRepository<PaymentMethod>>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IRepository<Payment>, GenericRepository<Payment>>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IRepository<Review>, GenericRepository<Review>>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddSingleton<FirebaseStorageService>();
builder.Services.AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();

// Configuration Authentication with JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

//builder.Services.AddAuthorization(options => {
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("ReaderPolicy", policy => policy.RequireRole("Reader"));
//    options.AddPolicy("AdminLibrarianPolicy", policy => policy.RequireRole("Admin", "Librarian"));
//    options.AddPolicy("AllUsersTypePolicy", policy => policy.RequireRole("Admin", "Librarian", "Reader"));
//});

var firebaseConfig = builder.Configuration.GetSection("Firebase");
var credentialsPath = firebaseConfig["CredentialsPath"];

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session sống
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseSession();

app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});

app.Run();
