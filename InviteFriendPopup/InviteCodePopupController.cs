using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Zenject;

namespace UI.Popups.InviteFriendPopup
{
    public class InviteCodePopupController : BasePopupController
    {
        [SerializeField]
        private InviteCodePopupView _view;

        private InviteCodePopupModel _model;
        
        [Inject] private PopupManager PopupManager { get; }
        [Inject] private MusicManager MusicManager { get; }

        public override void Init(BasePopupModel baseModel)
        {
            var isModelCorrect = baseModel is InviteCodePopupModel;

            if (!isModelCorrect)
            {
                Debug.LogError($"{ baseModel.GetType().Name } is wrong settings! Please verify type of model");
                return;
            }

            _model = baseModel as InviteCodePopupModel;
            
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