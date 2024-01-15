using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.DataModels.Models.Graph
{
    public class PermData
    {
        public bool Active = true;

        public List<string> Inputs = new List<string>();
        public List<string> Outputs = new List<string>();
        public List<string> Recipes = new List<string>();
        public List<string> Items = new List<string>();
        public List<string> Buildings = new List<string>();
        public List<string> Costs = new List<string>();
    }
}
