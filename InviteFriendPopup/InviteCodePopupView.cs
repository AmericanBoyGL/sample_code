using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popups.InviteFriendPopup
{
    public class InviteCodePopupView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_InputField _idUser;
        [SerializeField] private TMP_Text _messageInvite;
        [SerializeField] private GameObject _messageInviteObj;
        [SerializeField] private Button _sendInviteCode;
        
        private IDisposable _sendCodeButton;
        private IDisposable _closeDisposable;
        
        private InviteCodePopupModel _model;
        
        [Inject] private Client _client;
        
        public void Init(InviteCodePopupModel model)
        {
            _model = model;
            _messageInviteObj.SetActive(false);
            
            _closeDisposable?.Dispose();
            _closeDisposable = _closeButton.OnClickAsObservable().Subscribe(_=> OnClickCloseButton());
            
            _sendCodeButton = _sendInviteCode.OnClickAsObservable().Subscribe(_ => SetReferall(Int32.Parse(_idUser.text)));
        }
        
        private void SetReferall(int id)
        {
            _client.SetReferralById(id, OnSetReferall, OnFailSerReferall);
        }
        
        private void OnSetReferall(Auth.User rez)
        {
            Debug.Log("GOOD = " + rez);
        }

        private void OnFailSerReferall(string rez)
        {
            Debug.Log("ERROR = " + rez);
            _messageInviteObj.SetActive(true);
            _messageInvite.text = rez;
        }
        
        private void OnClickCloseButton()
        {
            _model?.closeCallback();
        }
    }
}