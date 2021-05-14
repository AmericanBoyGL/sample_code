using System;
using System.Collections.Generic;
using GeniusPoints;

namespace UI.Popups.DictionaryPopup
{
    public class DictionaryPopupModel:BasePopupModel
    {
        private List<UserWord> _words;
        public Action OnClickCloseButton;
        public List<UserWord> Words => _words;

        public DictionaryPopupModel()
        {
            _words = new List<UserWord>();
        }

        public void AddWord(UserWord word)
        {
            //_words.Add(word);
            _words.Insert(0, word);
        }

        public void ClearWords()
        {
            _words.Clear();
        }
    }
}