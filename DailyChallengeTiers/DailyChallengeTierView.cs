using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;
using Managers;
using GeniusPoints;
using System.Linq;
using Player;
using UI.Popups.WinLevelPopup;
using Crossword;
using AppsFlyerSDK;

namespace DailyChallenge
{
    public class DailyChallengeTierView : MonoBehaviour
    {
        [SerializeField] private GameObject[] graficToActive;

        [SerializeField] private TierView[] tiers; 
        [SerializeField] private Button next;
        [SerializeField] private Button backButton;

        private int maxPlayerLevel;
        [Inject] private PlayerController PlayerController { get; }
        [Inject] private Client client;

        public TierView[] Tiers => tiers;

        public async void Init(UserTier userTier)
        {
            TiersResponse tiersResponse = await client.GetTiersSettings();

            for (int i = 0; i < tiers.Length; i++)
            {
                Tier tier = tiersResponse.Entities[i];
                Debug.Log("tier = " + tier);
                if (tier.Id == userTier.Tier.Id)
                    tiers[i].Init(i, userTier.Trophies, tier.Trophies, tier.Reward.Amount, false, tiers);
                else if (tier.Id < userTier.Tier.Id)
                    tiers[i].Init(i, tier.Trophies, tier.Trophies, tier.Reward.Amount, false, tiers);
                else
                    tiers[i].Init(i, 0, tier.Trophies, tier.Reward.Amount, true, tiers);
            }
            
            //For local Testing
            //for (int i = 0; i < 4; i++)
            //{
            //    Tier tier = tiersResponse.Entities[i];
            //    if (i == 0)
            //        tiers[i].Init(i, 5, 5, 10, false, tiers);
            //    else if (i == 1)
            //        tiers[i].Init(i, 4, 5, 25, false, tiers);
            //    else
            //        tiers[i].Init(i, 0, 10, 30, true, tiers);
            //}

            TierView notFullTier = tiers.FirstOrDefault(t => !t.IsFull);

            if (notFullTier == null)
                next.gameObject.SetActive(true);

            next.OnClickAsObservable().Subscribe(_ => Next()).AddTo(this);
            backButton.OnClickAsObservable().Subscribe(_ => BackToMainMenu()).AddTo(this);
        }

        public void ActiveGrafic(bool active)
        {
            for (int i = 0; i < graficToActive.Length; i++)
                graficToActive[i].SetActive(active);
        }

        public void ShowNext()
        {
            next.gameObject.SetActive(true);
        }
        private void BackToMainMenu()
        {
            backButton.interactable = false;
            SceneManager.LoadSceneAsync(1);
        }

        private void Next()
        {
            next.interactable = false;
            maxPlayerLevel = PlayerController.GetMaxGameLevel();
            client.StartLevel(maxPlayerLevel, OnStartLevel);
        }

        private void OnStartLevel(ResponseStartLevelDto response)
        {
            PlayerController.SetMapLevel(maxPlayerLevel);
            SceneManager.LoadSceneAsync(2);

            var eventData = new Dictionary<string, string>();
            eventData.Add(AFInAppEvents.LEVEL, response.Level.ToString());
            AppsFlyer.sendEvent(AFInAppEvents.START_LEVEL, eventData);
        }
    }
}