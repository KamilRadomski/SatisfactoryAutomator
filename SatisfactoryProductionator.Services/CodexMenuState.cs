using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services
{
    public class CodexMenuState
    {
        public static CodexCategory SelectedCategory { get; set; } = CodexCategory.Item;

        public event Action OnStateChange;

        public void SetValue(CodexCategory category)
        {
            SelectedCategory = category;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}