using Boardgames.DataProcessor.ExportDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Utilities;

namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ExportCreatorDto[] creatorDtos = context.Creators
                .Include(b => b.Boardgames)
                .ToArray()
                .Where(c => c.Boardgames.Count >= 1)
                .Select(c => new ExportCreatorDto()
                {
                    BoardGamesCount = c.Boardgames.Count,
                    CreatorFullName = $"{c.FirstName} {c.LastName}",
                    BoardGames = c.Boardgames.Where(c => c.Creator.Boardgames.Count >= 1)
                        .Select(c => new ExportBoardgameDto()
                        {
                            Name = c.Name,
                            YearPublished = c.YearPublished
                        }).OrderBy(b => b.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardGames.Length)
                .ThenBy(c => c.CreatorFullName)
                .ToArray();

            return xmlHelper.Serialize(creatorDtos, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Include(bs => bs.BoardgamesSellers)
                .ThenInclude(b => b.Boardgame)
                .ToArray()
                .Where(s => s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year
                                                         && b.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Where(b => b.Boardgame.YearPublished >= year
                                    && b.Boardgame.Rating <= rating)
                        .Select(bg => new
                        {
                            Name = bg.Boardgame.Name,
                            Rating = bg.Boardgame.Rating,
                            Mechanics = bg.Boardgame.Mechanics,
                            Category = bg.Boardgame.CategoryType.ToString()
                        }).OrderByDescending(g => g.Rating)
                        .ThenBy(b => b.Name)
                        .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);

        }
    }
}