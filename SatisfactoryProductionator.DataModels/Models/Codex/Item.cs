using SatisfactoryProductionator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Item : CodexEntry
    {
        public int ResourceSinkPoints { get; set; }
        public string StackSize { get; set; }
        public double EnergyValue { get; set; }
        public FormType FormType { get; set; }
    }
}
