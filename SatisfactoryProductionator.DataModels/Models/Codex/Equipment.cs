﻿namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Equipment : CodexEntry
    {
        public int ResourceSinkPoints { get; set; }
        public string StackSize { get; set; }
        public List<string> CompatibleItems { get; set; }
        public Cost Cost { get; set; }

    }
}
