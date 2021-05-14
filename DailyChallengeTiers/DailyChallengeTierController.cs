using GeniusPoints;
using Managers;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Zenject;

namespace DailyChallenge
{
    public class DailyChallengeTierController : MonoBehaviour
    {
        [SerializeField] private DailyChallengeTierView view;
        private LevelData levelData;
        
        [Inject] private Client client;
        [Inject] private PopupManager popupManager;
        [Inject] private DailyChallengeCrosswordManager crosswordManager;
        
        private bool activeGrafic = false;
        public bool IsGraficActive => activeGrafic;
        public TierView[] Tiers => view.Tiers;

        private void Awake()
        {
            crosswordManager.OnCompleteInitLevel += OnLevelInited;
        }
        
        private void OnDestroy()
        {
            crosswordManager.OnCompleteInitLevel -= OnLevelInited;
        }
        
        private void Start()
        {
            
        }

        private void OnLevelInited()
        {
            levelData = crosswordManager.CurrentLevelData;
            client.GetUserTiers(GetUserTier);
        }

        public void ShowNext()
        {
            view.ShowNext();
        }

        public void ActiveGrafics(bool active)
        {
            activeGrafic = active;
            view.ActiveGrafic(active);
        }

        private async void GetUserTier(UserTier userTier)
        {
            view.Init(userTier);
            var countOpenWords = 0;
            foreach (var word in levelData.words)
            {
                if(word.IsOpened)
                    countOpenWords++;
            }
            
            Debug.Log("IsAvailable = " + userTier.IsAvailable);
            Debug.Log("levelData = " + levelData.CheckCompleteOtherWords());
            
            if (!userTier.IsAvailable)
            {
                if (countOpenWords == 0)
                {
                    activeGrafic = true;
                    view.ActiveGrafic(true);
                    await popupManager.ShowPopup(UI.PopupNames.DAILY_PUZZELES_COME_BACK_LATER, null);
                }
            }
        }
    }
}
