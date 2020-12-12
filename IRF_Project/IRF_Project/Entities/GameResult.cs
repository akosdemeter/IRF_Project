using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRF_Project.Entities
{
    public class GameResult
    {
        int HomeTeamID { get; set; }
        int AwayTeamID { get; set; }
        int HomeTeamGoals { get; set; }
        int AwayTeamGoals { get; set; }
        int HomeTeamPoints { get; set; }
        int AwayTeamPoints { get; set; }
    }
}
