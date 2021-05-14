using System;
using Configs.FlagsOnMap.Data;
using Player;

namespace UI.Popups.InviteFriendPopup
{
    public class InviteFriendPopupModel:BasePopupModel
    {
        public CountryData countryData;
        public Action<String> confirmCallback;
        public Action closeCallback;
        public float courseGpToCash;
    }
}