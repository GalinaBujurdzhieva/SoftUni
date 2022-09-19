namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsDto = JsonConvert.DeserializeObject<IEnumerable<DepartmentJsonImportModel>>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Department> validDepartments = new List<Department>();

            foreach (var depatrmentDto in departmentsDto)
            {
                if (!IsValid(depatrmentDto) || depatrmentDto.Cells.Any(c => !IsValid(c)) || depatrmentDto.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var currentDepartment = new Department
                {
                    Name = depatrmentDto.Name,
                };

                foreach (var cellDto in depatrmentDto.Cells)
                {
                    currentDepartment.Cells.Add(new Cell
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    });
                }

                validDepartments.Add(currentDepartment);
                sb.AppendLine($"Imported {currentDepartment.Name} with {currentDepartment.Cells.Count} cells");
            }
            context.Departments.AddRange(validDepartments);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDto = JsonConvert.DeserializeObject<IEnumerable<PrisonerJsonImportModel>>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Prisoner> validPrisoners = new List<Prisoner>();

            foreach (var prisonerDto in prisonersDto)
            {
                bool isIncarcerationDateValid = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime incarcerationDate);

                if (!IsValid(prisonerDto) || !isIncarcerationDateValid || prisonerDto.Mails.Any(x => !IsValid(x)) || prisonerDto.Mails.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if (!String.IsNullOrEmpty(prisonerDto.ReleaseDate))
                {
                    bool isReleaseDateValid = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDateValue);
                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }
                    releaseDate = releaseDateValue;
                }

                var currentPrisoner = new Prisoner
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };
                foreach (var mailDto in prisonerDto.Mails)
                {
                    currentPrisoner.Mails.Add(new Mail
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    });
                }

                validPrisoners.Add(currentPrisoner);
                sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            const string rootElement = "Officers";
            var serializer = new XmlSerializer(typeof(List<OfficerXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var officresDto = serializer.Deserialize(textReader) as List<OfficerXmlImportModel>;

            StringBuilder sb = new StringBuilder();
            List<Officer> validOfficers = new List<Officer>();

            foreach (var officerDto in officresDto)
            {
                bool isCurrentOfficerPositionValid = Enum.TryParse(officerDto.Position, out Position currentOfficerPosition);
                bool isCurrentOfficerWeaponValid = Enum.TryParse(officerDto.Weapon, out Weapon currentOfficerWeapon);

                if (!IsValid(officerDto) || !isCurrentOfficerPositionValid || !isCurrentOfficerWeaponValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var currentOfficer = new Officer
                {
                    FullName = officerDto.Name,
                    Salary = officerDto.Money,
                    Position = currentOfficerPosition,
                    Weapon = currentOfficerWeapon,
                    DepartmentId = officerDto.DepartmentId
                };

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    currentOfficer.OfficerPrisoners.Add(new OfficerPrisoner
                    {
                        PrisonerId = prisonerDto.Id,
                        OfficerId = currentOfficer.Id
                    });
                }

                validOfficers.Add(currentOfficer);
                sb.AppendLine($"Imported {currentOfficer.FullName} ({currentOfficer.OfficerPrisoners.Count} prisoners)");
            }
            context.Officers.AddRange(validOfficers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}