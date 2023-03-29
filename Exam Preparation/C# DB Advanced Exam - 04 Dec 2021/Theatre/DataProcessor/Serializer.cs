using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Utilities;
using Theatre.DataProcessor.ExportDto;

namespace Theatre.DataProcessor
{

    using System;
    using Theatre.Data;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context.Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(tr => tr.RowNumber >= 1 && tr.RowNumber <= 5)
                        .Sum(t => t.Price),
                    Tickets = t.Tickets.Select(tp => new
                    {
                        Price = tp.Price,
                        RowNumber = tp.RowNumber
                    }).Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5)
                      .OrderByDescending(ti => ti.Price)
                      .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(theaters, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ExportPlayDto[] playsDto = context.Plays
                .Include(a => a.Casts)
                .Where(p => p.Rating <= raiting)
                .ToArray()
                .Select(p => new ExportPlayDto()
                {
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(CultureInfo.InvariantCulture),
                    Genre = p.Genre.ToString(),
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Actors = p.Casts
                        .Where(c => c.IsMainCharacter)
                        .Select(c => new ExportActorDto()
                        {
                            FullName = c.FullName,
                            MainCharacter = $"Plays main character in '{p.Title}'."
                        })
                        .OrderByDescending(c => c.FullName)
                        .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            return xmlHelper.Serialize(playsDto, "Plays");
        }
    }
}
