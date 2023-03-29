

namespace Theatre.DataProcessor

{
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
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


        public static XmlHelper XmlHelper;
        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportPlaysDto[] playsDtos = xmlHelper.Deserialize<ImportPlaysDto[]>(xmlString, "Plays");

            StringBuilder sb = new StringBuilder();
            ICollection<Play> validPlays = new HashSet<Play>();

            foreach (ImportPlaysDto playsDto in playsDtos)
            {
                if (!IsValid(playsDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Validate if the Duration is maximum of 1 hour or not, and also format that into the correct format
                bool isDurationValid = TimeSpan.TryParseExact(playsDto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan duration);
                bool isGenreValid = Enum.TryParse<Genre>(playsDto.Genre, out Genre genre);

                if (!isDurationValid
                    || !isGenreValid
                    || duration < TimeSpan.FromHours(1))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play currentValidPlay = new Play()
                {
                    Title = playsDto.Title,
                    Duration = duration,
                    Rating = playsDto.Rating,
                    Genre = genre,
                    Description = playsDto.Description,
                    Screenwriter = playsDto.Screenwriter
                };


                validPlays.Add(currentValidPlay);
                sb.AppendLine(string.Format(SuccessfulImportPlay, currentValidPlay.Title, currentValidPlay.Genre,
                    currentValidPlay.Rating));
            }

            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            var sb = new StringBuilder();

            ImportCastDto[] castDtos = xmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");
            ICollection<Cast> validCasts = new HashSet<Cast>();

            //ICollection<int> dbValidPlayIds = context.Plays
            //    .AsNoTracking()
            //    .Select(p => p.Id)
            //    .ToArray();

            foreach (ImportCastDto castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Not certain if this check is needed... assignment not specifying that
                //if (!dbValidPlayIds.Contains(castDto.PlayId))
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                Cast validCast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                string mainOrLesserChar = validCast.IsMainCharacter ? "main" : "lesser";

                validCasts.Add(validCast);
                sb.AppendLine(string.Format(SuccessfulImportActor, validCast.FullName, mainOrLesserChar));
            }

            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportTheatreDto[] theatreDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);
            ICollection<Data.Models.Theatre> outputTheatres = new HashSet<Data.Models.Theatre>();


            foreach (ImportTheatreDto theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Validating the individual theaters now:
                ICollection<Ticket> validTickets = new HashSet<Ticket>();
                foreach (ImportTicketDto ticket in theatreDto.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket validTicket = new Ticket()
                    {
                        Price = ticket.Price,
                        RowNumber = ticket.RowNumber,
                        PlayId = ticket.PlayId

                    };

                    validTickets.Add(validTicket);

                }

                Theatre validTheatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director,
                    Tickets = validTickets
                };

                outputTheatres.Add(validTheatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, validTheatre.Name, validTheatre.Tickets.Count));

            }

            context.Theatres.AddRange(outputTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        //public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        //{
        //    ImportTheatreDto[] theaterDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

        //    StringBuilder sb = new StringBuilder();
        //    List<Theatre> theatres = new List<Theatre>();

        //    foreach (var theatreDto in theaterDtos)
        //    {
        //        if (!IsValid(theatreDto))
        //        {
        //            sb.AppendLine(ErrorMessage);
        //            continue;
        //        }

        //        List<Ticket> tickets = new List<Ticket>();
        //        foreach (var ticketDto in theatreDto.Tickets)
        //        {
        //            if (!IsValid(ticketDto))
        //            {
        //                sb.AppendLine(ErrorMessage);
        //                continue;
        //            }

        //            Ticket ticket = new Ticket()
        //            {
        //                PlayId = ticketDto.PlayId,
        //                Price = ticketDto.Price,
        //                RowNumber = ticketDto.RowNumber
        //            };
        //            tickets.Add(ticket);
        //        }

        //        Theatre theatre = new Theatre()
        //        {
        //            Director = theatreDto.Director,
        //            Name = theatreDto.Name,
        //            NumberOfHalls = theatreDto.NumberOfHalls,
        //            Tickets = tickets
        //        };
        //        theatres.Add(theatre);
        //        sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
        //    }
        //    context.Theatres.AddRange(theatres);
        //    context.SaveChanges();
        //    return sb.ToString().TrimEnd();
        //}

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
