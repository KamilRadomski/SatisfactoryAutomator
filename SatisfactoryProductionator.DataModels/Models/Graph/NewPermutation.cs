using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class NewPermutation
    {
        public int Id { get; set; }
        public Dictionary<Item, InputData> Inputs { get; set; } = new Dictionary<Item, InputData>();
        public Dictionary<Item, ItemData> Outputs { get; set; } = new Dictionary<Item, ItemData>();
        public Dictionary<Item, ItemData> Items { get; set; } = new Dictionary<Item, ItemData>();
        public Dictionary<Building, double> Buildings { get; set; } = new Dictionary<Building,  double>();
        public Dictionary<CodexEntry, double> Cost { get; set; } = new Dictionary<CodexEntry, double>();
        public double Power { get; set; }
    }
}
