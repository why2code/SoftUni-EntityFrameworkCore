using System.Globalization;
using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ImportDto;
using Footballers.Utilities;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        private static XmlHelper XmlHelper;

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            ImportCoachDto[] coachesDtos = xmlHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoaches = new HashSet<Coach>();

            foreach (ImportCoachDto coachDto in coachesDtos)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }



                //Validation for each footballer
                ICollection<Footballer> validFootballers = new HashSet<Footballer>();
                foreach (ImportFootballerDto footballer in coachDto.Footballers)
                {
                    if (!IsValid(footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime startD = DateTime.ParseExact(footballer.ContractStartDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                    DateTime EndD = DateTime.ParseExact(footballer.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);

                    //The test in Expected results does not seem to have check for Date!
                    //Ref. Coach Gerardo Seoane should be imported with 1 player, not 2 (in expected)
                    if (EndD < startD)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer currentFootballer = new Footballer()
                    {
                        Name = footballer.Name,
                        ContractStartDate = startD,
                        ContractEndDate = EndD,
                        BestSkillType = (BestSkillType)footballer.BestSkillType,
                        PositionType = (PositionType)footballer.PositionType
                    };

                    validFootballers.Add(currentFootballer);
                }

                Coach currentValidCoach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                    Footballers = validFootballers
                };


                validCoaches.Add(currentValidCoach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, currentValidCoach.Name,
                    validFootballers.Count));
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Team> validTeamsOutput = new HashSet<Team>();

            ImportTeamsDto[] teamsDtos = JsonConvert.DeserializeObject<ImportTeamsDto[]>(jsonString);

            foreach (ImportTeamsDto team in teamsDtos)
            {
                if (!IsValid(team))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (team.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team currValidTeam = new Team()
                {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    Trophies = team.Trophies
                };


                //Moving into validating the Footballer IDs provided with the json import:
                ICollection<Footballer> validFootballers = new HashSet<Footballer>();
                ICollection<int> dbFootballers = context.Footballers
                    .AsNoTracking()
                    .Select(f => f.Id)
                    .ToArray();
                

                foreach (var footballerId in team.Footballers.Distinct())
                {
                    if (!dbFootballers.Contains(footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer tmf = new TeamFootballer()
                    {
                        Team = currValidTeam,
                        FootballerId = footballerId
                    };

                    currValidTeam.TeamsFootballers.Add(tmf);
                }

                validTeamsOutput.Add(currValidTeam);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, currValidTeam.Name,
                    currValidTeam.TeamsFootballers.Count));
            }

            context.Teams.AddRange(validTeamsOutput);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
