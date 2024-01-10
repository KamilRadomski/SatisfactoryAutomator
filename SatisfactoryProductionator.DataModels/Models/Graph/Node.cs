using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class Node
    {
        public string Name { get; set; }
        public NodeType NodeType { get; set; }
        public double ItemQuantity { get; set; }
        public string Recipe { get; set; }
        public double RecipeQuantity { get; set; }
        public string Building { get; set; }
        public int BuildingQuantity { get; set; }
        public Dictionary<string, double> InfraCost { get; set; } = new Dictionary<string, double>();
        public double PowerUsed { get; set; }
    }
}
