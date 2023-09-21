using SatisfactoryProductionator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class CodexPage
    {
        public PageType PageType { get; set; }
        public List<string>? Recipes { get; set; }

    }
}
