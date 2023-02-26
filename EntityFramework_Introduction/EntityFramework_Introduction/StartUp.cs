using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();

            var output = GetDepartmentsWithMoreThan5Employees(dbContext);
            Console.WriteLine(output);

        }


        // Problem 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var selectedEmployees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    Salary = e.Salary.ToString("F2"),
                })
                .ToArray();

            foreach (var employee in selectedEmployees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary}");
            }

            return sb.ToString().TrimEnd();   
        }


        // Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var selectedEmployees = context.Employees
                .OrderBy(e => e.FirstName)
                .Where(em => em.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    Salary = e.Salary.ToString("F2")
                })
                .ToArray();

            foreach (var employee in selectedEmployees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary}");
            }

            return sb.ToString().TrimEnd();
        }


        // Problem 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder(); 
            var employeesResearchAndDev = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e=> e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    Salary = e.Salary.ToString("F2")
                })
                .ToArray();

            foreach (var employee in employeesResearchAndDev)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} from {employee.Department} - ${employee.Salary}");
            }

            return sb.ToString().TrimEnd();
        }


        // Problem 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var empl = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            var addressToAdd = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            empl.Address = addressToAdd;
            context.SaveChanges();

            var outputResultForEmployees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();

            return String.Join(Environment.NewLine, outputResultForEmployees);

        }


        // Problem 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var selectedEmployees = context.Employees
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagersFirstName = e.Manager!.FirstName,
                    ManagersLastName = e.Manager!.LastName,
                    EmployeeProjects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001
                                                && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate.HasValue 
                                            ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                            : "not finished"
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var employee in selectedEmployees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagersFirstName} {employee.ManagersLastName}");
                foreach (var project in employee.EmployeeProjects)
                {
                    sb.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        // Problem 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town!.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    Town = a.Town!.Name,
                    empCount = a.Employees.Count
                })
                .ToArray();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.Town} - {address.empCount} employees");
            }
            return sb.ToString().TrimEnd();
        }


        // Problem 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithTheirProjects = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.EmployeeId == e.EmployeeId)
                        .OrderBy(ep => ep.Project.Name)
                        .Select(ep => new
                        {
                            projectName = ep.Project.Name
                        }).ToArray()
                })
                .ToArray();

            foreach (var employee in employeesWithTheirProjects)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var project in employee.Projects)
                {
                    sb.AppendLine($"{project.projectName}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        // Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var resultingDepartments = context.Departments
                .Where(e => e.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(dep => new
                {
                    dep.Name,
                    ManFirstName = dep.Manager.FirstName,
                    ManLastName = dep.Manager.LastName,
                    ListEmployees = dep.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var dept in resultingDepartments)
            {
                sb.AppendLine($"{dept.Name} – {dept.ManFirstName} {dept.ManLastName}");
                foreach (var employee in dept.ListEmployees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }


    }
}