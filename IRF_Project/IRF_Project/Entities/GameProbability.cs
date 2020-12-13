using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRF_Project.Entities
{
    public class GameProbability
    {
        public int hometeamid { get; set; }
        public double hometeamopportunityprob { get; set; }
        public double hometeamgoalprob { get; set; }
        public int awayteamid { get; set; }
        public double awayteamopportunity { get; set; }
        public double awayteamgoalprob { get; set; }
    }
}
