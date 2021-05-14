using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using Player;
using TMPro;

namespace UI.Popups.CoinsJar
{
    public class CoinsJarView : MonoBehaviour
    {
        [SerializeField] private Button close;
        [SerializeField] private Button buy;
        [SerializeField] private TMP_Text _jarCoinsCount;
        [SerializeField] private Image jar;
        [SerializeField] private Sprite[] jarImages;

        private CoinsJarController controller;
        [Inject] private PlayerController PlayerController { get; }

        private void Awake()
        {
            buy.OnClickAsObservable().Subscribe(_ => Buy()).AddTo(this);
            close.OnClickAsObservable().Subscribe(_ => Close()).AddTo(this);
            
        }

        public void Init(CoinsJarController coinsJarController)
        {
            controller = coinsJarController;
            _jarCoinsCount.text = $"<sprite=2>{PlayerController.GetCoinsJar()}";
            UpdateIcon();
        }

        public void UnlockBuyButton()
        {
            buy.interactable = true;
        }

        private async void Buy()
        {
            buy.interactable = false;
            controller.Collect();
        } 

        private void UpdateIcon()
        {
            if (PlayerController.GetCoinsJar() == PlayerController.GetMaxJarValue)
                jar.sprite = jarImages[jarImages.Length - 1];
            else if (PlayerController.GetCoinsJar() >= PlayerController.GetMaxJarValue / 2)
                jar.sprite = jarImages[1];
            else
                jar.sprite = jarImages[0];
        }

        private void Close()
        {
            controller.Close();
        }
    }
}
