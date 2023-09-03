using SatisfactoryProductionator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Building : CodexItem
    {
        public double? PowerUsed { get; set; }
        public double[]? VariablePowerUsed { get; set; }
        public double PowerGenerated { get; set; }
        public (double, double)? Size { get; set; }
    }
}
