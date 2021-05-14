using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Unibit.TournamentsModule;
using TMPro;
using UnityEngine;
using Zenject;
using Unibit.TournamentsModule.Data;
using UniRx;
using UnityEngine.UI;
using Player;
using Signals;

namespace UI.Popups.ReferralReward
{
    public class ReferralCompletePopupView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _collectButton;
        [SerializeField] private GameObject _collectBtn;
        [Inject] private Client ClientGRPC { get; }
        [Inject] private PlayerController PlayerController { get; }
        [Inject] private PopupManager PopupManager { get; }
        [Inject] private readonly SignalBus _signalBus;

        private ReferralCompletePopupModel _model;
        private IDisposable _closeDisposable;
        private IDisposable _collectDisposable;

        public void Initialize(ReferralCompletePopupModel model)
        {
            _model = model;
            _collectBtn.SetActive(true);
            _closeDisposable?.Dispose();
            _collectDisposable?.Dispose();
            _closeDisposable = _closeButton.OnClickAsObservable().Subscribe(_=> OnClickCloseButton());
            _collectDisposable = _collectButton.OnClickAsObservable().Subscribe(_=> OnCollectBtnClick());
        }

        private void OnCollectBtnClick()
        {
            ClientGRPC.SetPrizesForReferalls(OnConfirmPrize);
        }
        
        private void OnConfirmPrize(DailyReward.RefPrizes rez)
        {
            PlayerController.SetCoinsValue(PlayerController.GetCoinsValue() + rez.Prizes[0].Prize.Amount);
            PopupManager.HidePopup(PopupNames.REFERRAL_REWARD_POPUP);
            _signalBus.Fire<ReferralWinAnimationSignal>(new ReferralWinAnimationSignal(){ });
        }
        
        private void OnClickCloseButton()
        {
            _model?.closeCallback();
        }
    }
}