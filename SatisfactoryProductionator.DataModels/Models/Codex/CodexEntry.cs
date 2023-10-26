using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class CodexEntry
    {
        public string ClassName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public List<string> UnlockBy { get; set; }
        public CodexCategory CodexCategory { get; set; }
        public CodexItemType CodexItemType { get; set; }
        public CodexSubItemType CodexSubItemType { get; set; }    
        public List<CodexPage> Pages { get; set; }
    }
}
