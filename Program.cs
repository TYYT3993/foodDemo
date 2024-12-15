using FoodOrderAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// �K�[��Ʈw�W�U��
builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// �K�[ CORS �䴩
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// �s�W�H�U�{���X�A�]�w���κ�ť�Ҧ� IP ��}
builder.WebHost.UseUrls("https://localhost:7239");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �ϥ� HTTPS ���w�V
app.UseHttpsRedirection();

// �ϥ� CORS�]�����b MapControllers ���e�^
app.UseCors("AllowAll");

// �ϥα��v
app.UseAuthorization();

// �M�g���
app.MapControllers();

app.Run();
