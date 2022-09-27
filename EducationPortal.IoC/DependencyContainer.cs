using EducationPortal.Application.Interfaces;
using EducationPortal.Application.Services;
using EducationPortal.Data.Context;
using EducationPortal.Data.Repositories;
using EducationPortal.Domain.Interfaces;
using EducationPortal.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EducationPortal.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //repositories
            services.AddScoped<IRepository<Course>, CourseRepository>();
            services.AddScoped<IRepository<Skill>, SkillRepository>();
            services.AddScoped<IRepository<Material>, MaterialRepository>();
            services.AddScoped<IRepository<CourseState>, CourseStateRepository>();
            services.AddScoped<IRepository<MaterialState>, MaterialStateRepository>();

            //managers
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IUserService, UserService>();

            //services.AddScoped<ISignService, SignService>();
        }
    }
}
