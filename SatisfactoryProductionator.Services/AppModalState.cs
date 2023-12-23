using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services
{
    public class AppModalState
    {
        public event Action OnStateChange;
        public ModalType modal { get; set; }

        public void SetModal(ModalType modalType)
        {
            modal = modalType;
        }

        public ModalType GetModalType()
        {
            return modal;
        }


        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
