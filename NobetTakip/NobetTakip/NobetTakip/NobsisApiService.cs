using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using NobetTakip.Core.Models;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using NobetTakip.ViewModel;
using NobetTakip.Core.DTO;

namespace NobetTakip
{
    public class NobsisApiService
    {
        public HttpClient Client { get; }

        public NobsisApiService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:5002/");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

            Client = client;
        }

        #region Personel
        public async Task<IEnumerable<Personel>> GetPersonels(Guid isletmeId)
        {
            var response = await Client.GetAsync("api/isletme/" + isletmeId.ToString() + "/personels");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Personel>>(responseStream);
        }

        public async Task<Personel> GetPersonel(Guid personelId)
        {
            var response = await Client.GetAsync("api/personel/" + personelId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Personel>(responseStream);
        }

        public async Task<Personel> GetPersonel(string mailAddress)
        {
            var response = await Client.GetAsync("api/personel/mail/" + mailAddress);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Personel>(responseStream);
        }

        public async Task<IEnumerable<Nobet>> GetPersonelNobets(Guid personelId)
        {
            var response = await Client.GetAsync("api/personel/" + personelId.ToString() + "/nobetler");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Nobet>>(responseStream);
        }

        public async Task<Personel> CreatePersonel(Personel personel)
        {

            var response = await Client.PostAsync("api/personel/", JsonContent(personel));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Personel>(responseStream);
        }

        public async Task<String> UpdatePersonel(Guid personelId, Personel personel)
        {
            
            var response = await Client.PutAsync("api/personel/" + personelId.ToString(), JsonContent(personel));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }

        public async Task<String> DeletePersonel(Guid personelId)
        {
            var response = await Client.DeleteAsync("api/personel/" + personelId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }

        #endregion

        #region Account
        public async Task<String> Register(Personel personel)
        {
            var response = await Client.PostAsync("api/account/register", JsonContent(personel));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }

        public async Task<Personel> Login(Personel personel)
        {
            var response = await Client.PostAsync("api/account/login", JsonContent(personel));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Personel>(responseStream);
        }
        #endregion

        #region Isletme
        public async Task<Isletme> GetIsletme(Guid isletmeId)
        {

            var response = await Client.GetAsync("api/isletme/" + isletmeId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Isletme>(responseStream);
        }

        public async Task<Isletme> GetIsletme(string isletmeKodu)
        {

            var response = await Client.GetAsync("api/isletme/code/" + isletmeKodu);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Isletme>(responseStream);
        }

        public async Task<Isletme> UpdateIsletme(Guid isletmeId, Isletme isletme)
        {
            var response = await Client.PutAsync("api/isletme/" + isletmeId.ToString(), JsonContent(isletme));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Isletme>(responseStream);
        }

        #endregion

        #region Nobet

        public async Task<IEnumerable<Nobet>> GetNobets(Guid isletmeId)
        {
            var response = await Client.GetAsync("api/isletme/" + isletmeId.ToString() + "/nobets");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Nobet>>(responseStream);
        }

        public async Task<Nobet> GetNobet(Guid nobetId)
        {
            var response = await Client.GetAsync("api/nobet/" + nobetId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Nobet>(responseStream);
        }

        public async Task<Nobet> CreateNobet(Nobet nobet)
        {
            var response = await Client.PostAsync("api/nobet/", JsonContent(nobet));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Nobet>(responseStream);
        }

        public async Task<String> UpdateNobet(Guid nobetId, Nobet nobet)
        {
            var response = await Client.PutAsync("api/nobet/" + nobetId.ToString(), JsonContent(nobet));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }

        public async Task<String> DeleteNobet(Guid nobetId)
        {
            var response = await Client.DeleteAsync("api/nobet/" + nobetId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }

        public async Task<IEnumerable<Nobet>> SearchNobet(NobetSearchModel nsm)
        {
            var response = await Client.PostAsync("api/nobet/search", JsonContent(nsm));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Nobet>>(responseStream);
        }


        public async Task<IEnumerable<Personel>> GetNobetPersonels(Guid nobetId)
        {
            var response = await Client.GetAsync("api/nobet/" + nobetId.ToString() + "/personels");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Personel>>(responseStream);
        }

        #endregion

        #region Bildirimler & Değişim İstekleri

        public async Task<IEnumerable<Bildirim>> GetBildirimler(Guid personelId)
        {
            var response = await Client.GetAsync("api/personel/" + personelId.ToString() + "/bildirimler");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Bildirim>>(responseStream);
        }

        public async Task<int> GetBildirimCount(Guid personelId)
        {
            var response = await Client.GetAsync("api/bildirim/count/" + personelId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(responseStream);
        }

        public async Task<Bildirim> CreateBildirim(Bildirim bildirim)
        {
            var response = await Client.PostAsync("api/bildirim", JsonContent(bildirim));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Bildirim>(responseStream);
        }

        public async Task<Bildirim> ChangeBildirimState(Guid degisimId)
        {
            var response = await Client.PutAsync("api/bildirim/" + degisimId.ToString(), null);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Bildirim>(responseStream);
        }

        public async Task<Degisim> GetDegisim(Guid degisimId)
        {
            var response = await Client.GetAsync("api/degisim/" + degisimId.ToString());
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Degisim>(responseStream);
        }

        public async Task<Degisim> CreateDegisim(Degisim degisim)
        {
            var response = await Client.PostAsync("api/degisim", JsonContent(degisim));
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Degisim>(responseStream);
        }

        public async Task<string> AcceptDegisim(Guid degisimId)
        {
            var response = await Client.PostAsync("api/degisim/" + degisimId.ToString(), null);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(responseStream);
        }

        #endregion

        private StringContent JsonContent(object model)
        {
            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }
    }
}
