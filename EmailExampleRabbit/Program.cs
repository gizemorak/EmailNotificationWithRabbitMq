using Bus.Shared.RabbitService;
using Bus.Sharedd.Events;
using Bus.Sharedd.Options;
using EmailExampleRabbit.Services.EmailService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQApp.API.Consumers;

namespace EmailExampleRabbit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ServiceBusOption>(
    builder.Configuration.GetSection(nameof(ServiceBusOption)));

            builder.Services.AddSingleton<ServiceBusOption>(sp =>
            {
                IOptions<ServiceBusOption> optionsServiceBus = sp.GetRequiredService<IOptions<ServiceBusOption>>();
                return optionsServiceBus.Value;
            });

            builder.Services.AddScoped<IExtendEmailSender, EmailSender>(i =>
                new EmailSender(
                    builder.Configuration["EmailSender:Host"], 
                    builder.Configuration.GetValue<int>("EmailSender:Port"),
                    builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    builder.Configuration.GetValue<string>("EmailSender:Username"),
                    builder.Configuration.GetValue<string>("EmailSender:Password"))
            );

       



            builder.Services.AddSingleton<IBusService, RabbitMqBusService>(sp =>
            {
                ServiceBusOption serviceBusOptions = sp.GetRequiredService<ServiceBusOption>();

                RabbitMqBusService rabbitMqBus = new RabbitMqBusService(serviceBusOptions);
                rabbitMqBus.Init().Wait();
                rabbitMqBus.CreateExchanges().Wait();
                return rabbitMqBus;
            });



            builder.Services.AddHostedService<EmailSendEventConsumer>();

      

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


       
            app.UseHttpsRedirection();

            app.UseAuthorization();

 

                app.MapGet("/mailsend", async (IBusService busService) =>
                {
                    await busService.Publish(new MailSendEvent("test.mail@testmail.com","test konu","test mesaj"));

                    return Results.Ok();
                })
            .WithName("SendMail")
            .WithOpenApi();

            app.Run();
        }
    }
}
