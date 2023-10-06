using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services
{
    public class CodexMenuState
    {
        public static CodexCategory SelectedCategory { get; set; } = CodexCategory.Item;

        public static InfrastructureSubCategory SelectedInfrastructure { get; set; } = InfrastructureSubCategory.Foundations;

        public static UnlockSubCategory SelectedUnlock { get; set; } = UnlockSubCategory.Milestone;

        public event Action OnStateChange;

        public void SetCategory(CodexCategory category)
        {
            SelectedCategory = category;

            NotifyStateChanged();
        }

        public void SetInfrastructure(InfrastructureSubCategory category)
        {
            SelectedInfrastructure = category;

            NotifyStateChanged();
        }

        public void SetUnlock(UnlockSubCategory category)
        {
            SelectedUnlock = category;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}