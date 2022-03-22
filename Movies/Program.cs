var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
//IWebHostEnvironment environment = builder.Environment;

var services = builder.Services;
// Add services to the container.
services.AddControllersWithViews();
services.AddScoped<IMovieRepo, SQLMovieRepo>();
services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>();
/*            services.Configure<IdentityOptions>(options => {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 10;
                });*/
services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MoviesDB")));
services.AddControllersWithViews().AddRazorRuntimeCompilation();
services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Home/Index";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
