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
        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.DataSource = TEAMs;
        }
        private void LoadData() {
            TEAMs = context.TEAMS.ToList();
        }
    }
}
