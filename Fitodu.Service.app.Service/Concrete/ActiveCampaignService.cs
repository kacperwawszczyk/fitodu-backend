using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fitodu.Service.Concrete
{
    public class ActiveCampaignService : IEmailMarketingService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _mainListId;

        public ActiveCampaignService(IConfiguration configuration)
        {
            _apiKey = configuration["ActiveCampaign:ApiKey"];
            _apiUrl = configuration["ActiveCampaign:ApiUrl"];
            _mainListId = configuration["ActiveCampaign:MainListId"];
        }

        public async Task<Result<int>> AddOrUpdate(User user)
        {
            var result = new Result<int>();

            var name = user.FullName?.Split(" ");
            var contactModel = new
            {
                contact = new
                {
                    email = user.Email,
                    firstName = name?.Length == 2 ? name[0] : user.FullName,
                    lastName = name?.Length == 2 ? name[1] : string.Empty,
                    phone = user.PhoneNumber
                }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(contactModel), Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Api-Token", _apiKey);

            HttpResponseMessage response;

            if (user.SubscriberId == default(int))
                response = await httpClient.PostAsync(_apiUrl + "/contacts/sync", content);
            else
                response = await httpClient.PutAsync($"{_apiUrl}/contacts/{user.SubscriberId}", content);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            result.Data = JObject.Parse(responseContent)["contact"]["id"].Value<int>();
            return result;
        }

        public async Task<Result<int>> Subscribe(User user)
        {
            var result = new Result<int>();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Api-Token", _apiKey);

            var response = await httpClient.GetAsync(_apiUrl + $"/contacts?email={user.Email}");
            var responseContent = await response.Content.ReadAsStringAsync();

            var contacts = JObject.Parse(responseContent)["contacts"].Values<JObject>();

            int contactId;
            if (contacts.Count() == 0)
            {
                var addResult = await AddOrUpdate(user);
                if (!result.Success)
                    return addResult;

                contactId = addResult.Data;
            }
            else
            {
                contactId = contacts.First()["id"].Value<int>();
            }

            var model = new
            {
                contactList = new
                {
                    list = _mainListId,
                    contact = contactId,
                    status = 1
                }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(_apiUrl + "/contactLists", content);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                result.Error = ErrorType.BadRequest;

            result.Data = contactId;
            return result;
        }

        public async Task<bool> Unsubscribe(int contactId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Api-Token", _apiKey);

            var model = new
            {
                contactList = new
                {
                    list = _mainListId,
                    contact = contactId,
                    status = 2
                }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(_apiUrl + "/contactLists", content);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}