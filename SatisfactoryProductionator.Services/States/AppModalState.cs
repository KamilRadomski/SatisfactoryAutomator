using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.Services.States
{
    public class AppModalState
    {
        private readonly CodexModalState codexModalState;
        private readonly PermModalState permModalState;

        public event Action OnStateChange;
        public ModalType modal { get; set; }

        public AppModalState(CodexModalState codexModalState, PermModalState permModalState)
        {
            this.codexModalState = codexModalState;
            this.permModalState = permModalState;
        }

        public void SetModal(ModalType modalType)
        {
            modal = modalType;
        }

        public ModalType GetModalType()
        {
            return modal;
        }


        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public bool Active() => codexModalState.Active || permModalState.Active;
    }
}
