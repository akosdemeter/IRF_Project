using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRF_Project.Entities
{
    public class LeagueResult
    {
        public string teamname { get; set; }
        public int totalpoints { get; set; }
        public int totalgoaldifference { get; set; }
        public int totalgoalsscored { get; set; }
        public int totalgoalsget { get; set; }
        public int totalwins { get; set; }
        public int totaldraws { get; set; }
        public int totallosses {get; set;}
    }
}
