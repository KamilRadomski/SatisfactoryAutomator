using SatisfactoryProductionator.DataModels.Models.Codex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class ItemData
    {
        public double Quantity { get; set; }
        public Recipe Recipe { get; set; }
        public double RecipeQuantity { get; set; }
        public Building Building { get; set; }
    }
}
