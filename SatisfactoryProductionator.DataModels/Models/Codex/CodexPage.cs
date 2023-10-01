using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class CodexPage
    {
        public PageType PageType { get; set; }
        public List<string>? Entries { get; set; }

    }
}
