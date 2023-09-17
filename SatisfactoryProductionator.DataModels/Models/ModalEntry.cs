using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.DataModels.Models
{
    public class ModalEntry
    {
        public CodexItem CodexItem { get; set; }
        public int Index { get; set; }
        public CodexPageCategoryType Category { get; set; }
    }
}
