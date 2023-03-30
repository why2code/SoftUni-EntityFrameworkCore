using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Utilities;
using TeisterMask.DataProcessor.ExportDto;

namespace TeisterMask.DataProcessor
{
    using Data;
    using Microsoft.VisualBasic;
    using TeisterMask.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var projects = context.Projects
                .Where(p => p.Tasks.Count >= 1)
                .Select(p => new ExportProjectDto()
                {
                    TaskCount = p.Tasks.Count,
                    Name = p.Name,
                    HasDate = string.IsNullOrEmpty(p.DueDate.ToString()) ? "No" : "Yes",
                    Tasks = p.Tasks.Select(p => new ExportTaskDto()
                        {
                            Name = p.Name,
                            LabelType = p.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                })
                .OrderByDescending(p => p.Tasks.Length)
                .ThenBy(p => p.Name)
                .ToArray();

            return xmlHelper.Serialize(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var sb = new StringBuilder();
            var employees = context.Employees
                .Include(et => et.EmployeesTasks)
                .ThenInclude(t => t.Task)
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(t => t.Task.OpenDate >= date)
                        .OrderByDescending(t => t.Task.DueDate)
                        .ThenBy(t => t.Task.Name)
                        .Select(t => new
                        {
                            TaskName = t.Task.Name,
                            OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = t.Task.LabelType.ToString(),
                            ExecutionType = t.Task.ExecutionType.ToString(),
                        }).ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }
    }
}