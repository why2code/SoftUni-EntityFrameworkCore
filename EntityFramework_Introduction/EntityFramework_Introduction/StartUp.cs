using System.Text;
using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();

            var output = GetEmployeesFromResearchAndDevelopment(dbContext);
            Console.WriteLine(output);

        }

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
        
    }
}