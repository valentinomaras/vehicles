using Microsoft.EntityFrameworkCore;
using VehicleManagement.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sgo => {
    var o = new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Vehicle Management API",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "support@vehiclemanagement.com",
            Name = "Vehicle Management Support"
        },
        Description = "API for managing vehicles, brands, and models",
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "Vehicle Management License"
        }
    };
    sgo.SwaggerDoc("v1", o);
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    sgo.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Configure the DbContext with SQL Server
builder.Services.AddDbContext<VehicleManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleManagementContext"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
    });
//}


app.UseHttpsRedirection();

app.UseStaticFiles();  // Serve static files from wwwroot folder
app.UseDefaultFiles();  // Allow serving default files like index.html
app.UseCors("CorsPolicy");  // Enable CORS

app.UseRouting();
app.UseAuthorization();

app.UseDeveloperExceptionPage();

app.MapControllers();
app.MapFallbackToFile("index.html");  // Fallback to index.html for any unmatched routes

app.Run();
