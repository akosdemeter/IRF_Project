using IRF_Project.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        Random rng = new Random(); //a seed megadása nem biztos, hogy fog kelleni
        int currenthomegoal = 0;
        int currentawaygoal = 0;
        int currentattack;
        int currentmidfield;
        int currentdefense;
        int currentgoalkeeper;
        double opportunityprob;
        double goalprob;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            SimulateMatches();
            dataGridView2.DataSource = gameResults;
        }

        private void LoadData() {
            //TEAMs.Clear();
            TEAMs = context.TEAMS.ToList();
            dataGridView1.DataSource = TEAMs;
            //gameResults.Clear();
        }

        //lejátssza az egyes mérkőzéseket
        private void SimulateMatches() {

            int numberofteams = TEAMs.Count();
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
    }
}
