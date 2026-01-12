using GeminiChat.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CHANGED SECTION STARTS HERE ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin() // changed from WithOrigins(...)
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
// --- CHANGED SECTION ENDS HERE ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- UPDATE THIS LINE TOO ---
app.UseCors("AllowAll"); 

app.UseAuthorization();
app.MapControllers();

app.Run();