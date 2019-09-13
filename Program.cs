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
			services.UseSharpRepository(section);
			services.AddDbContext<MyDbContext>(options => options.UseSqlServer("server=localhost;user id=sa;password=secret;database=employee"), ServiceLifetime.Transient);
			var serviceProvider = services.BuildServiceProvider();

			// this is how I want it to work, using a ISharpRepositoryConfiguration object
			var repo1 = new EmployeeRepository(sharpConfig); 
			repo1.Add(new Employee { Name = "Sven Svensson" });

			// this would be second best, but won't work either...
			var repo2 = serviceProvider.GetService<EmployeeRepository>();
			repo2.Add(new Employee { Name = "Sven Svensson" });

			// nor will this...
			var repo3 = serviceProvider.GetService<IRepository<Employee, int>>();
			repo3.Add(new Employee { Name = "Sven Svensson" });

		}
	}
}
