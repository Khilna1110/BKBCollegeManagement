using BKBCollegeManagement.Models;
using BKBCollegeManagement.Repository;
using BKBCollegeManagement.Services;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddPolicy("TheCodeBuzzPolicy", builder =>
{
    builder.WithOrigins("http://localhost:4200")
             .WithHeaders("Origin", "X-Api-Key", "X-Requested-With", "Content-Type", "Accept", "Authorization")
            .WithMethods("GET", "POST", "DELETE", "OPTIONS")
            
    ;
}));
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<CourseContext>(opt =>
  //  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BKBEduSimplified", Version = "v1" });
});

builder.Services.AddTransient<IAzureStorage, AzureStorage>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BKBEduSimplified v1"));
// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BKBEduSimplified v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("TheCodeBuzzPolicy");
//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireCors("TheCodeBuzzPolicy");

});

app.Run();