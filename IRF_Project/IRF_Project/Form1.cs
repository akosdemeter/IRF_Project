using IRF_Project.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_Project
{
    public partial class Form1 : Form
    {
        IRF_assignmentEntities context = new IRF_assignmentEntities();
        List<TEAM> TEAMs;
        List<GameResult> gameResults = new List<GameResult>();
        List<LeagueResult> leagueResults = new List<LeagueResult>();
        List<GameProbability> gameProbabilities = new List<GameProbability>();
        Random rng = new Random();
        int numberofteams;

        //-------------------------------------------------------------------
        //Konstruktor
        public Form1()
        {
            InitializeComponent();
            LoadData();
            dgvTeams.DataSource = (from x in TEAMs
                                   select new
                                   {
                                       Csapatnév = x.NAME,
                                       Támadók_erőssége = x.ATTACK_LEVEL,
                                       Középpálya_erőssége = x.MIDFIELD_LEVEL,
                                       Védelem_erőssége = x.DEFENSE_LEVEL,
                                       Kapus_erőssége = x.GOALKEEPER_LEVEL
                                   }).ToList();
        }

        //-------------------------------------------------------------------
        //Betöltés adatbázisból a listába és csapatok számának megállapítása
        private void LoadData() {
            TEAMs = context.TEAMS.ToList();
            numberofteams = TEAMs.Count();
        }

        //-------------------------------------------------------------------
        //lejátssza az egyes mérkőzéseket
        private void SimulateMatches()
        {
            int currenthomegoal;
            int currentawaygoal;
            int currentattack;
            int currentmidfield;
            int currentenemydefense;
            int currentenemygoalkeeper;

            CreateMatches();
            RandomizeMatchOrder();
            //A mérkőzések lejátszása
            var gamestobeplayed = (from y in gameResults
                                   select y).ToList();
            int counter = 0;
            foreach (var game in gamestobeplayed)
            {
                //Hazai csapat góljainak a száma
                currentattack = (int)(from x in TEAMs
                                      where x.ID == game.HomeTeamID
                                      select x.ATTACK_LEVEL).First();
                currentmidfield = (int)(from x in TEAMs
                                        where x.ID == game.HomeTeamID
                                        select x.MIDFIELD_LEVEL).First();
                currentenemydefense = (int)(from x in TEAMs
                                            where x.ID == game.AwayTeamID
                                            select x.MIDFIELD_LEVEL).First();
                currentenemygoalkeeper = (int)(from x in TEAMs
                                               where x.ID == game.AwayTeamID
                                               select x.MIDFIELD_LEVEL).First();
                GameProbability gameProbability = new GameProbability();
                gameProbability.hometeamid = game.HomeTeamID;
                gameProbability.hometeamopportunityprob = (double)currentmidfield /
                                                        (double)(currentmidfield + currentenemydefense);
                gameProbability.hometeamgoalprob = (double)currentattack /
                                                (double)(currentattack + currentenemygoalkeeper);
                currenthomegoal = GetGoalsScored(gameProbability.hometeamopportunityprob,
                                  gameProbability.hometeamgoalprob);
                //Idegen csapat góljainak a száma
                currentattack = (int)(from x in TEAMs
                                      where x.ID == game.AwayTeamID
                                      select x.ATTACK_LEVEL).First();
                currentmidfield = (int)(from x in TEAMs
                                        where x.ID == game.AwayTeamID
                                        select x.MIDFIELD_LEVEL).First();
                currentenemydefense = (int)(from x in TEAMs
                                            where x.ID == game.HomeTeamID
                                            select x.MIDFIELD_LEVEL).First();
                currentenemygoalkeeper = (int)(from x in TEAMs
                                               where x.ID == game.HomeTeamID
                                               select x.MIDFIELD_LEVEL).First();
                gameProbability.awayteamid = game.AwayTeamID;
                gameProbability.awayteamopportunity = (double)currentmidfield /
                                                        (double)(currentmidfield + currentenemydefense);
                gameProbability.awayteamgoalprob = (double)currentattack /
                                                (double)(currentattack + currentenemygoalkeeper);
                currentawaygoal = GetGoalsScored(gameProbability.awayteamopportunity,
                                  gameProbability.awayteamgoalprob);
                gameProbabilities.Add(gameProbability);
                //Mérkőzés eredményének frissítése
                gameResults[counter].HomeTeamGoals = currenthomegoal;
                gameResults[counter].AwayTeamGoals = currentawaygoal;
                gameResults[counter].HomeTeamPoints = GetPointsEarned(currenthomegoal, currentawaygoal);
                gameResults[counter].AwayTeamPoints = GetPointsEarned(currentawaygoal, currenthomegoal);
                counter++;
            }
        }

        //-------------------------------------------------------------------
        //Mérkőzések sorrendjének összekeverése
        private void RandomizeMatchOrder()
        {
            int numberofmatches = gameResults.Count;
            for (int i = 0; i < numberofmatches; i++)
            {
                var r = rng.Next(numberofmatches);
                var tmp = gameResults[i];
                gameResults[i] = gameResults[r];
                gameResults[r] = tmp;
            }
        }

        //-------------------------------------------------------------------
        //Lejátszandó mérkőzések rögzítése
        private void CreateMatches()
        {
            for (int i = 0; i < numberofteams; i++)
            {
                for (int j = 0; j < numberofteams; j++)
                {
                    if (i != j)
                    {
                        GameResult gameResult = new GameResult();
                        gameResult.HomeTeamID = i + 1;
                        gameResult.AwayTeamID = j + 1;
                        gameResults.Add(gameResult);
                    }
                }
            }
        }

        //-------------------------------------------------------------------
        //kiszámolja, hogy az adott meccsen az egyik fél hány gólt fog rúgni
        private int GetGoalsScored(double opportunityprob, double goalprob) {
            int goalsscored = 0;
            int createdoppotunities = 0;
            for (int i = 0; i < 10; i++)
            {
                if (rng.NextDouble() <= opportunityprob)
                {
                    createdoppotunities++;
                }
            }
            for (int i = 0; i < createdoppotunities; i++)
            {
                if (rng.NextDouble() <= goalprob)
                {
                    goalsscored++;
                }
            }
            return goalsscored;
        }

        //-------------------------------------------------------------------
        //a gólok alapján kiszámolja a csapat pontjait
        private int GetPointsEarned(int goalsscored, int goalsgot) {
            int points;
            if (goalsscored > goalsgot)
            {
                points = 3;
            }
            else
            {
                if (goalsscored == goalsgot)
                {
                    points = 1;
                }
                else
                {
                    points = 0;
                }
            }
            return points;
        }

        //-------------------------------------------------------------------
        //Eredmények összesítése
        private void RankTeams() {
            int allhomepoints;
            int allawaypoints;
            int allhomegoals;
            int allawaygoals;
            int allhomegoalsget;
            int allawaygoalsget;
            int allhomewins;
            int allhomedraws;
            int allhomelosses;
            int allawaywins;
            int allawaydraws;
            int allawaylosses;
            string teamname;

            for (int n = 0; n < numberofteams; n++)
            {
                allhomepoints = (int)(from y in gameResults 
                                     where y.HomeTeamID == n + 1 
                                     select y.HomeTeamPoints).Sum();
                allawaypoints = (int)(from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     select y.AwayTeamPoints).Sum();
                allhomegoals = (int)(from y in gameResults
                                     where y.HomeTeamID == n + 1
                                     select y.HomeTeamGoals).Sum();
                allawaygoals = (int)(from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     select y.AwayTeamGoals).Sum();
                allhomegoalsget = (int)(from y in gameResults
                                       where y.HomeTeamID == n + 1
                                       select y.AwayTeamGoals).Sum();
                allawaygoalsget = (int)(from y in gameResults
                                       where y.AwayTeamID == n + 1
                                       select y.HomeTeamGoals).Sum();
                allhomewins = (from y in gameResults
                                   where y.HomeTeamID == n + 1
                                   && y.HomeTeamPoints == 3
                                   select y).Count();
                allhomedraws = (from y in gameResults
                                    where y.HomeTeamID == n + 1
                                    && y.HomeTeamPoints == 1
                                    select y).Count();
                allhomelosses = (from y in gameResults
                                     where y.HomeTeamID == n + 1
                                     && y.HomeTeamPoints == 0
                                     select y).Count();
                allawaywins = (from y in gameResults
                                   where y.AwayTeamID == n + 1
                                   && y.AwayTeamPoints == 3
                                   select y).Count();
                allawaydraws = (from y in gameResults
                                    where y.AwayTeamID == n + 1
                                    && y.AwayTeamPoints == 1
                                    select y).Count();
                allawaylosses = (from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     && y.AwayTeamPoints == 0
                                     select y).Count();
                teamname = (from y in TEAMs
                                   where y.ID == n + 1
                                   select y.NAME).First();
                LeagueResult leagueResult = new LeagueResult();
                leagueResult.teamname = teamname;
                leagueResult.totalpoints = allhomepoints + allawaypoints;
                leagueResult.totalgoalsscored = allhomegoals + allawaygoals;
                leagueResult.totalgoalsget = allhomegoalsget + allawaygoalsget;
                leagueResult.totalgoaldifference = leagueResult.totalgoalsscored - leagueResult.totalgoalsget;
                leagueResult.totalwins = allhomewins + allawaywins;
                leagueResult.totaldraws = allhomedraws + allawaydraws;
                leagueResult.totallosses = allhomelosses + allawaylosses;
                leagueResults.Add(leagueResult);
            }
        }

        //-------------------------------------------------------------------
        //Szimuláció végrehajtása gombnyomásra
        private void btnSimulation_Click(object sender, EventArgs e)
        {
            gameProbabilities.Clear();
            gameResults.Clear();
            leagueResults.Clear();
            SimulateMatches();
            RankTeams();
            int ranking = 1;
            dgvResults.DataSource = (from z in leagueResults
                                        orderby z.totalpoints descending,
                                         z.totalgoaldifference descending,
                                         z.totalgoalsscored descending
                                        select new {
                                            Helyezés = ranking++, 
                                            Csapatnév = z.teamname, 
                                            Pontszám = z.totalpoints,
                                            Gólkülönbség = z.totalgoaldifference, 
                                            Rúgott_Gólok_Száma = z.totalgoalsscored,
                                            Kapott_Gólok_Száma = z.totalgoalsget,
                                            Győzelmek = z.totalwins,
                                            Döntetlenek = z.totaldraws,
                                            Vereségek = z.totallosses
                                        }).ToList();
        }

        //-------------------------------------------------------------------
        //Tabellaeredmények kiíratása csv fileba gombnyomásra
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (leagueResults.Count!=0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Comma Separated Values (*.csv)|*.csv";
                sfd.DefaultExt = "csv";
                sfd.AddExtension = true;
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    int rank = 1;
                    sw.Write("Helyezés,Csapatnév,Pontszám,Gólkülönbség,Rúgott gólok száma,Kapott gólok száma,Győzelmek,Döntetlenek,Vereségek");
                    sw.WriteLine();
                    var orderedResults = (from z in leagueResults
                                          orderby z.totalpoints descending,
                                          z.totalgoaldifference descending,
                                          z.totalgoalsscored descending
                                          select z).ToList();
                    foreach (var row in orderedResults)
                    {
                        sw.Write(rank);
                        sw.Write(",");
                        sw.Write(row.teamname);
                        sw.Write(",");
                        sw.Write(row.totalpoints);
                        sw.Write(",");
                        sw.Write(row.totalgoaldifference);
                        sw.Write(",");
                        sw.Write(row.totalgoalsscored);
                        sw.Write(",");
                        sw.Write(row.totalgoalsget);
                        sw.Write(",");
                        sw.Write(row.totalwins);
                        sw.Write(",");
                        sw.Write(row.totaldraws);
                        sw.Write(",");
                        sw.Write(row.totallosses);
                        sw.WriteLine();
                        rank++;
                    }
                }
            }    
        }

        //-------------------------------------------------------------------
        //Mérkőzéseredmények kiíratása csv fileba gombnyomásra
        private void btngameexport_Click(object sender, EventArgs e)
        {
            if (gameResults.Count != 0)
            {
                string hometeam;
                string awayteam;
                SaveFileDialog sfd2 = new SaveFileDialog();
                sfd2.InitialDirectory = Application.StartupPath;
                sfd2.Filter = "Comma Separated Values (*.csv)|*.csv";
                sfd2.DefaultExt = "csv";
                sfd2.AddExtension = true;
                if (sfd2.ShowDialog() != DialogResult.OK) return;
                using (StreamWriter sw = new StreamWriter(sfd2.FileName, false, Encoding.UTF8))
                {
                    sw.Write("Hazai csapat,Vendég csapat, Hazai csapat góljai, Vendég csapat góljai");
                    sw.WriteLine();
                    var listofgames = (from v in gameResults
                                       select v).ToList();
                    foreach (var game in listofgames)
                    {
                        hometeam = (from z in TEAMs 
                                    where z.ID == game.HomeTeamID 
                                    select z.NAME).First();
                        awayteam = (from z in TEAMs
                                    where z.ID == game.AwayTeamID
                                    select z.NAME).First();
                        sw.Write(hometeam);
                        sw.Write(",");
                        sw.Write(awayteam);
                        sw.Write(",");
                        sw.Write(game.HomeTeamGoals);
                        sw.Write(",");
                        sw.Write(game.AwayTeamGoals);
                        sw.WriteLine();
                    }
                }
            }
        }
    }
}
