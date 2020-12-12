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
        Random rng = new Random();
        int currenthomegoal = 0;
        int currentawaygoal = 0;
        int currentattack;
        int currentmidfield;
        int currentdefense;
        int currentgoalkeeper;
        double opportunityprob;
        double goalprob;
        int numberofteams;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.DataSource = TEAMs;
        }

        private void LoadData() {
            TEAMs = context.TEAMS.ToList();
        }

        //lejátssza az egyes mérkőzéseket
        private void SimulateMatches() {

            numberofteams = TEAMs.Count();
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
                        currentdefense = (int)(from x in TEAMs
                                                where x.ID == j + 1
                                                select x.MIDFIELD_LEVEL).First();
                        currentgoalkeeper = (int)(from x in TEAMs
                                                where x.ID == j + 1
                                                select x.MIDFIELD_LEVEL).First();
                        currenthomegoal = GetGoalsScored(currentattack, currentmidfield, 
                            currentdefense, currentgoalkeeper);
                        //Idegen csapat góljainak a száma
                        currentattack = (int)(from x in TEAMs
                                              where x.ID == j + 1
                                              select x.ATTACK_LEVEL).First();
                        currentmidfield = (int)(from x in TEAMs
                                                where x.ID == j + 1
                                                select x.MIDFIELD_LEVEL).First();
                        currentdefense = (int)(from x in TEAMs
                                               where x.ID == i + 1
                                               select x.MIDFIELD_LEVEL).First();
                        currentgoalkeeper = (int)(from x in TEAMs
                                                  where x.ID ==  + 1
                                                  select x.MIDFIELD_LEVEL).First();
                        currentawaygoal = GetGoalsScored(currentattack, currentmidfield,
                            currentdefense, currentgoalkeeper);
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

        //kiszámolja, hogy az adott meccsen az egyik fél hány gól fog rúgni
        private int GetGoalsScored(int attacklevel, int midfieldlevel, 
            int enemydefenselevel, int enemygoalkeeperlevel) {

            int goalsscored = 0;
            opportunityprob = midfieldlevel / (double)(midfieldlevel + enemydefenselevel);
            goalprob = (double)attacklevel / (double)(attacklevel + enemygoalkeeperlevel);
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

        //Eredmények összesítése
        private void RankTeams() {
            for (int n = 0; n < numberofteams; n++)
            {
                int allhomepoints = (from y in gameResults 
                                     where y.HomeTeamID == n + 1 
                                     select y.HomeTeamPoints).Sum();
                int allawaypoints = (from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     select y.AwayTeamPoints).Sum();
                int allhomegoals = (from y in gameResults
                                     where y.HomeTeamID == n + 1
                                     select y.HomeTeamGoals).Sum();
                int allawaygoals = (from y in gameResults
                                     where y.AwayTeamID == n + 1
                                     select y.AwayTeamGoals).Sum();
                int allhomegoalsget = (from y in gameResults
                                       where y.HomeTeamID == n + 1
                                       select y.AwayTeamGoals).Sum();
                int allawaygoalsget = (from y in gameResults
                                       where y.AwayTeamID == n + 1
                                       select y.HomeTeamGoals).Sum();
                string teamname = (from y in TEAMs
                                   where y.ID == n + 1
                                   select y.NAME).First();
                LeagueResult leagueResult = new LeagueResult();
                leagueResult.teamname = teamname;
                leagueResult.totalpoints = allhomepoints + allawaypoints;
                leagueResult.totalgoalsscored = allhomegoals + allawaygoals;
                leagueResult.totalgoalsget = allhomegoalsget + allawaygoalsget;
                leagueResult.totalgoaldifference = (leagueResult.totalgoalsscored) - (leagueResult.totalgoalsget);
                leagueResults.Add(leagueResult);
            }
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            SimulateMatches();
            dataGridView2.DataSource = gameResults;
            RankTeams();
            dataGridView3.DataSource = (from z in leagueResults
                                        orderby z.totalpoints descending,
                                         z.totalgoaldifference descending,
                                         z.totalgoalsscored descending
                                        select z).ToList();
        }

        //Eredmények kiíratása csv fileba
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
                    sw.Write("Helyezés,Csapatnév,Pontszám,Gólkülönbség,Rúgott gólok száma,Kapott gólok száma");
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
                        sw.WriteLine();
                        rank = rank + 1;
                    }
                }
            }
            
        }
    }
}
