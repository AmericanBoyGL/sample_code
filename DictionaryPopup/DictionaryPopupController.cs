using Managers;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace UI.Popups.DictionaryPopup
{
    public class DictionaryPopupController:BasePopupController
    {
        [SerializeField] private DictionaryPopupView _view;
        private DictionaryPopupModel _model;
        
        [Inject] private PopupManager _popupManager;

        public override void Init(BasePopupModel baseModel)
        {
            _model = baseModel as DictionaryPopupModel;
            Assert.IsNotNull(_model, "BasePopupModel is not DictionaryPopupModel");
            _model.OnClickCloseButton = OnClickCloseButton;

            _view.Init(_model);
        }

        private void OnClickCloseButton()
        {
            _popupManager.HidePopup(PopupName);
        }
    }
}