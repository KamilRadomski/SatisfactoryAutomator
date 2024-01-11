using SatisfactoryProductionator.DataModels.Models.Settings;

namespace SatisfactoryProductionator.Services.States

{
    public class SettingsState
    {
        public AppSettings Value { get; set; }
        public event Action OnStateChange;

        public void SetValue(AppSettings value)
        {
            var minInput = value.MinRecycleInput;
            value.MinRecycleInput = minInput < 30 ? 30 : minInput > 9999 ? 9999 : minInput;

            Value = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
