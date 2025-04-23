using SignalRChat.Hubs; // 1
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

////

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(
    opt => opt.UseSqlite("DataSource=Users.db")
);

builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
);

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AuthDbContext>(); ;

builder.Services.AddSignalR(); // 2

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config => {
    config.DocumentName = "ChatLogIn";
    config.Title = "ChatLogIn V1";
    config.Version = "v1";
});

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; //4
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
}); // 4


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins); // 5
app.UseOpenApi();
app.UseSwaggerUi(config =>
{   config.DocumentTitle = "ChatApp";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();
app.MapHub<ChatHub>("/chatHub"); // 3

app.Run();
