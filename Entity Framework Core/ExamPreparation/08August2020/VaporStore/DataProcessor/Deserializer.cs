namespace VaporStore.DataProcessor
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
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var gamesDto = JsonConvert.DeserializeObject<IEnumerable<GameJsonImportModel>>(jsonString);
			StringBuilder sb = new StringBuilder();

            foreach (var gameDto in gamesDto)
            {
				DateTime currentDtoReleaseDate;
				bool isCurrentDtoReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentDtoReleaseDate);

				if (!IsValid(gameDto) || !isCurrentDtoReleaseDateValid)
                {
					sb.AppendLine("Invalid Data");
					continue;
                }
				var currentGameDtoDeveloper = context.Developers.FirstOrDefault(x => x.Name == gameDto.Developer) ?? new Developer { Name = gameDto.Developer };
				var currentGameDtoGenre = context.Genres.FirstOrDefault(x => x.Name == gameDto.Genre) ?? new Genre { Name = gameDto.Genre };

				var currentGame = new Game
				{
					Name = gameDto.Name,
					Price = gameDto.Price,
					ReleaseDate = currentDtoReleaseDate,
					Developer = currentGameDtoDeveloper,
					Genre = currentGameDtoGenre
				};
                foreach (var tag in gameDto.Tags)
                {
					var currentGameDtoTag = context.Tags.FirstOrDefault(x => x.Name == tag) ?? new Tag { Name = tag };
					currentGame.GameTags.Add(new GameTag { Game = currentGame, Tag = currentGameDtoTag });
                }
				context.Games.Add(currentGame);
				context.SaveChanges();
				sb.AppendLine($"Added {currentGame.Name} ({currentGame.Genre.Name}) with {currentGame.GameTags.Count} tags");
            }

			return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var usersDto = JsonConvert.DeserializeObject<IEnumerable<UserJsonImportModel>>(jsonString);
			StringBuilder sb = new StringBuilder();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto) || !(userDto.Cards.All(IsValid)))
                {
					sb.AppendLine("Invalid Data");
					continue;
				}

				var currentUser = new User
				{
					Username = userDto.Username,
					FullName = userDto.FullName,
					Email = userDto.Email,
					Age = userDto.Age,
					Cards = userDto.Cards.Select(x => new Card
                    {
						Number = x.Number,
						Cvc = x.Cvc,
						Type = x.Type.Value
                    }).ToList()
				};
				context.Users.Add(currentUser);
				context.SaveChanges();
				sb.AppendLine($"Imported {currentUser.Username} with {currentUser.Cards.Count} cards");
            }
			return sb.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			string rootElement = "Purchases";
			var serializer = new XmlSerializer(typeof(List<PurchaseXmlImportModel>), new XmlRootAttribute(rootElement));
			var textReader = new StringReader(xmlString);
			var purchasesDto = serializer.Deserialize(textReader) as List<PurchaseXmlImportModel>;
			var sb = new StringBuilder();

            foreach (var purchaseDto in purchasesDto)
            {
				DateTime currentPurchaseDate;
				var isPurchaseDtoDateValid = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentPurchaseDate);

                if (!IsValid(purchaseDto) || !isPurchaseDtoDateValid)
                {
					sb.AppendLine("Invalid Data");
					continue;
				}
				var currentPurchase = new Purchase
				{
					Type = purchaseDto.Type.Value,
					ProductKey = purchaseDto.Key,
					Date = currentPurchaseDate,
					Card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card),
					Game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Game)
				};
				context.Purchases.Add(currentPurchase);
				context.SaveChanges();
				var username = context.Users.FirstOrDefault(x => x.Cards.Any(c => c.Number == purchaseDto.Card));
				sb.AppendLine($"Imported {currentPurchase.Game.Name} for {username.Username}"!);
			}	
				
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