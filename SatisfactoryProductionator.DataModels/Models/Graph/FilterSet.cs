using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class FilterSet
    {
        HashSet<Item> Inputs = new HashSet<Item>();
        HashSet<Item> Outputs = new HashSet<Item>();
        HashSet<Item> Items = new HashSet<Item>();
        HashSet<Building> Buildings = new HashSet<Building>();
        HashSet<Item> Costs = new HashSet<Item>();
    }
}
