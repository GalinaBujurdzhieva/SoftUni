namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projectsWithTasks = context.Projects
                .Where(x => x.Tasks.Any())
                .ToArray()
                .Select(x => new ProjectXmlExportModel
                {
                    TasksCount = x.Tasks.Count,
                    ProjectName = x.Name,
                    HasEndDate = x.DueDate == null ? "No" : "Yes",
                    Tasks = x.Tasks.Select(t => new ProjectTaskXmlExportModel
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.Tasks.Length)
                .ThenBy(x => x.ProjectName)
                .ToArray();

            const string rootElement = "Projects";
            var serializer = new XmlSerializer(typeof(ProjectXmlExportModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, projectsWithTasks, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var mostBusiestEmployees = context.Employees
                .Where(x => x.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .ToList()
                .Select(x => new
                {
                    Username = x.Username,
                    Tasks = x.EmployeesTasks
                            .Where(t => t.Task.OpenDate >= date)
                            .OrderByDescending(t => t.Task.DueDate)
                            .ThenBy(t => t.Task.Name)
                            .Select(t => new
                            {
                                TaskName = t.Task.Name,
                                OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                                DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                                LabelType = t.Task.LabelType.ToString(),
                                ExecutionType = t.Task.ExecutionType.ToString()
                            })
                            .ToList()
                })
                .OrderByDescending(x => x.Tasks.Count)
                .ThenBy(x => x.Username)
                .Take(10)
                .ToList();

            return JsonConvert.SerializeObject(mostBusiestEmployees, Formatting.Indented);
        }
        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}