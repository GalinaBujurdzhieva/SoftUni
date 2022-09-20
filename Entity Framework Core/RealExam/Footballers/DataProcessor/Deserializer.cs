namespace Footballers.DataProcessor
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
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            const string rootElement = "Coaches";
            var serializer = new XmlSerializer(typeof(List<FootballerXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var coachesDtos = serializer.Deserialize(textReader) as List<FootballerXmlImportModel>;

            var validCoaches = new List<Coach>();
            StringBuilder sb = new StringBuilder();

            foreach (var coachDto in coachesDtos)
            {
                if (!IsValid(coachDto) || string.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime invalidNationality;
                if (DateTime.TryParse(coachDto.Nationality, out invalidNationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentCoach = new Coach
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };
                foreach (var footballerDto in coachDto.Footballers)
                {
                    DateTime currentFootballerContractStartDate;
                    var isCurrentFootballerContractStartDateValid = DateTime.TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentFootballerContractStartDate);

                    DateTime currentFootballerContractEndDate;
                    var isCurrentFootballerContractEndDateValid = DateTime.TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentFootballerContractEndDate);

                    bool isCurrentDTOBestSkillTypeValid = Enum.TryParse(footballerDto.BestSkillType, out BestSkillType bestSkillType);
                    bool isCurrentDTOPositionTypeValid = Enum.TryParse(footballerDto.PositionType, out PositionType positionType);

                    if (!IsValid(footballerDto)
                        || !isCurrentFootballerContractStartDateValid
                        || !isCurrentFootballerContractEndDateValid
                        || !isCurrentDTOBestSkillTypeValid
                        || !isCurrentDTOPositionTypeValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (currentFootballerContractStartDate > currentFootballerContractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    currentCoach.Footballers.Add(new Footballer
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = currentFootballerContractStartDate,
                        ContractEndDate = currentFootballerContractEndDate,
                        PositionType = positionType,
                        BestSkillType = bestSkillType
                    });
                }
                validCoaches.Add(currentCoach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, currentCoach.Name, currentCoach.Footballers.Count));

            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamsDtos = JsonConvert.DeserializeObject<IEnumerable<TeamJsonImportModel>>(jsonString);
            
            var validTeams = new List<Team>();
            StringBuilder sb = new StringBuilder();

            foreach (var teamDto in teamsDtos)
            {
                if (!IsValid(teamDto) || teamDto.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentTeam = new Team
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies
                };

                var footballersIdsInDatabase = context.Footballers.Select(x => x.Id).ToList();
                
                foreach (var uniqueFootballerId in teamDto.Footballers.Distinct())
                {
                    
                    if (!footballersIdsInDatabase.Contains(uniqueFootballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    currentTeam.TeamsFootballers.Add(new TeamFootballer
                    {
                        Team = currentTeam,
                        FootballerId = uniqueFootballerId
                    });
                }
                validTeams.Add(currentTeam);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, currentTeam.Name, currentTeam.TeamsFootballers.Count));
            }
            context.AddRange(validTeams);
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
