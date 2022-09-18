namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
				var gamesByGenres = context.Genres
					.ToList()
					.Where(x => genreNames.Contains(x.Name))
					.Select(x => new
					{
						Id = x.Id,
						Genre = x.Name,
						Games = x.Games
						.Where(x => x.Purchases.Count > 0)
						.Select(g => new
						{
							Id = g.Id,
							Title = g.Name,
							Developer = g.Developer.Name,
							Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
							Players = g.Purchases.Count
						})
						.OrderByDescending(g => g.Players)
						.ThenBy(g => g.Id)
						.ToList(),
						TotalPlayers = x.Games.Sum(p => p.Purchases.Count)
					})
					.OrderByDescending(x => x.TotalPlayers)
					.ThenBy(x => x.Id)
					.ToList();
				return JsonConvert.SerializeObject(gamesByGenres, Formatting.Indented);	
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var userPurchasesByType = context.Users
				.ToArray()
				.Where(x => x.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == storeType)))
				.Select(x => new UserXmlExportModel
				{
					Username = x.Username,
					Purchases = x.Cards.SelectMany(c => c.Purchases)
				   .Where(c => c.Type.ToString() == storeType)
				   .OrderBy(c => c.Date)
				   .Select(c => new PurchaseXmlExportModel
				   {
					   Card = c.Card.Number,
					   Cvc = c.Card.Cvc,
					   Date = c.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
					   Game = new GameXmlExportModel
					   {
						   Name = c.Game.Name,
						   Genre = c.Game.Genre.Name,
						   Price = c.Game.Price
					   }
				   }).ToArray(),
					TotalSpent = x.Cards.Sum(ts => ts.Purchases.Where(p => p.Type.ToString() == storeType).Sum(p => p.Game.Price))
				})
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.Username)
				.ToArray();

			const string rootElement = "Users";
			var serializer = new XmlSerializer(typeof(UserXmlExportModel[]), new XmlRootAttribute(rootElement));
			var textWriter = new StringWriter();
			using (textWriter)
			{
				serializer.Serialize(textWriter, userPurchasesByType, GetXmlNamespaces());
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