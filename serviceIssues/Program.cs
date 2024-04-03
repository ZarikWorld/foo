using serviceIssues.Source.Data.GitData;
using serviceIssues.Source.Service;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.AllowSynchronousIO = true;
});

//builder.Services.AddScoped<serviceIssue.Service.ServiceIssue>();

// Add WSDL support
builder.Services.AddServiceModelServices().AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

// Configure an explicit none credential type for WSHttpBinding as it defaults to Windows which requires extra configuration in ASP.NET
var myWSHttpBinding = new WSHttpBinding(SecurityMode.Transport);
myWSHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

app.UseServiceModel(builder =>
{
    builder.AddService<serviceIssues.Service.ServiceIssue>((serviceOptions) => { })
    .AddServiceEndpoint<serviceIssues.Service.ServiceIssue, IServiceIssue>(new BasicHttpBinding(), "/ServiceIssue/basichttp")
    .AddServiceEndpoint<serviceIssues.Service.ServiceIssue, IServiceIssue>(myWSHttpBinding, "/ServiceIssue/WSHttps");
});

var serviceMetadataBehavior = app.Services.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;

AppConfig.Initialize();


app.Run();