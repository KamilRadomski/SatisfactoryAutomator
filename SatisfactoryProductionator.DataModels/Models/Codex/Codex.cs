using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models.Codex
{
    public class Codex
    {
        public List<CodexEntry> CodexItems { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
