using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.CodexPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class CodexItem
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }

        public CodexItemType CodexItemType { get; set; }

        public List<CodexPage> Pages { get; set; }
    }
}
