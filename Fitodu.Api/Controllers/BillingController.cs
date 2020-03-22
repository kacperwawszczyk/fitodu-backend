//using Microsoft.AspNetCore.Mvc;
//using Fitodu.Service.Abstract;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using System.IO;
//using Fitodu.Core.Enums;
//using Fitodu.Service.Infrastructure.Attributes;
//using Fitodu.Service.Models;

//namespace Fitodu.Api.Controllers
//{
//    [AuthorizePolicy(UserRole.User)]
//    public class BillingController : BaseController
//    {
//        private readonly IBillingService _billingService;

//        public BillingController(IBillingService billingService)
//        {
//            _billingService = billingService;
//        }

//        // https://dashboard.stripe.com/webhooks
//        [AllowAnonymous]
//        [HttpPost("billing/webhook")]
//        public async Task<IActionResult> Webhook()
//        {
//            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

//            var result = await _billingService.WebhookEvent(
//                json, Request.Headers["Stripe-Signature"]);

//            return GetResult(result);
//        }

//        [HttpPost("billing/subscribe")]
//        [ProducesResponseType(typeof(string), 200)]
//        public async Task<IActionResult> Subscribe([FromBody]SubscriptionInput model)
//        {
//            var result = await _billingService.Subscribe(CurrentUser.Id, model.Plan);
//            return GetResult(result);
//        }

//        [HttpPut("billing/change")]
//        public async Task<IActionResult> Change([FromBody]SubscriptionInput model)
//        {
//            var result = await _billingService.Change(CurrentUser.Id, model.Plan);
//            return GetResult(result);
//        }

//        [HttpPut("billing/update")]
//        [ProducesResponseType(typeof(string), 200)]
//        public async Task<IActionResult> Update()
//        {
//            var result = await _billingService.Update(CurrentUser.Id);
//            return GetResult(result);
//        }

//        [HttpDelete("billing/unsubscribe")]
//        public async Task<IActionResult> Cancel()
//        {
//            var result = await _billingService.Unsubscribe(CurrentUser.Id);
//            return GetResult(result);
//        }
//    }
//}
