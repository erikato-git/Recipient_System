using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Digst.DigitalPost.SSLClient.Clients;
using Digst.DigitalPost.Systems.RestPush.Sender.Configuration;
using Digst.DigitalPost.Systems.RestPush.Sender.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SenderSystem.Services;
using SenderSystem.UtilityLibrary.Memos.Configuration;
using SenderSystem.UtilityLibrary.Memos.Services.Logging;
using SenderSystem.UtilityLibrary.Memos.Services.MemoBuilder;
using SenderSystem.UtilityLibrary.Memos.Services.Parser;
using SenderSystem.UtilityLibrary.Memos.Services.Persistence;
using SenderSystem.UtilityLibrary.Receipts.Configuration;
using SenderSystem.UtilityLibrary.Receipts.Sender;
using SenderSystem.UtilityLibrary.Receipts.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = context =>
            {
                Claim[] claims =
                {
                    new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, context.ClientCertificate.Subject, ClaimValueTypes.String,
                        context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Exception, "Failed authentication");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton(BindSenderSystemConfiguration(builder.Configuration));
builder.Services.AddSingleton(BindRecipientSystemConfiguration(builder.Configuration));     // This DI somehow has to be there, but it's not critical for sending MeMos
//builder.Services.AddSingleton(BindMemoConfiguration(builder.Configuration));

builder.Services.AddScoped<IMemoService, ParseMemoService>();
builder.Services.AddScoped<IBusinessReceiptFactory, BusinessReceiptFactory>();
builder.Services.AddScoped<IBusinessReceiptService, BusinessReceiptService>();
builder.Services.AddScoped<IMemoBuilder, MemoBuilder>();
builder.Services.AddScoped<IMeMoPersister, MeMoPersister>();
builder.Services.AddScoped<IMemoLogging, MemoLoggingService>();
builder.Services.AddScoped<MeMoPushService>();
builder.Services.AddScoped<IReceivedBusinessReceiptLoggingService, ReceivedBusinessReceiptLoggingService>();

// DI for own services
builder.Services.AddScoped<ISendMemoService, SendMemoService>();


builder.Services.AddHttpClient<RestClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ClientCertificates.Add(LoadClientCertificate(builder.Configuration));
        return handler;
    });

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();


//Scope and execute service methods
using (var scope = app.Services.CreateScope())
{
    var memoPushService = scope.ServiceProvider.GetRequiredService<MeMoPushService>();
    //memoPushService.SendMeMo();
    //memoPushService.SendMeMoTar();
}

app.Run();



// Helper methods to bind configurations
SenderSystemConfiguration BindSenderSystemConfiguration(IConfiguration configuration)
{
    SenderSystemConfiguration config = new SenderSystemConfiguration();
    configuration.Bind("SenderSystem", config);

    return config;
}

RecipientSystemConfiguration BindRecipientSystemConfiguration(IConfiguration configuration)
{
    RecipientSystemConfiguration config = new RecipientSystemConfiguration();
    configuration.Bind("RecipientSystem", config);

    return config;
}

//static MemoConfiguration BindMemoConfiguration(IConfiguration configuration)
//{
//    //var config = new MemoConfiguration();
//    //configuration.Bind("SenderSystem:Memo", config);

//    string MemoConfigJSON = @"
//    {
//          ""NumberOfMemos"": 3,
//          ""MemoDirectoryName"": ""Artifacts"",
//          ""Header"": {
//            ""Sender"": {
//              ""Id"": ""33647166"",
//              ""IdType"": ""cvr"",
//              ""Label"": ""afsendernavn""
//            },
//            ""Recipient"": {
//              ""Id"": ""0412840565"",
//              ""IdType"": ""cpr""
//            },
//            ""Notification"": ""En notifikation"",
//            ""Label"": ""Demo fest"",
//            ""Mandatory"": false,
//            ""LegalNotification"": false
//          },
//          ""Body"": {
//            ""MainDocument"": {
//              ""Id"": ""123"",
//              ""Label"": ""Hoveddokument"",
//              ""Language"": ""da"",
//              ""EncodingFormat"": ""text/plain"",
//              ""Filename"": ""hovedfil.txt"",
//              ""Base64Content"": ""RGVyIGVyIGluZ2VuIGdydW5kIHRpbCBhdCBsw6ZzZSBkZXQgaGVyLCBkZXIgc3TDpXIgaWtrZSBub2dldCBpbnRlcmVzc2FudA==""
//            }
//          }
//    }";

//    var config = JsonSerializer.Deserialize<MemoConfiguration>(MemoConfigJSON);

//    return config;
//}

// Helper method to load client certificate
static X509Certificate2 LoadClientCertificate(IConfiguration configuration)
{
    var certificateFileName = configuration["CLIENT_CERTIFICATE_LOCATION"];
    var certificatePassword = configuration["CLIENT_CERTIFICATE_PASSWORD"];
    return new X509Certificate2(Path.Combine(certificateFileName), certificatePassword);
}
