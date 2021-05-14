using System;
using AppsFlyerSDK;
using Balance;
using Data;
using Managers;
using Player;
using Scripts.UI.Popups.WaitingPopup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.Popups.CoinsJar
{
    public class CoinsJarController : BasePopupController
    {
        [SerializeField] private CoinsJarView view;

        [Inject] private PurchaserManager purchaserManager;
        [Inject] private PopupManager popupManager;
        [Inject] private Client client;
        [Inject] private PlayerController playerController;

        private void Start()
        {
            purchaserManager.OnFailedPurchase += FailedPurchase;
            purchaserManager.OnSuccessPurchase += OnSuccessPurchase;
        }

        private void OnDestroy()
        {
            purchaserManager.OnFailedPurchase -= FailedPurchase;
            purchaserManager.OnSuccessPurchase -= OnSuccessPurchase;
        }

        public override void Init(BasePopupModel baseModel)
        {
            view.Init(this);
        }

        public async void Collect()
        {
            purchaserManager.BuyItemFromID(PurchaserManager.PIGGY);
            await popupManager.ShowPopup(PopupNames.WAITING_POUP, new WaitingPopupModel()).ConfigureAwait(true);
        }

        public void Close()
        {
            popupManager.HidePopup(PopupName);
        }

        private void OnSuccessPurchase()
        {
            client.GetBalance(OnUpdatedBalance);
        }

        //RemoveHeart
        private void OnUpdatedBalance(ResponseUserBalanceDto data)
        {
            popupManager.HidePopup(PopupNames.WAITING_POUP);
            if (data != null)
            {
                Debug.Log("Purchase Success Update Balance " + data.BalanceHeart);
                playerController.SetGPValue(data.BalanceIP);
                playerController.SetCoinsValue(data.BalanceCoin);
                //PlayerController.SetHeartValue(data.BalanceHeart);

                playerController.SetBoosterTarget(data.BalanceBoosterTarget);
                playerController.SetBoosterWand(data.BalanceBoosterWand);
                playerController.SetBoosterLightBulb(data.BalanceBoosterLightBulb);

                playerController.SetRealMoneyValue(data.BalanceReal);
                playerController.SetJarCoinsValue(data.BalancePiggyBank);
            }
        }

        private void FailedPurchase()
        {
            popupManager.HidePopup(PopupNames.WAITING_POUP);
        }
    }
}
