using System;
using TMPro;
using UniRx;
using UnityEngine;
using Managers;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Player;

using Client = Managers.Client;

namespace UI.Popups.InviteFriendPopup
{
    public class InviteFriendPopupView:BasePopupView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _idUser;
        [SerializeField] private Button _copyCodeBtn;
        [SerializeField] private TMP_Text _inviteMessage;
        [SerializeField] private GameObject _inviteMessageObj;

        private IDisposable _closeDisposable;
        private IDisposable _copyDisposable;
        private IDisposable _confirmDisposable;
        
        private InviteFriendPopupModel _model;

        [Inject] private PlayerController playerController;
        [Inject] private Client _client;

        public void Init(InviteFriendPopupModel model)
        {
            _model = model;
            _inviteMessageObj.SetActive(false);
            
            _idUser.text = playerController.GetUserId().ToString();
            //_rewardAmount.text = $"${rewardBucks:F2}";
            
            _closeDisposable?.Dispose();
            _closeDisposable = _closeButton.OnClickAsObservable().Subscribe(_=> OnClickCloseButton());
            
            _copyDisposable?.Dispose();
            _copyDisposable = _copyCodeBtn.OnClickAsObservable().Subscribe(_ => OnClickCopyCodeBtn(playerController.GetUserId().ToString()));
        }

        private void OnClickCopyCodeBtn(string textToCopy)
        {
            TextEditor editor = new TextEditor
            {
                text = textToCopy
            };
            editor.SelectAll();
            editor.Copy();
            
            _inviteMessageObj.SetActive(true);
            _inviteMessage.text = "Send this to your mate to enter into his referral window to get mutual bonuses";
        }

        private void OnClickCloseButton()
        {
            _model?.closeCallback();
        }
    }
}