using SatisfactoryProductionator.Shared.States.Models;

namespace SatisfactoryProductionator.Shared.States
{
	public class SettingsState
	{
		public AppSettings Value { get; set; }
		public event Action OnStateChange;

		public void SetValue(AppSettings value)
		{
			var minInput = value.MinRecycleInput;
			value.MinRecycleInput = minInput < 30 ? 30 : minInput > 9999 ? 9999 : minInput;
			
			this.Value = value;
			NotifyStateChanged();
		}

		private void NotifyStateChanged() => OnStateChange?.Invoke();
	}
}
