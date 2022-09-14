namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            const string rootElement = "Projects";
            var serializer = new XmlSerializer(typeof(ProjectXmlImportModel[]), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var prodjectsDto = serializer.Deserialize(textReader) as ProjectXmlImportModel[];

            List<Project> validProjects = new List<Project>();
            StringBuilder sb = new StringBuilder();

            foreach (var projectDto in prodjectsDto)
            {
                DateTime currentProjectOpenDate;
                var isCurrentProjectOpenDateValid = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentProjectOpenDate);
                
                if (!IsValid(projectDto) || !isCurrentProjectOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? projectDuedate = null;
                if (!string.IsNullOrEmpty(projectDto.DueDate))
                {
                    DateTime currentProjectDueDate;
                    var isCurrentProjectDueDateValid = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentProjectDueDate);
                    if (!isCurrentProjectDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    projectDuedate = currentProjectDueDate;
                }

                var currentProject = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = currentProjectOpenDate,
                    DueDate = projectDuedate,
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    DateTime currentTaskOpenDate;
                    DateTime currentTaskDueDate;

                    var isCurrentTaskOpenDateValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentTaskOpenDate);
                    var isCurrentTaskDueDateValid = DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentTaskDueDate);
                    bool isCurrentTaskExecutionTypeValid = Enum.TryParse(taskDto.ExecutionType, out ExecutionType excurtionType);
                    bool isCurrentTaskLabelTypeValid = Enum.TryParse(taskDto.LabelType, out LabelType labelType);

                    if (!IsValid(taskDto) || 
                        !isCurrentTaskOpenDateValid || 
                        !isCurrentTaskDueDateValid || 
                        !isCurrentTaskExecutionTypeValid ||
                        !isCurrentTaskLabelTypeValid ||
                        currentTaskOpenDate < currentProjectOpenDate || 
                        (currentProject.DueDate.HasValue && currentTaskDueDate > currentProject.DueDate.Value))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    
                    currentProject.Tasks.Add(new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = currentTaskOpenDate,
                        DueDate = currentTaskDueDate,
                        ExecutionType = excurtionType,
                        LabelType = labelType,
                        ProjectId = currentProject.Id
                    });
                }
                validProjects.Add(currentProject);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, currentProject.Name, currentProject.Tasks.Count));
            }
            context.Projects.AddRange(validProjects);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDto = JsonConvert.DeserializeObject<IEnumerable<EmployeeJsonImportModel>>(jsonString);

            List<Employee> validEmployees = new List<Employee>();
            StringBuilder sb = new StringBuilder();

            foreach (var employeeDto in employeesDto)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentEmployee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                var currentEmployeeTasks = employeeDto.Tasks.Distinct().ToList();
                var databaseTasks = context.Tasks.ToList();
                foreach (var currentTask in currentEmployeeTasks)
                {
                    if (!databaseTasks.Any(x => x.Id == currentTask))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    currentEmployee.EmployeesTasks.Add(new EmployeeTask
                    {
                        TaskId = currentTask,
                        EmployeeId = currentEmployee.Id
                    });
                }
                validEmployees.Add(currentEmployee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, currentEmployee.Username, currentEmployee.EmployeesTasks.Count));
            }
            context.Employees.AddRange(validEmployees);
            context.SaveChanges();
            return sb.ToString().TrimEnd(); ;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}