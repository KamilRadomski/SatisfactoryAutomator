using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;

namespace SatisfactoryProductionator.Extensions
{
    public static class ModalEntryExtensions
    {
        public static PageType GetPageType(this ModalEntry modalEntry)
        {
            var index = modalEntry.Index;

            return modalEntry.CodexItem.Pages[index].PageType;
        }

        public static string GetPageHeader(this ModalEntry modalEntry)
        {
            return modalEntry.GetPageType().ToString();
        }
    }
}
