using JH_Challenge.Web.Hubs;
using JH_Challenge.Web.Services;
using JH_Challenge.Web.Services.TwitterService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

//Register Twitter Service
var twitterService = builder.Configuration.GetValue<string>("TwitterService");
switch (twitterService.ToLower())
{
    case "tweetinvi":
    case "tweetinviservice":
        builder.Services.AddSingleton<ITwitterService, TweetinviService>();
        break;
    default:
        builder.Services.AddSingleton<ITwitterService, TwitterService>();
        break;  
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<TwitterHub>("/Twitter");

app.Run();
