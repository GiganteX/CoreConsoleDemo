using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpRepository.Ioc.Microsoft.DependencyInjection;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;

namespace CoreConsoleDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("repository.json")
				.Build();

			var section = config.GetSection("sharpRepository");
			ISharpRepositoryConfiguration sharpConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(section);

			// setting up services
			var services = new ServiceCollection();

            // add dbcontext to container before configuration to sharprepository
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer("server=localhost;user id=sa;password=secret;database=employee"), ServiceLifetime.Transient);
                       
            services.UseSharpRepository(section);

            // add this lines for repo2
            services.AddSingleton(sharpConfig);
            services.AddTransient<EmployeeRepository>();

            var serviceProvider = services.BuildServiceProvider();

            // this need dbcontext from sharprepository DI
            var repo1 = new EmployeeRepository(sharpConfig);
            repo1.Add(new Employee { Name = "Sven Svensson" });

            // this needs EmployeeRepository and ISharpRepositoryConfiguration configured
            var repo2 = serviceProvider.GetService<EmployeeRepository>();
            repo2.Add(new Employee { Name = "Sven Svensson" });

            // this is not working yet, there some different how service provider is configured here ASP.NET Core
            var repo3 = serviceProvider.GetService<IRepository<Employee, int>>();
			repo3.Add(new Employee { Name = "Sven Svensson" });

		}
	}
}
