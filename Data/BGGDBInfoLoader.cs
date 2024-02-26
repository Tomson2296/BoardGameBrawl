#nullable disable
using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;

namespace BoardGameBrawl.Data
{
    public static class BGGDBInfoLoader
    {
        static string RemoveQuotes(string value)
        {
            // This method removes quotes from the beginning and end of a value
            if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"')
            {
                return value.Substring(1, value.Length - 2);
            }
            return value;
        }

        public static async Task LoadFromDB(ApplicationDbContext context, IBoardGameStore<BoardgameModel> boardGameStore)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            context.Database.EnsureCreated();

            if (context.Boardgames.Any())
            {
                return;
            }
            else
            {
                string filePath = "C:\\Users\\Tomson\\source\\repos\\BoardGameBrawl\\Resources\\top2500_boardgames_data.csv";
                bool firstLine = true;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while(!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        if (firstLine)
                        {
                            firstLine = false;
                            continue;
                        }

                        bool variable1 = int.TryParse(values[0], out int BGGId);
                        string boardgameName = values[1];
                        bool variable2 = byte.TryParse(values[2], out byte minPlayers);
                        bool variable3 = byte.TryParse(values[3], out byte maxPlayers);
                        bool variable4 = int.TryParse(values[4], out int yearPublished);
                        bool variable5 = int.TryParse(values[5], out int playingtime);
                        string imageType = values[6];
                        string boardgame_category = values[7];
                        string boardgame_mechanics = values[8];

                        if (boardgameName.StartsWith('"'))
                        {
                            boardgameName = RemoveQuotes(boardgameName);
                        }

                        List<string> categories = boardgame_category.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
                        List<string> mechanics = boardgame_mechanics.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

                        // Create a new instance of Boardgame
                        BoardgameModel entry = new BoardgameModel
                        {
                           Id = Guid.NewGuid().ToString(),
                           Name = boardgameName,
                           BGGId = BGGId,
                           MinPlayers = minPlayers,
                           MaxPlayers = maxPlayers,
                           YearPublished = yearPublished,
                           PlayingTime = playingtime,
                           ImageFile = imageType,
                           Boardgame_Categories = categories,
                           Boardgame_Mechanics = mechanics
                        };
                        await boardGameStore.CreateAsync(entry);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}