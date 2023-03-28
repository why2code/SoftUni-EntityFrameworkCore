using System.Globalization;
using Footballers.DataProcessor.ExportDto;
using Footballers.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            var coachesDb = context.Coaches
                .Where(c => c.Footballers.Count >= 1)
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers.Select(f => new ExportFootballerDto()
                        {
                            Name = f.Name,
                            PositionType = f.PositionType.ToString()
                        })
                        .OrderBy(f => f.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.Footballers.Length)
                .ThenBy(c => c.CoachName)
                .ToArray();

            return xmlHelper.Serialize(coachesDb, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Include(tf => tf.TeamsFootballers)
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                                  .Where(tf => tf.Footballer.ContractStartDate >= date)
                                  .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                                  .ThenBy(tf => tf.Footballer.Name)
                                  .ToArray()
                                  .Select(tfb => new
                                        {
                                            FootballerName = tfb.Footballer.Name,
                                            ContractStartDate = tfb.Footballer.ContractStartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                                            ContractEndDate = tfb.Footballer.ContractEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                                            BestSkillType = tfb.Footballer.BestSkillType.ToString(),
                                            PositionType = tfb.Footballer.PositionType.ToString()
                                        }).ToArray()
                })
                .OrderByDescending(m => m.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
