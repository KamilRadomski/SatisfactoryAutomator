using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.DataModels.Models.Modal
{
    public class ModalEntry
    {
        public CodexEntry CodexEntry { get; set; }
        public int Index { get; set; }

        public string SelectedRecipe { get; set; }
    }
}
