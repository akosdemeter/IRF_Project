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
        Random rng = new Random(1234); //a seed megadása nem biztos, hogy fog kelleni

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
            int numberofteams = TEAMs.Count();
            for (int i = 0; i < numberofteams; i++)
            {
                for (int j = 0; j < numberofteams; j++)
                {
                    if (i!=j)
                    {
                        
                    }
                }
            }
        }

        //kiszámolja, hogy az adott meccsen az egyik fél hány gól fog rúgni
        private void GetGoalsScored(int attacklevel, int midfieldlevel, 
            int enemydefenselevel, int enemygoalkeeperlevel) {

            int goalsscored = 0;
            int createdoppotunities = 0;
            for (int i = 0; i < 10; i++)
            {
                if (rng.NextDouble() <= (midfieldlevel / (midfieldlevel + enemydefenselevel)))
                {
                    createdoppotunities = createdoppotunities + 1;
                }
            }
            for (int i = 0; i < createdoppotunities; i++)
            {
                if (rng.NextDouble() <= (attacklevel / (attacklevel + enemygoalkeeperlevel)))
                {
                    goalsscored = goalsscored + 1;
                }
            }
        }


    }
}
