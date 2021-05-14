using System.Collections.Generic;
using AppsFlyerSDK;
using Balance;
using Managers;
using Player;
using UI.Popups.CashOutCongratulationsPopup;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace UI.Popups.InviteFriendPopup
{
    public class InviteFriendPopupController:BasePopupController
    {
        [SerializeField] private InviteFriendPopupView _view;

        [Inject] private Client ClientGRPC { get; }
        [Inject] private PlayerController PlayerController { get; }
        [Inject] private PopupManager PopupManager { get; }
        [Inject] private MusicManager MusicManager { get; }
        
        [Inject] private Client client;

        private InviteFriendPopupModel _model;
        private string _email;
        public override void Init(BasePopupModel baseModel)
        {
            _model = baseModel as InviteFriendPopupModel;
            Assert.IsNotNull(_model, "BasePopupModel is not InviteFriendPopupModel");

            _model.closeCallback = CloseCallback;
            _view.Init(_model);
        }

        private void CloseCallback()
        {
            MusicManager.PlaySound(SoundsNames.MenuButtonClick);
            PopupManager.HidePopup(PopupName);
        }
    }
}