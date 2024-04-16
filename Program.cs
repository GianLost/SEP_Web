using SEP_Web.Interfaces.UsersInterfaces;
using SEP_Web.Interfaces.StructuresInterfaces;
using SEP_Web.Interfaces.LicensesInterfaces;
using SEP_Web.Interfaces.AssessmentsInterfaces;
using SEP_Web.Interfaces.ReportsInterfaces;
using SEP_Web.Services.UsersRepository;
using SEP_Web.Services.StructuresRepository;
using SEP_Web.Services.LicensesRepository;
using SEP_Web.Services.AssessmentsRepository;
using SEP_Web.Services.ReportsRepository;
using SEP_Web.Database;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Validation;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseIISIntegration().UseUrls("https://192.168.100.183:38450"); //Descomente e altere o Ip para o ip da máquina que será responsável por executar a aplicação no IIS Server

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

builder.Services.AddScoped<IFilterServices, FilterServices>();
builder.Services.AddScoped<LoggedinUserFilter>();
builder.Services.AddScoped<UserAdminFilter>();
builder.Services.AddScoped<UserEvaluatorFilter>();
builder.Services.AddScoped<AssessmentFilter>();

builder.Services.AddScoped<IInstituitionServices, InstituitionServices>();
builder.Services.AddScoped<IDivisionServices, DivisionServices>();
builder.Services.AddScoped<ISectionServices, SectionServices>();
builder.Services.AddScoped<ISectorServices, SectorServices>();

builder.Services.AddScoped<IUsersServices, UsersServices>();
builder.Services.AddScoped<IUserAdministratorServices, UserAdministratorServices>();
builder.Services.AddScoped<IUserEvaluatorServices, UserEvaluatorServices>();
builder.Services.AddScoped<ICivilServantServices, CivilServantServices>();
builder.Services.AddScoped<IUsersValidation, UsersValidation>();

builder.Services.AddScoped<ILicenseServices, LicenseServices>();
builder.Services.AddScoped<IServantLicenseServices, ServantLicenseServices>();

builder.Services.AddScoped<IAssessmentServices, AssessmentServices>();

builder.Services.AddScoped<IReportServices, ReportServices>();

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

app.Run();