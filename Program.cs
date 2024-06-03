using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetShop.Data;
using PetShop.Services.AppointmentService;
using PetShop.Services.CheckoutService;
using PetShop.Services.CommentService;
using PetShop.Services.DogItemService;
using PetShop.Services.DogProductItemService;
using PetShop.Services.EmailService;
using PetShop.Services.UserService;
using PetShop.Services.VoucherService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// add Service Imple
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.IgnoreNullValues = true;
        });
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDogItemService, DogItemService>();
builder.Services.AddScoped<IDogProductItemService, DogProductItemService>();
builder.Services.AddScoped<IDogSpeciesService, DogSpeciesService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>  
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
));

// entity framework
builder.Services.AddDbContext<PetShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PetShop123"));
});

// for identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
    }
    ).AddEntityFrameworkStores<PetShopDbContext>()
    .AddDefaultTokenProviders();

// add authen
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
// add jwt

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseCors(options => options
                .WithOrigins(new[] { "http://localhost:3000", "http://localhost:5173" })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );

app.MapControllers();

app.Run();
