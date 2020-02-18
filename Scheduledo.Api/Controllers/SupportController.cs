//using System.Threading.Tasks;
//using Scheduledo.Core.Enums;
//using Scheduledo.Service.Abstract;
//using Scheduledo.Service.Models;
//using Scheduledo.Service.Infrastructure.Attributes;
//using Microsoft.AspNetCore.Mvc;

//namespace Scheduledo.Api.Controllers
//{
//    [AuthorizePolicy(UserRole.User)]
//    public class SupportController : BaseController
//    {
//        private readonly ISupportService _supportService;

//        public SupportController(ISupportService supportService)
//        {
//            _supportService = supportService;
//        }

//        [HttpPost("support/contact")]
//        [ProducesResponseType(200)]
//        public async Task<IActionResult> Contact([FromBody]ContactInput model)
//        {
//            model.UserId = CurrentUser.Id;

//            var result = await _supportService.Contact(model);
//            return GetResult(result);
//        }

//        [HttpPost("support/feature-request")]
//        [ProducesResponseType(200)]
//        public async Task<IActionResult> FeatureRequest([FromBody]FeatureRequestInput model)
//        {
//            model.UserId = CurrentUser.Id;

//            var result = await _supportService.FeatureRequest(model);
//            return GetResult(result);
//        }
//    }
//}
