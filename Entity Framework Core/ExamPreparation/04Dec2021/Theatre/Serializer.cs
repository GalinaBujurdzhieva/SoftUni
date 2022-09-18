namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var topTheatres = context.Theatres
                .ToList()
                .Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count >= 20)
                .OrderByDescending(x => x.NumberOfHalls)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    Name = x.Name,
                    Halls = x.NumberOfHalls,
                    TotalIncome = x.Tickets.Where(x => x.RowNumber >= 1 && x.RowNumber <= 5).Sum(t => t.Price),
                    Tickets = x.Tickets
                         .Where(x => x.RowNumber >= 1 && x.RowNumber <= 5)
                         .Select(t => new
                         {
                             Price = t.Price,
                             RowNumber = t.RowNumber
                         })
                         .OrderByDescending(t => t.Price)
                         .ToList()
                })
                .ToList();
            return JsonConvert.SerializeObject(topTheatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var playsWithActors = context.Plays
                .ToArray()
                .Where(x => x.Rating <= rating)
                .Select(x => new PlayWithActorsXmlExportModel
                {
                    Title = x.Title,
                    Duration = x.Duration.ToString("c"),
                    Rating = x.Rating == 0 ? "Premier" : x.Rating.ToString(),
                    Genre = x.Genre.ToString(),
                    Actors = x.Casts
                    .Where(a => a.IsMainCharacter)
                    .Select(a => new ActorXmlExportModel
                    {
                        FullName = a.FullName,
                        MainCharacter = $"Plays main character in '{x.Title}'."
                    })
                    .OrderByDescending(a => a.FullName)
                    .ToArray()
                })
                .OrderBy(x => x.Title)
                .ThenByDescending(x => x.Genre)
                .ToArray();
            
            const string rootElement = "Plays";
            var serializer = new XmlSerializer(typeof(PlayWithActorsXmlExportModel[]), new XmlRootAttribute(rootElement));
            var textWriter = new StringWriter();
            using (textWriter)
            {
                serializer.Serialize(textWriter, playsWithActors, GetXmlNamespaces());
            }
            return textWriter.ToString();
        }
        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}
