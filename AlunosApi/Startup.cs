using AlunosApi.Context;
using AlunosApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace AlunosApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); }); // -para fazer uso do SQLServer

            services.AddCors();
            services.AddScoped<IAlunoService, AlunoService>(); // -diz ao container injetor de dependecia que ao referenciar um IAlunoService, ele implementara os metodos da Classe AlunoService
            services.AddScoped<IAuthenticate, AuthenticateService>();


            // Adiciona a configura��o padr�o para o os tipos em seu construtor, que representam o perfil do usuario

            services.AddIdentity<IdentityUser, IdentityRole>() //-- Contem as prop do User que ira autenticar
                .AddEntityFrameworkStores<AppDbContext>() //-- Serve para registrar e recuperar infos do user e dos perfis, que foram registered, no caso ser� recuperado do Entity
                .AddDefaultTokenProviders(); //-- Usado para gerar token nas opera��es de conta do user como redefini��o de senha, autentica��o de dois fatores


            //Registrar e habilitar a autentica��o.

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     //define oque deseja validar, e obtendo os valores do arquivo de configura��o

                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,

                         ValidIssuer = Configuration["Jwt:Issuer"], // quem est� emitindo o token
                         ValidAudience = Configuration["Jwt:Audience"], // destinatario do token
                         IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) // chave secreta para assinar o token
                     };
                 });


            services.AddControllers();


            //Para Autenticar Users pelo SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AlunosApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlunosApi v1"));
            }

            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:5000"); // -rota onde o React ira fazer as requisi��es
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseRouting();

            // Estas duas declara��es devem seguir estas ordens

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
