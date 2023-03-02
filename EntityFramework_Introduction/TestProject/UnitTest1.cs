using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using SoftUni;
using SoftUni.Data;
using SoftUni.Models;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

[TestFixture]
public class Test_004_001_001
{
    [Test]
    public void ValidateOutput()
    {
        var services = new ServiceCollection()
            .AddDbContext<SoftUniContext>(b => b.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        var serviceProvider = services.BuildServiceProvider();

        var context = serviceProvider.GetService<SoftUniContext>();

        this.Seed(context);

        string result = StartUp.GetEmployeesWithSalaryOver50000(context).Trim();

        string expected = AssertMethod(context).Trim();

        Assert.AreEqual(expected, result, "Returned value is incorrect!");
    }

    private void Seed(SoftUniContext context)
    {
        ICollection<Employee> employees = new List<Employee>();

        var firstNames = new[]
        {
            "Atanas", "Mariela", "Ani", "Slavi", "Pesho"
        };

        var lastNames = new[]
        {
            "Atanasow", "Cureva", "Topova", "Ludakov", "Peshev"
        };

        var middleNames = new[]
        {
            "Petrov", "Kirov", "Stoyanov", "Angelov", "Vladimirov"
        };

        var jobTitles = new[]
        {
            "Bartender", "Waiter", "Artist", "Shefa", "Driver"
        };

        var salaries = new[]
        {
            642.12m, 5235, 4000.32m, 25432, 2535235.33m
        };

        for (int i = 0; i < 5; i++)
        {
            var employee = new Employee
            {
                FirstName = firstNames[i],
                LastName = lastNames[i],
                MiddleName = middleNames[i],
                JobTitle = jobTitles[i],
                Salary = salaries[i],
            };

            employees.Add(employee);
        }

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }

    public static string AssertMethod(SoftUniContext context)
    {
        StringBuilder content = new StringBuilder();

        var employees = context.Employees
            .Where(x => x.Salary > 50000)
            .OrderBy(x => x.FirstName)
            .ToList();

        foreach (var employee in employees)
        {
            content.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
        }

        return content.ToString().TrimEnd();
    }
}