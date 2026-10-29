using AppLogin.Libraries.Login;
using AppLogin.Libraries.Middleware;
using AppLogin.Repository;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add a interface como um serviço
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>{
    //definir um tempo para duração
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    //mostrar para o navegador que o cookie é essencial
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().AddSessionStateTempDataProvider();

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AppLogin.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();
builder.Services.AddScoped<LoginColaborador>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// controllers mvc e api
app.MapControllers();

app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name:"areas",
    pattern:"{area:exists}/{controller=Home}/{action=Index}/{Id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
