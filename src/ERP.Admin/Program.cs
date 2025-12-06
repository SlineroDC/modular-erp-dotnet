// --- 1. Import necessary namespaces ---
using ERP.Core.Interfaces;
using ERP.Infrastructure; // For the ApplicationDbContext
using ERP.Infrastructure.Repositories; // For the Repository Implementations (the "how")
using ERP.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 2. Configure Services (Dependency Injection Container) ---

// Load the connection string from appsettings.json
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure the main DbContext (ApplicationDbContext from ERP.Infrastructure)
// and tell it to use PostgresSQL (Npgsql).
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// Configure Identity (authentication and user management)
builder
    .Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        options.SignIn.RequireConfirmedAccount = true
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Adds useful error pages for database operations during development.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- Register our custom services (Dependency Injection) ---
// 'AddScoped' means one instance per HTTP request.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddHttpClient<IAiService, GeminiAiService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

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
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
            };
            // This will be your master password!
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
                Console.WriteLine("✅ Admin user created: admin@firmeza.com / Admin123!");
            else
                Console.WriteLine(
                    "Error creating Admin: "
                        + string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }

        // 3. Seed Test Client User (to be able to login and test the buying process)
        var clientEmail = "cliente@test.com";
        var clientUser = await userManager.FindByEmailAsync(clientEmail);

        if (clientUser == null)
        {
            // A. Crear Login (Identity)
            clientUser = new IdentityUser
            {
                UserName = clientEmail,
                Email = clientEmail,
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(clientUser, "Cliente123!");
            Console.WriteLine("✅ Test Client user created: cliente@test.com / Cliente123!");

            // B. Crear Datos de Negocio (Tabla Customers)
            var customerRepo = services.GetRequiredService<ICustomerRepository>();

            var testCustomer = new ERP.Core.Entities.Customer
            {
                Name = "Usuario",
                LastName = "Prueba",
                Email = clientEmail,
                IdDocument = "999999999",
                Phone = "3000000000",
                Address = "Calle Falsa 123",
                IsActive = true,
            };

            await customerRepo.AddAsync(testCustomer);
        }

        // 4. Seed Dummy Customers (Para que la tabla de Admin no esté vacía)
        if (!context.Customers.Any(c => c.Email != clientEmail)) // Si solo está el de prueba o ninguno
        {
            var dummyCustomers = new List<ERP.Core.Entities.Customer>
            {
                new()
                {
                    Name = "Juan",
                    LastName = "Perez",
                    Email = "juan.p@test.com",
                    IdDocument = "1001",
                    Phone = "3001234567",
                    Address = "Calle 1 #2-3",
                    IsActive = true,
                },
                new()
                {
                    Name = "Maria",
                    LastName = "Gomez",
                    Email = "maria.g@test.com",
                    IdDocument = "1002",
                    Phone = "3101234567",
                    Address = "Av Siempre Viva 123",
                    IsActive = true,
                },
                new()
                {
                    Name = "Constructora",
                    LastName = "El Bloque SAS",
                    Email = "contacto@bloque.com",
                    IdDocument = "900123456",
                    Phone = "6012345678",
                    Address = "Zona Industrial Lote 4",
                    IsActive = true,
                },
                new()
                {
                    Name = "Luis",
                    LastName = "Diaz",
                    Email = "lucho@test.com",
                    IdDocument = "1005",
                    Phone = "3151112233",
                    Address = "Calle 10 #20-30",
                    IsActive = true,
                },
                new()
                {
                    Name = "Sofia",
                    LastName = "Vergara",
                    Email = "sofia@test.com",
                    IdDocument = "1006",
                    Phone = "3164445566",
                    Address = "Barrio Alto #101",
                    IsActive = true,
                },
                new()
                {
                    Name = "Ferreteria",
                    LastName = "La Tuerca",
                    Email = "ventas@latuerca.com",
                    IdDocument = "900987654",
                    Phone = "6025556677",
                    Address = "Centro Comercial Local 1",
                    IsActive = true,
                },
            };

            context.Customers.AddRange(dummyCustomers);
            context.SaveChanges();
            Console.WriteLine($"✅ Seeded {dummyCustomers.Count} dummy customers for testing.");
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
    app.UseMigrationsEndPoint();
}
else
{
    // Use a friendly error page in production.
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforce HTTPS.
}

app.UseStatusCodePagesWithRedirects("/Notfound");

app.MapGet(
    "/",
    async context =>
    {
        // If already logged in, go to the Dashboard (Index)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            context.Response.Redirect("/Index");
        }
        else
        {
            // Otherwise, go to Login
            context.Response.Redirect("/Identity/Account/Login");
        }
        await Task.CompletedTask;
    }
);

//app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files (CSS, JS, images) from the wwwroot folder.
app.UseRouting(); // Enable routing.
app.UseAuthentication(); // Enable user authentication.
app.UseAuthorization(); // Enable authorization checks (must be after UseRouting).
app.MapRazorPages(); // Set up endpoints for all Razor Pages.
app.MapControllers(); // Set up controllers for the widget AI
app.Run(); // Start the application.
