﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.TestApp
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutoMapperModule)
        )]
    public class TestAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureAutoMapper(context.Services);

            context.Services.AddAssemblyOf<TestAppModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add(ctx =>
                {
                    ctx.MapperConfiguration.CreateMap<Person, PersonDto>().ReverseMap();
                    ctx.MapperConfiguration.CreateMap<Phone, PhoneDto>().ReverseMap();
                });
            });
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<TestDataBuilder>()
                    .Build();
            }
        }
    }
}
