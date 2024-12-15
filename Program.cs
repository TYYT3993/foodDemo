using FoodOrderAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 添加資料庫上下文
builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 添加 CORS 支援
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// 新增以下程式碼，設定應用監聽所有 IP 位址
builder.WebHost.UseUrls("https://localhost:7239");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 使用 HTTPS 重定向
app.UseHttpsRedirection();

// 使用 CORS（必須在 MapControllers 之前）
app.UseCors("AllowAll");

// 使用授權
app.UseAuthorization();

// 映射控制器
app.MapControllers();

app.Run();
