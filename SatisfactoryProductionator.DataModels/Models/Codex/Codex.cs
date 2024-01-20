namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Codex
    {
        public List<CodexEntry> CodexItems { get; set; }
        public List<Recipe> Recipes { get; set; }

        public List<Extraction> Extractions { get; set; }
    }
}
