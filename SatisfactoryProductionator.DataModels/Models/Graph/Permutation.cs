using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class Permutation
    {
        public bool Active { get; set; }

        public Dictionary<string, double> ItemsBuilt { get; set; }

        public Dictionary<string, double> Inputs { get; set; }

        public Dictionary<string, double> RecipesUsed { get; set;}

        public Dictionary<string, int> Buildings { get; set; }

        public Dictionary<string, int> InfraCost { get; set; }

        public Dictionary<string, double> OverFlow { get; set; }

        public double PowerNeeded { get; set; }
    }
}
