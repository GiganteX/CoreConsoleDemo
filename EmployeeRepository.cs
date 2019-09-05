using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreConsoleDemo
{
	public interface IEmployeeRepository : IRepository<Employee, int>
	{
	}

	public class EmployeeRepository : ConfigurationBasedRepository<Employee, int>, IEmployeeRepository
	{
		public EmployeeRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
		{
		}
	}
}

