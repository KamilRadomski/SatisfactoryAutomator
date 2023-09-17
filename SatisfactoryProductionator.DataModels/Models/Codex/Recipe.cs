using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Recipe
    {
        public string ClassName { get; set; }
        public string? DisplayName { get; set; }
        public double BuildTime { get; set; }
        public string Building { get; set; }
        public bool BuildGun { get; set; }
        public string CraftBuilding { get; set; }
        public RecipeType RecipeType { get; set; }
        public Dictionary<string, double[]> Inputs { get; set; }
        public Dictionary<string, double[]> Outputs { get; set; }
    }
}
