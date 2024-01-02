using SEP_Web.Database;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Validation;
using SEP_Web.Models;
using SEP_Web.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddMemoryCache();
builder.Services.AddSession(x =>
{
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
});

builder.Services.AddMvc();

builder.Services.AddDbContext<SEP_WebContext>();

builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IInstituitionServices, InstituitionServices>();
builder.Services.AddScoped<IDivisionServices, DivisionServices>();
builder.Services.AddScoped<ISectionServices, SectionServices>();
builder.Services.AddScoped<ISectorServices, SectorServices>();

builder.Services.AddScoped<IUsersServices, UsersServices>();
builder.Services.AddScoped<IUserAdministratorServices, UserAdministratorServices>();
builder.Services.AddScoped<IUserEvaluatorServices, UserEvaluatorServices>();
builder.Services.AddScoped<ICivilServantServices, CivilServantServices>();
builder.Services.AddScoped<IUsersValidation, UsersValidation>();

builder.Services.AddScoped<IAssessmentServices, AssessmentServices>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCookiePolicy();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(lc2 => lc2
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File("Logs/Errors/log-error-{Date}.txt", rollingInterval: RollingInterval.Month)
    ).CreateLogger();

var logFac = app.Services.GetRequiredService<ILoggerFactory>();
logFac.AddSerilog();

app.Run();
