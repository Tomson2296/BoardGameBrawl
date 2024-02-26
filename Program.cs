using AutoMapper;
using BoardGameBrawl.Data;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using BoardruleBrawl.Data.Stores.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AppDBConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add normalizer -> changing all normalized fields in Identity APi tables from uppercase to lowercase
builder.Services.AddSingleton<ILookupNormalizer, CustomNormalizer>();

// Add DBContext -> creating ApplicationDBContext and all neccessary tables using Entity Framework Code First approach
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity features to web application
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;

    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(120);
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Add SecurityStampValidator feature to checking logged user credentials every 30 minutes 
builder.Services.Configure<SecurityStampValidatorOptions>(options => {
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

// Add Default implementation of UserEmailStore for custom ApplicationUser class
// builder.Services.AddScoped<IUserEmailStore<ApplicationUser>, ApplicationUserEmailStore>();

// Add Reimplemenented UserCLaimsPrincipialFactory to get more claims about the user 
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
        AppUserClaimsPrincipalFactory>();

// Add Custom UserFriendsService to handle all users friendship related methods in web application
builder.Services.AddScoped<IUserFriendStore<UserFriend, ApplicationUser>, UserFriendStore>();
//builder.Services.AddScoped<UserFriendService>();

// Add Custom BoardgameService to handle all Boardgame related methods (adding, removing, updating) boardgame objects in web application
builder.Services.AddScoped<IBoardGameStore<BoardgameModel>, BoardGameStore>();
//builder.Services.AddScoped<BoardGameService>();

// Add Custom MatchmakingRuleService to hangle methods (adding, modifing, deleting) matchmaking rules in web application
builder.Services.AddScoped<IMatchmakingRuleStore<MatchmakingRule>, MatchmakingRuleStore>();
//builder.Services.AddScoped<MatchmakingRuleService>();

// Add Custom BoardgameRuleService to handle management (adding, modifing, deleting) matchmaking rules applied to specified boardgame
builder.Services.AddScoped<IBoardgameRuleStore<BoardgameRule, BoardgameModel, MatchmakingRule>, BoardgameRuleStore>();
//builder.Services.AddScoped<BoardgameRuleService>();

// Add Custom GroupService to handle all Group related methods in web application
builder.Services.AddScoped<IGroupStore<GroupModel>, GroupStore>();
//builder.Services.AddScoped<GroupService>();

// Add Custom GroupParticipantService to handle adding users to/removing users from groups in web application
builder.Services.AddScoped<IGroupParticipantStore<GroupParticipant, GroupModel, ApplicationUser>, GroupParticipantStore>();
//builder.Services.AddScoped<GroupParticipantService>();

// Add Custom MessageService to handle all Message related methods (creating, deleting and retrieving messages) from application database
builder.Services.AddScoped<IMessageStore<MessageModel, ApplicationUser>, MessageStore>();
//builder.Services.AddScoped<MessageService>();

// Add Custom UserNotificationService to handle notification methods in web application
builder.Services.AddScoped<IUserNotificationStore<UserNotification, ApplicationUser>, UserNotificationStore>();
//builder.Services.AddScoped<UserNotificationService>();

// Add Custom MatchService to handle all methods related to matches in application
builder.Services.AddScoped<IMatchStore<MatchModel, BoardgameModel, ApplicationUser>, MatchStore>();
//builder.Services.AddScoped<MatchService>();

// Add Custom UserRatingService to handle all methods realted to checking user progression in boardgames
builder.Services.AddScoped<IUserRatingStore<UserRating, ApplicationUser, BoardgameModel>, UserRatingStore>();

// Add Custom UserScheduleService to handle all methods related to handling user schedule in application
builder.Services.AddScoped<IUserScheduleStore<UserSchedule, ApplicationUser>, UserScheduleStore>();
//builder.Services.AddScoped<UserScheduleService>();

// Add Custom UserGeolocationService to handle all methods related to User geolocation data in application
builder.Services.AddScoped<IUserGeolocationStore<UserGeolocation, ApplicationUser>, UserGeolocationStore>();


// Add Automapper framework to aplication
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));
var AutomapperConfig = new MapperConfiguration(cfg => {
    cfg.AddProfile<MappingProfiles>();
});
AutomapperConfig.AssertConfigurationIsValid();
AutomapperConfig.CreateMapper();

// Add Session feature to web application. Setting Idle Timeout for specified session to 30minutes (default value - 20minutes)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "SessionCookie";
});

// Add Authnetication feature -> external login to web application using Google and Facebook external authentication service

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.Name = "ApplicationCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict; 
        
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration.GetSection("Facebook:AppId").Value!;
        options.AppSecret = builder.Configuration.GetSection("Facebook:AppSecret").Value!;
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration.GetSection("Google:ClientId").Value!;
        options.ClientSecret = builder.Configuration.GetSection("Google:ClientSecret").Value!;
    });

// for other authentication purposes you can use ConfigureApplicationCookie - multiple authentication sources acenario
//builder.Services.ConfigureApplicationCookie(options =>
//{
//});

// Add Custom DateService to process additional info about ApplicationUser - availability for playing according to data passed in user schedule 
builder.Services.AddScoped<IDateService, DateService>();

// Add Email Service to web application - using MailKit package 
builder.Services.AddScoped<IMailKitService, MailKitService>();

// Add BGGSerice to web application - to handle all BGG related http request (getting boardgameInfo, user BGG collection)
builder.Services.AddScoped<IBGGAPIService, BGGAPIService>();

// Add NominativGeolocationAPI to web application - to handle all methods related to get info about geolocalization 
builder.Services.AddScoped<INominatimGeolocationAPI, NominatimGeolocationAPI>();

// Add Google StaticMaps API to get a image binary data that can be represented on web page / saved in database 
builder.Services.AddScoped<IGoogleStaticMapsService, GoogleStaticMapsService>();

// Add HttpClient service to implement making a Http request to external API's
// Extending options to AddHttpClient to provide an custom named HttpClient

builder.Services.AddHttpClient("BoardGameGeekClient", client => 
{
    client.BaseAddress = new Uri("https://boardgamegeek.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/xml");
});

builder.Services.AddHttpClient("NominativeGeolocationAPICLient", client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.Add("Accept", "application/xml");
});

builder.Services.AddHttpClient("GoogleStaticMapsAPI", client =>
{
    client.BaseAddress = new Uri("https://maps.googleapis.com/");
    client.DefaultRequestHeaders.Add("Accept", "image/png");
});


builder.Services.AddRazorPages();

// Add HTTPS Redirection service and setting a default https port
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();

    // Seeding database with defualt user, roles and boardgames at application startup
    //await app.SeedSqlServer();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Use cookie session feature in web application. app.UseSession() has to be placed before mapping options and after app.UseRouting()
app.UseSession();

app.MapRazorPages();

app.Run();