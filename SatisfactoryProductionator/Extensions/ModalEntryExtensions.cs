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

        public static string GetCompletionHeader(this ModalEntry modalEntry)
        {
            var unlockEntry = modalEntry.CodexEntry as Unlock;

            return unlockEntry.CompletionPage.PageType.ToString();
        }

        public static int GetPageCount(this ModalEntry modalEntry) 
        {
            return modalEntry.CodexEntry.Pages.Count;
        }

        public static List<string> GetEntries(this ModalEntry modalEntry)
        {
            var index = modalEntry.Index;

            return modalEntry.CodexEntry.Pages[index].Entries;
        }
    }
}
