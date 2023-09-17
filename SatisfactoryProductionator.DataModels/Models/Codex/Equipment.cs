﻿namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Equipment : CodexItem
    {
        public int ResourceSinkPoints { get; set; }
        public string StackSize { get; set; }
        public List<string> CompatibleItems { get; set; }

    }
}
