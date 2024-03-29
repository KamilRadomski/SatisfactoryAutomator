﻿using SatisfactoryProductionator.DataModels.Enums;
using System.Xml.Linq;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Item : CodexEntry
    {
        public int ResourceSinkPoints { get; set; }
        public string StackSize { get; set; }
        public double EnergyValue { get; set; }
        public List<string> AutoRecipes { get; set; }
        public FormType FormType { get; set; }
    }
}
