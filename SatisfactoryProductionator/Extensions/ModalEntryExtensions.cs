using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;

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
    }
}
