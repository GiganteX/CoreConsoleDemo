using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

			// how to setup MyDbContext with the UseSqlServer() line?

			var repo = new EmployeeRepository(sharpConfig);

			var emp = new Employee { Name = "Sven Svensson" };

			repo.Add(emp);
		}
	}
}
