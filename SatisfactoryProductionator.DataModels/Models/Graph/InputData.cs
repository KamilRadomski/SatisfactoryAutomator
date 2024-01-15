using SatisfactoryProductionator.DataModels.Models.Codex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class InputData
    {
        public double Quantity { get; set; }
        public Extraction Extraction { get; set; }
        public double Quantifier { get; set; }

        public Building Building { get; set; }
    }
}
