using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services
{
    public class CodexMenuState
    {
        public static CodexCategory SelectedCategory { get; set; } = CodexCategory.Item;

        public static InfrastructureSubCategory SelectedInfrastructure { get; set; } = InfrastructureSubCategory.Foundations;

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

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}