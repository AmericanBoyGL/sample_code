using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Zenject;

namespace UI.Popups.ReferralReward
{
    public class ReferralCompletePopupController : BasePopupController
    {
        [SerializeField]
        private ReferralCompletePopupView _view;

        private ReferralCompletePopupModel _model;
        
        [Inject] private PopupManager PopupManager { get; }
        [Inject] private MusicManager MusicManager { get; }

        public override void Init(BasePopupModel baseModel)
        {
            var isModelCorrect = baseModel is ReferralCompletePopupModel;

            if (!isModelCorrect)
            {
                Debug.LogError($"{ baseModel.GetType().Name } is wrong settings! Please verify type of model");
                return;
            }

            _model = baseModel as ReferralCompletePopupModel;
            
            _model.closeCallback = CloseCallback;
            
            _view.Initialize(_model);
        }
        
        private void CloseCallback()
        {
            MusicManager.PlaySound(SoundsNames.MenuButtonClick);
            PopupManager.HidePopup(PopupName);
        }
    }
}