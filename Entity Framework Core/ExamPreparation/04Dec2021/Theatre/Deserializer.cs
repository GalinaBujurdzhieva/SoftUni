namespace Theatre.DataProcessor
{
    using AutoMapper;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";
        static IMapper mapper;

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            const string rootElement = "Plays";
            var serializer = new XmlSerializer(typeof(List<PlayXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var playsDto = serializer.Deserialize(textReader) as List<PlayXmlImportModel>;

            List<Play> validPlays = new List<Play>();
            StringBuilder sb = new StringBuilder();
            InitializeMapper();

            foreach (var playDto in playsDto)
            {
                bool isCurrentDTOGenreValid = Enum.TryParse(playDto.Genre, out Genre genre);
                bool isCurrentDTODurationValid = TimeSpan.TryParse(playDto.Duration, CultureInfo.InvariantCulture, out TimeSpan duration);
                if (!IsValid(playDto) || !isCurrentDTOGenreValid || !isCurrentDTODurationValid || duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentPlay = mapper.Map<Play>(playDto);
                validPlays.Add(currentPlay);
                sb.AppendLine(string.Format(SuccessfulImportPlay, currentPlay.Title, currentPlay.Genre.ToString(), currentPlay.Rating));
            }
            context.Plays.AddRange(validPlays);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            const string rootElement = "Casts";
            var serializer = new XmlSerializer(typeof(List<CastXmlImportModel>), new XmlRootAttribute(rootElement));
            var textReader = new StringReader(xmlString);
            var castsDto = serializer.Deserialize(textReader) as List<CastXmlImportModel>;

            List<Cast> validCasts = new List<Cast>();
            StringBuilder sb = new StringBuilder();
            InitializeMapper();

            foreach (var castDto in castsDto)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentCast = mapper.Map<Cast>(castDto);
                validCasts.Add(currentCast);
                string mainCharacterOrNot = currentCast.IsMainCharacter ? "main" : "lesser";
                sb.AppendLine(string.Format(SuccessfulImportActor, currentCast.FullName, mainCharacterOrNot));
            }
            context.Casts.AddRange(validCasts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theatresWithTicketsDto = JsonConvert.DeserializeObject<IEnumerable<TheatreJsonImportModel>>(jsonString);

            List<Theatre> validTheatres = new List<Theatre>();
            StringBuilder sb = new StringBuilder();
            foreach (var theatre in theatresWithTicketsDto)
            {
                if (!IsValid(theatre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var validTheatre = new Theatre
                {
                    Name = theatre.Name,
                    NumberOfHalls = theatre.NumberOfHalls,
                    Director = theatre.Director,
                    Tickets = new List<Ticket>()
                };
                foreach (var ticket in theatre.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var validTicket = new Ticket
                    {
                        Price = ticket.Price,
                        RowNumber = ticket.RowNumber,
                        PlayId = ticket.PlayId
                    };
                    validTheatre.Tickets.Add(validTicket);
                }
                validTheatres.Add(validTheatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, validTheatre.Name, validTheatre.Tickets.Count));
            }
            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
        private static void InitializeMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<TheatreProfile>();
            });
            mapper = configuration.CreateMapper();
        }
    }
}
