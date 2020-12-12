﻿using IRF_Project.Entities;
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
        int currenthomegoal = 0;
        int currentawaygoal = 0;
        int currentattack;
        int currentmidfield;
        int currentdefense;
        int currentgoalkeeper;

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
                    }
                }
            }
        }

        //kiszámolja, hogy az adott meccsen az egyik fél hány gól fog rúgni
        private int GetGoalsScored(int attacklevel, int midfieldlevel, 
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
            return goalsscored;
        }


    }
}
