using System;
using GeniusPoints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;
using Zenject;
using Signals;
using DailyChallenge.DailyChallengeProgress;
using DG.Tweening;
using System.Threading.Tasks;
using Game.TopPanel.MoneyPanel.CoinsPanel;
using Managers;
using UI.Popups.WinLevelPopup;

namespace DailyChallenge
{
    public class TierView : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image tierImage;
        [SerializeField] private TextMeshProUGUI winAmountClosedText;
        [SerializeField] private TextMeshProUGUI winAmountOpenedText;
        [SerializeField] private TextMeshProUGUI trophiesAmountText;
        [SerializeField] private Sprite closedImage;
        [SerializeField] private Sprite openedTierImage;
        [SerializeField] private Transform moveTo;
        [SerializeField] private BonusWords _bonusWords;
        [SerializeField] private GameObject _posStartAnimCoins;
        
        [Inject] private PlayerController playerController;
        [Inject] private SignalBus signalBus;
        [Inject] private Client client;
        
        private int currTrophyAmount;
        private int trophyAmount;
        private int winAmount;
        private bool closed;
        private TierView[] tiers;
        private int index;

        public bool IsFull => currTrophyAmount == trophyAmount && !closed;
        public Transform MoveTo => moveTo;

        public void Init(int _index,int _currTorphyAmount, int _torphyAmount, int _winAmount, bool _closed, TierView[] _tiers)
        {
            //_coinsPanel.Init(playerController.GetCoinsValue());
            closed = _closed;
            currTrophyAmount = _currTorphyAmount;
            trophyAmount = _torphyAmount;
            winAmount = _winAmount;
            tiers = _tiers;
            index = _index;

            if (_closed)
            {
                tierImage.sprite = closedImage;
                winAmountClosedText.gameObject.SetActive(true);
                winAmountClosedText.text = "<sprite=2> +" + winAmount;
                slider.gameObject.SetActive(false);
                trophiesAmountText.gameObject.SetActive(false);
            }
            else
            {
                tierImage.sprite = openedTierImage;
                winAmountOpenedText.gameObject.SetActive(true);
                winAmountOpenedText.text = "+" + winAmount + " <sprite=2>";
                SetSliderValues();
            }
        }

        public void OpenTier()
        {
            Debug.Log("OPEN TIER " + currTrophyAmount + " max " + trophyAmount);
            closed = false;
            tierImage.sprite = openedTierImage;
            winAmountClosedText.gameObject.SetActive(false);
            slider.gameObject.SetActive(true);
            winAmountOpenedText.gameObject.SetActive(true);
            trophiesAmountText.gameObject.SetActive(true);
            winAmountOpenedText.text = "+" + winAmount + " <sprite=2>";
            SetSliderValues();
        }

        public async Task AddTorphy(Trophy trophy)
        {
            trophy.transform.DOMove(MoveTo.position, 1.5f);

            if (!trophy.IsSaved)
                currTrophyAmount++;

            await Task.Delay(1500);
            SetSliderValues();
            if (trophyAmount == currTrophyAmount)
            {
                signalBus.Fire<GpFlyAnimationSignal>(new GpFlyAnimationSignal()
                {
                    StartPositionInWorldCoordinate = default,
                    CountBrains = winAmount
                });
                //playerController.SetGPValue(playerController.GetGPValue() + winAmount);
                //Debug.Log("win coisn = " + winAmount);
                //playerController.ChangeCoins(winAmount, TypeBalance.PER_LEVEL);
                //_coinsPanel.AddCoins(winAmount);
                Debug.Log("WIN = " + winAmount);
                _bonusWords.Init(winAmount, false);
                _bonusWords.StartAnimationDaily(_posStartAnimCoins.transform.position);
                
                int nextTierIndex = index + 1;
                if(nextTierIndex < tiers.Length)
                {
                    tiers[nextTierIndex].OpenTier(); 
                }
            }
        }

        private void SetSliderValues()
        {
            trophiesAmountText.text = currTrophyAmount.ToString() + "/" + trophyAmount.ToString();
            slider.maxValue = trophyAmount;
            slider.value = currTrophyAmount;
        }
    }
}
