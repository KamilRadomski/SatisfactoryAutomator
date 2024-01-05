using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class Node
    {
        public Dictionary<string, int> Items { get; set; }
        public List<string> Recipes { get; set; }
        public List<string> UsedRecipes { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
