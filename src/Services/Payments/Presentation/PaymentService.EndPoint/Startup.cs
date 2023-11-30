using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentService.Application.Context;
using PaymentService.Persistence.Context;
using PaymentService.Application.Services;
using PaymentService.Infrastructure.MessagingBus.Base;
using PaymentService.Infrastructure.MessagingBus.ReceivedMessage.GetPaymentMessage;
using PaymentService.Infrastructure.MessagingBus.SendMessages;

namespace PaymentService.EndPoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddTransient<IPaymentDatabaseContext, PaymentDatabaseContext>();
            services.AddDbContext<PaymentDatabaseContext>(p =>
                p.UseSqlServer(Configuration["PaymentConnection"]) , ServiceLifetime.Singleton);

            services.AddScoped<IMessageBus, RabbitMqMessageBus>();
            services.AddHostedService<ReceivedPaymentForOrderMessage>();
            services.AddTransient<IPaymentService, PaymentServiceConcrete>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentService.EndPoint", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Identity:Uri"];
                    options.Audience = Configuration["Identity:Audience"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PaymentDatabaseContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentService.EndPoint v1"));

                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
