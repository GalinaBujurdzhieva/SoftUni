using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            //03. Console.WriteLine(GetEmployeesFullInformation(softUniContext));
            //04. Console.WriteLine(GetEmployeesWithSalaryOver50000(softUniContext));
            //05. Console.WriteLine(GetEmployeesFromResearchAndDevelopment(softUniContext));
            //06. Console.WriteLine(AddNewAddressToEmployee(softUniContext));
            //07. Console.WriteLine(GetEmployeesInPeriod(softUniContext));
            //08. Console.WriteLine(GetAddressesByTown(softUniContext));
            //09. Console.WriteLine(GetEmployee147(softUniContext));
            //10. Console.WriteLine(GetDepartmentsWithMoreThan5Employees(softUniContext));
            //11. Console.WriteLine(GetLatestProjects(softUniContext));
            //12. Console.WriteLine(IncreaseSalaries(softUniContext));
            //13. Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(softUniContext));
            //14. Console.WriteLine(DeleteProjectById(softUniContext));
            Console.WriteLine(RemoveTown(softUniContext));
        }
        //03.
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.
                Select(x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.EmployeeId)
                .ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        //04.
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .Select(x => new
                {
                    x.FirstName,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        //05.
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employeesFromRandD = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    DepartmentName = x.Department.Name,
                    x.Salary
                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employeesFromRandD)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }
        //06.
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(newAddress);
            context.SaveChanges();

            var employeeNakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            employeeNakov.AddressId = newAddress.AddressId;
            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(x => new
                {
                    x.Address.AddressText,
                    x.Address.AddressId
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.AddressText}");
            }
            return sb.ToString().TrimEnd();
        }
        //07.
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {

            var employees = context.Employees
                .Include(ep => ep.EmployeesProjects)
                .ThenInclude(p => p.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    employeeFirstName = x.FirstName,
                    employeeLastName = x.LastName,
                    managerFirstName = x.Manager.FirstName,
                    managerLastName = x.Manager.LastName,
                    employeeProjects = x.EmployeesProjects
                    .Select(y =>
                    new
                    {
                        projectName = y.Project.Name,
                        projectStartDate = y.Project.StartDate,
                        projectEndDate = y.Project.EndDate == null ? "not finished" : y.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                    })
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.employeeFirstName} {employee.employeeLastName} - Manager: {employee.managerFirstName} {employee.managerLastName}");
                foreach (var project in employee.employeeProjects)
                {
                    sb.AppendLine($"--{project.projectName} - {project.projectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {project.projectEndDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        //08.
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(x => x.Employees.Count)
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    townName = a.Town.Name,
                    employeeCount = a.Employees.Count
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.townName} - {address.employeeCount} employees");
            }
            return sb.ToString().TrimEnd();
        }
        //09.
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee147 = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(y => y.Project)
                .Where(emp => emp.EmployeeId == 147)
                .Select(z => new
                {
                    z.FirstName,
                    z.LastName,
                    z.JobTitle,
                    projects = z.EmployeesProjects
                    .OrderBy(v => v.Project.Name)
                    .Select(pr => new
                    {
                        pr.Project.Name
                    })
                })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
            foreach (var project in employee147.projects)
            {
                sb.AppendLine($"{project.Name}");
            }
            return sb.ToString().TrimEnd();
        }

        //10.
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var neededDepartments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(y => new
                {
                    departmentName = y.Name,
                    managerFirstName = y.Manager.FirstName,
                    managerLastName = y.Manager.LastName,
                    employees = y.Employees
                .Select(z => new
                {
                    employeeFirstName = z.FirstName,
                    employeeLastName = z.LastName,
                    employeeJobTitle = z.JobTitle,
                })
                    .OrderBy(z => z.employeeFirstName)
                    .ThenBy(z => z.employeeLastName)
                    .ToList()
                })
                .ToList();


            StringBuilder sb = new StringBuilder();
            foreach (var department in neededDepartments)
            {
                sb.AppendLine($"{department.departmentName} - {department.managerFirstName}  {department.managerLastName}");
                foreach (var employee in department.employees)
                {
                    sb.AppendLine($"{employee.employeeFirstName} {employee.employeeLastName} - {employee.employeeJobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        //11.
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .Select(x => new
                  {
                   x.Name,
                   x.Description,
                   x.StartDate
                  })
                .OrderBy(x => x.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }
            return sb.ToString().TrimEnd();
        }
        //12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] myDepartments = new string[]
            {
                "Engineering", "Tool Design", "Marketing", "Information Services"
            };

            var employees = context.Employees
                .Where(x => myDepartments.Contains(x.Department.Name))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }
            context.SaveChanges();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }
            return sb.ToString().TrimEnd();
        }
        //13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => EF.Functions.Like(x.FirstName, "Sa%"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }
            return sb.ToString().TrimEnd();
        }
        //14.
        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesToBeDeleted = context.EmployeesProjects
                .Where(x => x.ProjectId == 2);

            foreach (var employee in employeesToBeDeleted)
            {
                context.EmployeesProjects.Remove(employee);
            }
            context.SaveChanges();
            
            var departmentToBeDeleted = context.Projects
                .Where(x => x.ProjectId == 2)
                .FirstOrDefault();

            context.Projects.Remove(departmentToBeDeleted);
            context.SaveChanges();

            var neededProjects = context.Projects.Take(10).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var project in neededProjects)
            {
                sb.AppendLine(project.Name);
            }
            return sb.ToString().TrimEnd();
        }
        //15
        public static string RemoveTown(SoftUniContext context)
        {
            var townSeattle = context.Towns
                .Where(x => x.Name == "Seattle")
                .FirstOrDefault();

            var addressIdsOfSeattleTown = context.Addresses
                .Include(x => x.Town)
                .Where(x => x.Town.Name == "Seattle")
                .ToList();

            var employeesWhereAddressIdHasToBeNull = context.Employees
                .Include(x => x.Address)
                .Where(x => addressIdsOfSeattleTown.Contains(x.Address));

            foreach (var employee in employeesWhereAddressIdHasToBeNull)
            {
                employee.AddressId = null;
            }
            context.SaveChanges();

            context.Addresses.RemoveRange(addressIdsOfSeattleTown);
            context.SaveChanges();

            context.Towns.Remove(townSeattle);
            context.SaveChanges();

            return $"{addressIdsOfSeattleTown.Count} addresses in {townSeattle.Name} were deleted";
        }
    }
}
