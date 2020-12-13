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
        //Betöltés adatbázisból a listába és csapatokszámának megállapítása
        private void LoadData() {
            TEAMs = context.TEAMS.ToList();
            numberofteams = TEAMs.Count();
        }

        //-------------------------------------------------------------------
        //lejátssza az egyes mérkőzéseket
        private void SimulateMatches() {
            int currenthomegoal;
            int currentawaygoal;
            int currentattack;
            int currentmidfield;
            int currentenemydefense;
            int currentenemygoalkeeper;

            for (int i = 0; i < numberofteams; i++)
            {
                for (int j = 0; j < numberofteams; j++)
                {
                    if (i!=j)
                    {
                        //Hazai csapat góljainak a száma
                        currentattack = (int)(from x in TEAMs 
                                              where x.ID == i + 1 
                                              select x.ATTACK_LEVEL).First();
                        currentmidfield = (int)(from x in TEAMs
                                                where x.ID == i + 1
                                                select x.MIDFIELD_LEVEL).First();
                        currentenemydefense = (int)(from x in TEAMs
                                                where x.ID == j + 1
                                                select x.MIDFIELD_LEVEL).First();
                        currentenemygoalkeeper = (int)(from x in TEAMs
                                                where x.ID == j + 1
                                                select x.MIDFIELD_LEVEL).First();
                        GameProbability gameProbability = new GameProbability();
                        gameProbability.hometeamid = i + 1;
                        gameProbability.hometeamopportunityprob = (double)currentmidfield /
                                                                (double)(currentmidfield + currentenemydefense);
                        gameProbability.hometeamgoalprob = (double)currentattack /
                                                        (double)(currentattack + currentenemygoalkeeper);
                        currenthomegoal = GetGoalsScored(gameProbability.hometeamopportunityprob, 
                                          gameProbability.hometeamgoalprob);
                        
                        //Idegen csapat góljainak a száma
                        currentattack = (int)(from x in TEAMs
                                              where x.ID == j + 1
                                              select x.ATTACK_LEVEL).First();
                        currentmidfield = (int)(from x in TEAMs
                                                where x.ID == j + 1
                                                select x.MIDFIELD_LEVEL).First();
                        currentenemydefense = (int)(from x in TEAMs
                                               where x.ID == i + 1
                                               select x.MIDFIELD_LEVEL).First();
                        currentenemygoalkeeper = (int)(from x in TEAMs
                                                  where x.ID ==  + 1
                                                  select x.MIDFIELD_LEVEL).First();
                        gameProbability.awayteamid = j + 1;
                        gameProbability.awayteamopportunity = (double)currentmidfield /
                                                                (double)(currentmidfield + currentenemydefense);
                        gameProbability.awayteamgoalprob = (double)currentattack /
                                                        (double)(currentattack + currentenemygoalkeeper);
                        currentawaygoal = GetGoalsScored(gameProbability.awayteamopportunity,
                                          gameProbability.awayteamgoalprob);
                        gameProbabilities.Add(gameProbability);
                        
                        //Mérkőzések rögzítése
                        GameResult gameResult = new GameResult();
                        gameResult.HomeTeamID = i + 1;
                        gameResult.AwayTeamID = j + 1;
                        gameResult.HomeTeamGoals = currenthomegoal;
                        gameResult.AwayTeamGoals = currentawaygoal;
                        gameResult.HomeTeamPoints = GetPointsEarned(currenthomegoal, currentawaygoal);
                        gameResult.AwayTeamPoints = GetPointsEarned(currentawaygoal, currenthomegoal);
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
                    createdoppotunities = createdoppotunities + 1;
                }
            }
            for (int i = 0; i < createdoppotunities; i++)
            {
                if (rng.NextDouble() <= goalprob)
                {
                    goalsscored = goalsscored + 1;
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
                allhomepoints = (from y in gameResults 
                                     where y.HomeTeamID == n + 1 
                                     select y.HomeTeamPoints).Sum();
                allawaypoints = (from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     select y.AwayTeamPoints).Sum();
                allhomegoals = (from y in gameResults
                                     where y.HomeTeamID == n + 1
                                     select y.HomeTeamGoals).Sum();
                allawaygoals = (from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     select y.AwayTeamGoals).Sum();
                allhomegoalsget = (from y in gameResults
                                       where y.HomeTeamID == n + 1
                                       select y.AwayTeamGoals).Sum();
                allawaygoalsget = (from y in gameResults
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
        //Eredmények kiíratása csv fileba gombnyomásra
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
                        rank = rank + 1;
                    }
                }
            }    
        }
    }
}
