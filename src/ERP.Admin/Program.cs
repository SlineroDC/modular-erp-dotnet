// --- 1. Import necessary namespaces ---
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ERP.Core.Interfaces;            
using ERP.Infrastructure;              // For the ApplicationDbContext
using ERP.Infrastructure.Repositories; // For the Repository Implementations (the "how")
using ERP.Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

// --- 2. Configure Services (Dependency Injection Container) ---

// Load the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure the main DbContext (ApplicationDbContext from ERP.Infrastructure)
// and tell it to use PostgresSQL (Npgsql).
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure ASP.NET Core Identity for user authentication and roles.
// IMPORTANT: Tell Identity to use *our* existing ApplicationDbContext
// so all tables (yours and Identity's) live in the same DB.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Adds useful error pages for database operations during development.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- Register our custom services (Dependency Injection) ---
// This tells the app: "When a PageModel asks for an IClientRepository,
// give it an instance of ClientRepository."
// 'AddScoped' means one instance per HTTP request.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddHttpClient<IAiService, GeminiAiService>();
builder.Services.AddScoped<IEmailService,SmtpEmailService>();

builder.Services.AddControllers();
// Add support for Razor Pages.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
});

// --- 3. Build the App and Configure the HTTP Pipeline ---
var app = builder.Build();

var defaultCulture = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = defaultCulture;

//Auto migration for docker
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        context.Database.Migrate();
        Console.WriteLine("Database migrated successfully.");

        var adminEmail = "admin@firmeza.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            // ¡Esta será tu contraseña maestra!
            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded) Console.WriteLine("✅ Usuario Admin creado: admin@firmeza.com / Admin123!");
            else
                Console.WriteLine("Error creando Admin: " +
                                  string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error to migrate databases: {ex.Message}");
        throw;
    }
}

// Configure the HTTP request pipeline.
// Show detailed error pages only when in Development.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Helps with applying EF migrations.
}
else
{
    // Use a friendly error page in production.
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforce HTTPS.
}

app.UseStatusCodePagesWithRedirects("/Notfound");

app.MapGet("/", async context =>
{
        // Si ya está logueado, va al Dashboard (Index)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            context.Response.Redirect("/Index");
        }
        else
        {
            // Si no, va al Login
            context.Response.Redirect("/Identity/Account/Login");
        }
        await Task.CompletedTask;
    
});


//app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files (CSS, JS, images) from the wwwroot folder.
app.UseRouting(); // Enable routing.
app.UseAuthentication(); // Enable user authentication.
app.UseAuthorization(); // Enable authorization checks (must be after UseRouting).
app.MapRazorPages(); // Set up endpoints for all Razor Pages.
app.MapControllers(); // Set up controllers for the widget AI
app.Run(); // Start the application.