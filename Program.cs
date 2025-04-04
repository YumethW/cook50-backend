using cook50_backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontEnd",
        policy =>
        {
            policy.WithOrigins("https://cook50-lac.vercel.app/").AllowAnyMethod().AllowAnyHeader();
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddSingleton<GeminiService>();

var app = builder.Build();

app.UseCors("AllowFrontEnd");
app.UseRouting();
app.MapControllers();
app.Run();
