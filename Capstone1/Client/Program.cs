global using Capstone1.Shared;
global using System.Net.Http.Json;
global using Capstone1.Client.Services.AuthService;
global using Capstone1.Client.Services.LanguageService;
global using Microsoft.AspNetCore.Components.Authorization;
global using Capstone1.Client.Services.InterestService;
global using Capstone1.Client.Services.StudentService;
global using Capstone1.Client.Services.PostService;
global using Capstone1.Client.Services.TagService;
global using Capstone1.Client.Services.TeacherService;
global using Capstone1.Client.Services.ProposalService;
global using Capstone1.Client.Services.ChatService;
using Capstone1.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IInterestService, InterestService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IProposalService, ProposalService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
