using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Extensions
{
    public static class ModalEntryExtensions
    {
        public static PageType GetPageType(this ModalEntry modalEntry)
        {
            var index = modalEntry.Index;

            return modalEntry.CodexEntry.Pages[index].PageType;
        }

        public static string GetPageHeader(this ModalEntry modalEntry)
        {
            return modalEntry.GetPageType().ToString();
        }

        public static int GetPageCount(this ModalEntry modalEntry) 
        {
            return modalEntry.CodexEntry.Pages.Count;
        }

        public static List<string> GetEntries(this ModalEntry modalEntry)
        {
            var index = modalEntry.Index;

            if (modalEntry.CodexEntry.Pages.Count > 0)
            {
                return modalEntry.CodexEntry.Pages[index].Entries;
            }
            else return new List<string>();
            
        }

        public static List<List<KeyValuePair<string, double[]>>> BundleInputs(this ModalEntry modalEntry, List<Recipe> recipes, int rowSplit = 0)
        {
            List<List<KeyValuePair<string, double[]>>> items = new();

            foreach(var recipe in recipes)
            {
                var inputs = recipe.Inputs.ToList();

                if(rowSplit == 0)
                {
                    rowSplit = GetRowSplit(inputs.Count);
                }
                
                var tempList = new List<KeyValuePair<string, double[]>>();

                foreach (var entry in inputs)
                {
                    tempList.Add(entry);

                    if (tempList.Count % rowSplit == 0)
                    {
                        items.Add(tempList);
                        tempList = new List<KeyValuePair<string, double[]>>();
                    }
                }

                if (tempList.Count > 0)
                {
                    items.Add(tempList);
                }
            }

            return items;
        }

        public static List<List<string>> BundleItems(this ModalEntry modalEntry)
        {
            List<List<string>> items = new();

            var tempList = new List<string>();

            var entries = GetEntries(modalEntry);
            var rowSplit = GetRowSplit(entries.Count);

            foreach (var entry in entries)
            {
                tempList.Add(entry);

                if (tempList.Count % rowSplit == 0)
                {
                    items.Add(tempList);
                    tempList = new List<string>();
                }
            }

            if (tempList.Count > 0)
            {
                items.Add(tempList);
            }

            return items;
        }

        public static List<List<KeyValuePair<string, string>>> BundleFuels(this ModalEntry modelEntry, List<KeyValuePair<string, string>> entries)
        {
            List<List<KeyValuePair<string, string>>> items = new();

            var tempList = new List<KeyValuePair<string, string>>();

            var rowSplit = GetRowSplit(entries.Count);

            foreach (var entry in entries)
            {
                tempList.Add(entry);

                if (tempList.Count % rowSplit == 0)
                {
                    items.Add(tempList);
                    tempList = new List<KeyValuePair<string, string>>();
                }
            }

            if (tempList.Count > 0)
            {
                items.Add(tempList);
            }

            return items;
        
        }

        public static List<List<CodexEntry>> BundleEntries(this ModalEntry modalEntry, List<CodexEntry> entries)  
        {
            List<List<CodexEntry>> items = new();

            var tempList = new List<CodexEntry>();

            var rowSplit = GetRowSplit(entries.Count);

            foreach (var entry in entries)
            {
                tempList.Add(entry);

                if (tempList.Count % rowSplit == 0)
                {
                    items.Add(tempList);
                    tempList = new List<CodexEntry>();
                }
            }

            if (tempList.Count > 0)
            {
                items.Add(tempList);
            }

            return items;
        }

        private static int GetRowSplit(int cnt)
        {
            if (cnt <= 4) return 4;

            if (cnt > 10) return 6;

            if (cnt % 2 == 1) cnt++;

            if (cnt % 5 == 0) return 5;
            else if (cnt % 4 == 0) return 4;
            else return 3;
        }
    }
}
