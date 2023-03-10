using AutoMapper;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Concrete;
using Fitodu.Service.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fitodu.Service
{
    public static class AppServices
    {
        public static void RegisterDatabase(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddDbContext<Context>(x => x.UseSqlServer(config
                .GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies());

            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IDateTimeService, UtcDateTimeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, SendGridService>();
            //services.AddScoped<ITextMessageService, NexmoService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailMarketingService, ActiveCampaignService>();
            services.AddScoped<IBillingService, StripeService>();
            services.AddScoped<ISupportService, SupportService>();
            services.AddScoped<ICoachService, CoachService>();
            services.AddScoped<IPrivateNoteService, PrivateNoteService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPublicNoteService, PublicNoteService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IMaximumService, MaximumService>();
            services.AddScoped<ITrainingExerciseService, TrainingExerciseService>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ITrainingResultService, TrainingResultService>();
            services.AddScoped<IWeekPlanService, WeekPlanService>();
            services.AddScoped<IAwaitingTrainingService, AwaitingTrainingService>();
            services.AddScoped<ISummaryService, SummaryService>();
            services.AddScoped<IUserFeedbackService, UserFeedbackService>();
        }
    }
}