using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{

    public interface IAuthViewModel
    {
        string GetRealName();
        string GetIsletmeAdi();
        string GetPhotoUrl();

        //

        void SetRealName(string _realName);
        void SetIsletmeAdi(string _isletmeAdi);
        void SetPhotoUrl(string _photoUrl);
    }

    public class AuthViewModel : IAuthViewModel
    {
        private string RealName;
        private string IsletmeAdi;
        private string PhotoUrl;

        public AuthViewModel() {
            
        }

        public string GetIsletmeAdi()
        {
            return this.IsletmeAdi;
        }

        public string GetPhotoUrl()
        {
            return this.PhotoUrl;
        }

        public string GetRealName()
        {
            return this.RealName;
        }

        public void SetIsletmeAdi(string _isletmeAdi)
        {
            this.IsletmeAdi = _isletmeAdi;
        }

        public void SetPhotoUrl(string _photoUrl)
        {
            this.PhotoUrl = _photoUrl;
        }

        public void SetRealName(string _realName)
        {
            this.RealName = _realName;
        }
    }
}
