namespace SatisfactoryProductionator.Services
{
    public class PermState
    {
        public event Action OnStateChange;

        private void NotifyStateChanged() => OnStateChange?.Invoke();


    }
}
