using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataModels.Models
{
    public class Codex
    {
        public List<CodexItem> CodexItems { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
