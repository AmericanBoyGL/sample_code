using System;
using Managers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace UI.Popups.DictionaryPopup
{
    public class DictionaryPopupView:BasePopupView
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _wordNameText;
        [SerializeField] private TMP_Text _countWordsText;
        [SerializeField] private Button _prevWordButton;
        [SerializeField] private Button _nextWordButton;
        
        [SerializeField] private RectTransform _scrollBarPos;
        
        [SerializeField] private Color _colorForActiveDot;
        [SerializeField] private Color _colorForNoActiveDot;
        [SerializeField] private List<GameObject> _dotsForPagination;
        [SerializeField] private List<GameObject> _dotsInGame;
        [SerializeField] private GameObject _dotForWordPref;
        [SerializeField] private Transform _contentForSpawn;
        
        private DictionaryPopupModel _model;
        private int index = 0;

        [Inject] private Client ClientGRPC { get; }
        
        private IDisposable _closeDisposable;
        private IDisposable _prevButtonDisposable;
        private IDisposable _nextButtonDisposable;


        public void Init(DictionaryPopupModel model)
        {
            _model = model;

            if (_closeDisposable == null)
            {
                _closeDisposable = closeButton.OnClickAsObservable().Subscribe(_ => OnClickCloseButton());
            }
            
            _nextWordButton.interactable = _model.Words.Count > 1;
            _prevWordButton.interactable = false;
            ShowWord(index);
            
            _prevButtonDisposable?.Dispose();
            _prevButtonDisposable = _prevWordButton.OnClickAsObservable().Subscribe(_=>OnClickPrevButton());

            _nextButtonDisposable?.Dispose();
            _nextButtonDisposable = _nextWordButton.OnClickAsObservable().Subscribe(_=>OnClickNextButton());

            SetPaginationForDictionary();
            
            
        }

        private void SetPaginationForDictionary()
        {
            _dotsForPagination = new List<GameObject>();
            _dotsInGame = new List<GameObject>();
            
            if (_model.Words.Count > 0)
            {
                if (_model.Words.Count > 8)
                {
                }
                
                foreach (var word in _model.Words)
                {
                    _dotsForPagination.Add(_dotForWordPref);
                }
                
                foreach (var dot in _dotsForPagination)
                {
                    var dotClon = Instantiate(dot, _contentForSpawn);
                    _dotsInGame.Add(dotClon);
                }

                _dotsInGame[0].GetComponent<Image>().color = _colorForActiveDot;
            }
        }

        private void ChangePaginationDotsNext(int index)
        {
            _dotsInGame[index].GetComponent<Image>().color = _colorForNoActiveDot;
            _dotsInGame[index + 1].GetComponent<Image>().color = _colorForActiveDot;
        }
        
        private void ChangePaginationDotsPrev(int index)
        {
            _dotsInGame[index].GetComponent<Image>().color = _colorForNoActiveDot;
            _dotsInGame[index - 1].GetComponent<Image>().color = _colorForActiveDot;
        }

        private void OnClickNextButton()
        {
            ChangePaginationDotsNext(index);
            index++;
            _nextWordButton.interactable = index < _model.Words.Count - 1;
            ShowWord(index);
            _prevWordButton.interactable = true;
        }

        private void OnClickPrevButton()
        {
            ChangePaginationDotsPrev(index);
            index--;
            _prevWordButton.interactable = index > 0;
            ShowWord(index);
            
            _nextWordButton.interactable = _model.Words.Count > 1;
        }

        private void ShowWord(int counter)
        {
            if (_model.Words.Count <= counter)
            {
                counter = _model.Words.Count - 1;
            }

            var word = _model.Words[counter];
            
            _titleText.text = word.Word.Word_.ToUpper();
            _wordNameText.text = word.Word.Word_;
            bool isDiscr = true;
            var no_discr = "If you see this, then the description has not been added yet, sorry ^_^";
            if (word.Word.Description == "No valid description" || word.Word.Description == null || word.Word.Description == "")
                isDiscr = false;
            
            //_descriptionText.text = isDiscr ? word.Word.Description : no_discr;

            _descriptionText.text = "";
            if (word.Word.Meta.Count > 0)
            {
                int i = 1;
                foreach (var meta in word.Word.Meta)
                {
                    _descriptionText.text = _descriptionText.text + i + ". (" + meta.PartOfSpeech + ") "+ meta.Text + "\n\n";
                    i++;
                }
            }
            
            _scrollBarPos.anchorMax = new Vector2(1f, 0f);
            
            _countWordsText.text = $"{index + 1} / {_model.Words.Count}";
        }

        private void ClearDotsPref()
        {
            foreach (var dot in _dotsInGame)
            {
                Destroy(dot);
            }
        }

        private void OnClickCloseButton()
        {
            _descriptionText.text = "";
            _wordNameText.text = "";
            _scrollBarPos.anchorMax = new Vector2(1f, 1f);
            index = 0;
            _model.ClearWords();
            ClearDotsPref();
            _model.OnClickCloseButton?.Invoke();
        }
    }
}