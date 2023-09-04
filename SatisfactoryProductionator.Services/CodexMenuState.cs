using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services
{
    public class CodexMenuState
    {
        public CodexCategory SelectedCategory { get; set; }

        public event Action OnStateChange;

        public CodexMenuState() 
        {
            SelectedCategory = CodexCategory.Item;
        }
        
        public void SetValue(CodexCategory category)
        {
            SelectedCategory = category;

            NotifyStateChanged();
        }


        private void NotifyStateChanged() => OnStateChange?.Invoke();

       

    }
}