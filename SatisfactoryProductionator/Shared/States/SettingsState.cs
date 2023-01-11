using SatisfactoryProductionator.Shared.States.Models;

namespace SatisfactoryProductionator.Shared.States
{
	public class SettingsState
	{
		public AppSettings Value { get; set; }
		public event Action OnStateChange;

		public void SetValue(AppSettings value)
		{
			this.Value = value;
			NotifyStateChanged();
		}

		private void NotifyStateChanged() => OnStateChange?.Invoke();
	}
}
