// ReSharper disable InconsistentNaming

using ProductShop.Utilities;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;
using Task = TeisterMask.Data.Models.Task;

namespace TeisterMask.DataProcessor
{
    using Data;
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static XmlHelper XmlHelper;
        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportProjectDto[] projects = xmlHelper.Deserialize<ImportProjectDto[]>(xmlString, "Projects");

            ICollection<Project> validProjects = new HashSet<Project>();
            StringBuilder sb = new StringBuilder();

            foreach (ImportProjectDto projectDto in projects)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isProjectOpenDateAvailable = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectOpeningDate);

                bool isProjectDueDateAvailable = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectDueDate);

                if (!isProjectOpenDateAvailable)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Project project = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = projectOpeningDate
                };

                if (!string.IsNullOrEmpty(projectDto.DueDate))
                {
                    project.DueDate = projectDueDate;
                }

                //Validation for the Task array
                List<Task> validTasks = new List<Task>();
                foreach (ImportTaskDto taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTaskOpenDateAvailable = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate);
                    bool isTaskDueDateAvailable = DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate);

                    if (!isTaskOpenDateAvailable
                        || !isTaskDueDateAvailable)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < projectOpeningDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(projectDto.DueDate) 
                        && taskDueDate > projectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    
                    Task task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType,
                        Project = project
                    };

                    validTasks.Add(task);
                }

                project.Tasks = validTasks;
                validProjects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(validProjects);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();
            ImportEmployeeDto[] employeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);
            ICollection<Employee> employees = new HashSet<Employee>();

            foreach (ImportEmployeeDto employeeDto in employeeDtos)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Employee employee = new Employee()
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                //Verification for available tasks in DB
                ICollection<int> taskIds = context.Tasks
                    .AsNoTracking()
                    .Select(t => t.Id)
                    .ToArray();

                //Checking each distinct taskId for the individual Dtos
                ICollection<EmployeeTask> validTasks = new HashSet<EmployeeTask>();
                foreach (int taskId in employeeDto.Tasks.Distinct())
                {
                    if (!taskIds.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask empTask = new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = taskId
                    };
                    validTasks.Add(empTask);
                }

                employee.EmployeesTasks = validTasks;
                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username,
                    employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}