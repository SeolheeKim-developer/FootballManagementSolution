using FootballManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Numerics;

namespace FootballManagement.Data
{
    public static class FMInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            FootballManagementContext context = applicationBuilder.ApplicationServices.CreateScope()
               .ServiceProvider.GetRequiredService<FootballManagementContext>();

            try
            {
                //Delete the database if you need to apply a new Migration
                context.Database.EnsureDeleted();
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();

                //To randomly generate data
                Random random = new(50);

                // Look for any League. 
                if (!context.Leagues.Any())
                {
                    context.Leagues.AddRange(
                     new League
                     {
                         Code = "CF",
                         Name = "Canadian Football League"
                     },

                     new League
                     {
                         Code = "ML",
                         Name = "Major League Soccer"
                     },
                     new League
                     {
                         Code = "LO",
                         Name = "League1 Ontario"
                     });
                    context.SaveChanges();
                }
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                    new Team
                    {
                        Name = "Calgary Stampeders",
                        Budget = 600.0,
                        LeagueCode = context.Leagues.FirstOrDefault(d => d.Name == "Canadian Football League").Code
                    },
                    new Team
                    {
                        Name = "BC Lions",
                        Budget = 700.0,
                        LeagueCode = context.Leagues.FirstOrDefault(d => d.Name == "Major League Soccer").Code
                    },
                    new Team
                    {
                        Name = "Edmonton Football Team",
                        Budget = 800.0,
                        LeagueCode = context.Leagues.FirstOrDefault(d => d.Name == "Major League Soccer").Code
                    },
                    new Team
                    {
                        Name = "Hamilton Tiger-Cats",
                        Budget = 900.0,
                        LeagueCode = context.Leagues.FirstOrDefault(d => d.Name == "Major League Soccer").Code
                    },
                    new Team
                    {
                        Name = "Montreal Alouettes",
                        Budget = 650.0,
                        LeagueCode = context.Leagues.FirstOrDefault(d => d.Name == "League1 Ontario").Code
                    },
                    new Team
                    {
                        Name = "Toronto Argonauts",
                        Budget = 850.0,
                        LeagueCode = context.Leagues.FirstOrDefault(d => d.Name == "League1 Ontario").Code
                    });
                    context.SaveChanges();
                }
                if (!context.Players.Any())
                {
                    context.Players.AddRange(
                    new Player
                    {
                        FirstName = "Andrew Harris",
                        LastName = "Harris",
                        Jersey = "10",
                        DOB = DateTime.Parse("1993-09-01"),
                        FeePaid = 130,
                        EMail = "fflintstone@outlook.com"
                    },
                    new Player
                    {
                        FirstName = "Brandon",
                        LastName = "Banks",
                        Jersey = "12",
                        DOB = DateTime.Parse("1994-03-11"),
                        FeePaid = 125,
                        EMail = "Brandon@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Levi",
                        LastName = "Mitchell",
                        Jersey = "13",
                        DOB = DateTime.Parse("1995-11-24"),
                        FeePaid = 140,
                        EMail = "Levi@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Trey",
                        LastName = "Lewis",
                        Jersey = "15",
                        DOB = DateTime.Parse("1996-01-20"),
                        FeePaid = 135,
                        EMail = "Trey@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Nik",
                        LastName = "Lewis",
                        Jersey = "20",
                        DOB = DateTime.Parse("1992-04-04"),
                        FeePaid = 125,
                        EMail = "Harris@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Jevon",
                        LastName = "Holland",
                        Jersey = "21",
                        DOB = DateTime.Parse("1991-08-11"),
                        FeePaid = 129,
                        EMail = "Jevon@gmail.com"
                    },
                    new Player
                    {
                        FirstName = "Tevaughn",
                        LastName = "Campbell",
                        Jersey = "25",
                        DOB = DateTime.Parse("1995-12-09"),
                        FeePaid = 140,
                        EMail = "Tevaughn@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Mike",
                        LastName = "Reilly",
                        Jersey = "26",
                        DOB = DateTime.Parse("1996-09-16"),
                        FeePaid = 145,
                        EMail = "Reilly@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Brad",
                        LastName = "Sinopoli",
                        Jersey = "28",
                        DOB = DateTime.Parse("1999-10-02"),
                        FeePaid = 130,
                        EMail = "Sinopoli@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Tyrone",
                        LastName = "Pierre",
                        Jersey = "29",
                        DOB = DateTime.Parse("1997-03-01"),
                        FeePaid = 145,
                        EMail = "Tyrone@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Llevi",
                        LastName = "Noel",
                        Jersey = "31",
                        DOB = DateTime.Parse("1998-09-06"),
                        FeePaid = 125,
                        EMail = "Llevi@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Simoni",
                        LastName = "Lawrence",
                        Jersey = "33",
                        DOB = DateTime.Parse("1992-11-11"),
                        FeePaid = 130,
                        EMail = "Simoni@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Davaris",
                        LastName = "Daniels",
                        Jersey = "35",
                        DOB = DateTime.Parse("1999-05-05"),
                        FeePaid = 140,
                        EMail = "Davaris@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Juwan",
                        LastName = "Brescacin",
                        Jersey = "36",
                        DOB = DateTime.Parse("1997-06-06"),
                        FeePaid = 125,
                        EMail = "Juwan@gamil.com"
                    },
                    new Player
                    {
                        FirstName = "Shaquille",
                        LastName = "Richardson",
                        Jersey = "38",
                        DOB = DateTime.Parse("1998-08-08"),
                        FeePaid = 125,
                        EMail = "Shaquille@gamil.com"
                    });
                    context.SaveChanges();
                }
                //Create collections of the primary keys of the Players and Teams
                int[] playerIDs = context.Players.Select(s => s.ID).ToArray();
                int playerIDCount = playerIDs.Length;
                int[] teamIDs = context.Teams.Select(s => s.ID).ToArray();
                int conditionIDCount = teamIDs.Length;

                //PlayerTeams - the Intersection
                //Add a few players to each Team
                if (!context.PlayerTeams.Any())
                {
                    //i loops through the primary keys of the Teams
                    //j is just a counter so we add some Players to a Teams
                    //k lets us step through all Players so we can make sure each gets used
                    int k = 0;//Start with the first Condition
                    foreach (int i in teamIDs)
                    {
                        int howMany = random.Next(1, 6);//Add up to 6 players
                        for (int j = 1; j <= howMany; j++)
                        {
                            k = (k >= playerIDCount) ? 0 : k;//Resets counter k to 0 if we have run out of Players
                            PlayerTeam ds = new PlayerTeam()
                            {
                                TeamID = i,
                                PlayerID = playerIDs[k]
                            };
                            k++;
                            context.PlayerTeams.Add(ds);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
