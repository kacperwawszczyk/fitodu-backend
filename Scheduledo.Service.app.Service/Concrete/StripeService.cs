//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Scheduledo.Core.Enums;
//using Scheduledo.Core.Extensions;
//using Scheduledo.Model;
//using Scheduledo.Model.Entities;
//using Scheduledo.Resource;
//using Scheduledo.Service.Abstract;
//using Scheduledo.Service.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Stripe;
//using Stripe.Checkout;

//namespace Scheduledo.Service.Concrete
//{
//    public class StripeService : IBillingService
//    {
//        private readonly Context _context;
//        private readonly string _secret;
//        private readonly string _clientUrl;
//        private readonly ILogger<StripeService> _logger;

//        public StripeService(IConfiguration configuration, Context context, ILogger<StripeService> logger)
//        {
//            StripeConfiguration.ApiKey = configuration["Stripe:ApiKey"];
//            _secret = configuration["Stripe:Secret"];
//            _clientUrl = configuration["Environment:ClientUrl"];

//            _context = context;
//            _logger = logger;
//        }

//        public async Task<Result> WebhookEvent(string body, string signature)
//        {
//            var result = new Result();

//            try
//            {
//                var stripeEvent = EventUtility.ConstructEvent(body, signature, _secret);

//                // https://stripe.com/docs/api/events/types
//                switch (stripeEvent.Type)
//                {
//                    case Events.CheckoutSessionCompleted:
//                        var session = stripeEvent.Data.Object as Session;

//                        using (var transaction = _context.Database.BeginTransaction())
//                        {
//                            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == session.ClientReferenceId);
//                            if (user == null)
//                            {
//                                _logger.LogCritical("Can't find ClientReferenceId", session.ClientReferenceId);
//                                result.Error = ErrorType.NotFound;
//                                return result;
//                            }

//                            if (session.Mode == "setup")
//                            {
//                                var setupService = new SetupIntentService();
//                                var setupIntent = await setupService.GetAsync(session.SetupIntentId);

//                                var paymentService = new PaymentMethodService();
//                                await paymentService.AttachAsync(
//                                    setupIntent.PaymentMethodId,
//                                    new PaymentMethodAttachOptions { Customer = user.Company.BillingCustomerId });

//                                var subscriptionService = new SubscriptionService();
//                                await subscriptionService.UpdateAsync(
//                                    user.Company.BillingSubscriptionId,
//                                    new SubscriptionUpdateOptions { DefaultPaymentMethod = setupIntent.PaymentMethodId });

//                                return result;
//                            }

//                            var plan = session.DisplayItems[0].Plan;

//                            user.Company.Plan = Enums.GetEnum<PricingPlan>(plan.Nickname.Replace("PL", string.Empty));
//                            user.Company.PlanExpiredOn = null;
//                            user.Company.BillingSubscriptionId = session.SubscriptionId;
//                            user.Company.BillingCustomerId = session.CustomerId;

//                            _context.Companies.Update(user.Company);

//                            if (await _context.SaveChangesAsync() == 0)
//                            {
//                                transaction.Rollback();
//                                result.Error = ErrorType.InternalServerError;
//                                return result;
//                            }

//                            transaction.Commit();
//                        }
//                        break;
//                    case Events.CustomerSubscriptionUpdated:
//                        var subscription = stripeEvent.Data.Object as Subscription;

//                        using (var transaction = _context.Database.BeginTransaction())
//                        {
//                            var company = await _context.Companies.FirstOrDefaultAsync(x => x.BillingSubscriptionId == subscription.Id);
//                            if (company == null)
//                            {
//                                _logger.LogCritical("Can't find SubscriptionId", subscription);
//                                return result;
//                            }

//                            company.Plan = Enums.GetEnum<PricingPlan>(subscription.Plan.Nickname.Replace("PL", string.Empty));
//                            company.PlanExpiredOn = null;

//                            _context.Companies.Update(company);

//                            if (await _context.SaveChangesAsync() == 0)
//                            {
//                                transaction.Rollback();
//                                result.Error = ErrorType.InternalServerError;
//                                return result;
//                            }

//                            transaction.Commit();
//                        }
//                        break;
//                    case Events.CustomerSubscriptionDeleted:
//                        subscription = stripeEvent.Data.Object as Subscription;

//                        using (var transaction = _context.Database.BeginTransaction())
//                        {
//                            var company = await _context.Companies.FirstOrDefaultAsync(x => x.BillingSubscriptionId == subscription.Id);
//                            if (company == null)
//                            {
//                                _logger.LogCritical("Can't find SubscriptionId", subscription);
//                                result.Error = ErrorType.NotFound;
//                                return result;
//                            }

//                            company.BillingSubscriptionId = null;
//                            company.PlanExpiredOn = subscription.CurrentPeriodEnd.Value.AddDays(-1);

//                            _context.Companies.Update(company);

//                            if (await _context.SaveChangesAsync() == 0)
//                            {
//                                transaction.Rollback();
//                                result.Error = ErrorType.InternalServerError;
//                                return result;
//                            }

//                            transaction.Commit();
//                        }
//                        break;
//                }
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogCritical(ex, "Can't handle webhook event");
//                result.Error = ErrorType.BadRequest;
//            }

//            return result;
//        }

//        public async Task<Result<string>> Subscribe(string userId, PricingPlan plan)
//        {
//            var result = new Result<string>();

//            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
//            if (user == null)
//            {
//                result.Error = ErrorType.NotFound;
//                return result;
//            }

//            try
//            {
//                if (user.Company.BillingCustomerId == null)
//                {
//                    var customer = await CreateCustomer(user);
//                    user.Company.BillingCustomerId = customer.Id;
//                }
//                else
//                {
//                    await UpdateCustomer(user);
//                }

//                var options = new SessionCreateOptions
//                {
//                    PaymentMethodTypes = new List<string> { "card" },
//                    SubscriptionData = new SessionSubscriptionDataOptions
//                    {
//                        Items = new List<SessionSubscriptionDataItemOptions>
//                        {
//                            new SessionSubscriptionDataItemOptions
//                            {
//                                Plan = Internal.ResourceManager.GetString(plan.ToString()),
//                                Quantity = 1
//                            }
//                        }
//                    },
//                    Customer = user.Company.BillingCustomerId,
//                    ClientReferenceId = userId,
//                    SuccessUrl = _clientUrl,
//                    CancelUrl = _clientUrl + "/#/billing"
//                };

//                var service = new SessionService();
//                var session = await service.CreateAsync(options);

//                result.Data = session.Id;
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogCritical(ex, "Can't create subscribe session", user.Company.BillingCustomerId);
//                result.Error = ErrorType.BadRequest;
//            }

//            return result;
//        }

//        public async Task<Result> Change(string userId, PricingPlan plan)
//        {
//            var result = new Result();

//            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
//            if (user == null)
//            {
//                result.Error = ErrorType.NotFound;
//                return result;
//            }

//            try
//            {
//                var service = new SubscriptionService();
//                var subscription = service.Get(user.Company.BillingSubscriptionId);

//                var items = new List<SubscriptionItemUpdateOption>
//                {
//                    new SubscriptionItemUpdateOption
//                    {
//                        Id = subscription.Items.Data[0].Id,
//                        Plan = Internal.ResourceManager.GetString(plan.ToString())
//                    }
//                };

//                var options = new SubscriptionUpdateOptions
//                {
//                    CancelAtPeriodEnd = false,
//                    Items = items,
//                };

//                await service.UpdateAsync(subscription.Id, options);
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogCritical(ex, "Can't update subscription", user.Company.BillingSubscriptionId);
//                result.Error = ErrorType.BadRequest;
//            }

//            return result;
//        }

//        public async Task<Result<string>> Update(string userId)
//        {
//            var result = new Result<string>();

//            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
//            if (user == null)
//            {
//                result.Error = ErrorType.NotFound;
//                return result;
//            }

//            try
//            {
//                var options = new SessionCreateOptions
//                {
//                    PaymentMethodTypes = new List<string> { "card" },
//                    SetupIntentData = new SessionSetupIntentDataOptions
//                    {
//                        Metadata = new Dictionary<string, string>
//                        {
//                            {"customer_id", user.Company.BillingCustomerId},
//                            {"subscription_id", user.Company.BillingSubscriptionId}
//                        }
//                    },
//                    CustomerEmail = user.Email,
//                    ClientReferenceId = userId,
//                    Mode = "setup",
//                    SuccessUrl = _clientUrl,
//                    CancelUrl = _clientUrl + "/#/billing"
//                };

//                var service = new SessionService();
//                var session = await service.CreateAsync(options);

//                result.Data = session.Id;
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogCritical(ex, "Can't create update session", user.Company.BillingCustomerId);
//                result.Error = ErrorType.BadRequest;
//            }

//            return result;
//        }

//        public async Task<Result> Unsubscribe(string userId)
//        {
//            var result = new Result();

//            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
//            if (user == null)
//            {
//                result.Error = ErrorType.NotFound;
//                return result;
//            }

//            try
//            {
//                var service = new SubscriptionService();
//                var cancelOptions = new SubscriptionCancelOptions
//                {
//                    InvoiceNow = false,
//                    Prorate = false,
//                };

//                await service.CancelAsync(user.Company.BillingSubscriptionId, cancelOptions);
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogCritical(ex, "Can't cancel subscription", user.Company.BillingSubscriptionId);
//                result.Error = ErrorType.BadRequest;
//            }

//            return result;
//        }

//        public async Task UpdateCustomer(User admin)
//        {
//            var company = admin.Company;

//            try
//            {
//                var options = new CustomerUpdateOptions
//                {
//                    Name = company.Name ?? admin.FullName,
//                    Address = new AddressOptions
//                    {
//                        Line1 = company.AddressLine1,
//                        Line2 = company.AddressLine2,
//                        PostalCode = company.AddressPostalCode,
//                        City = company.AddressCity,
//                        State = company.AddressState,
//                        Country = company.AddressCountry
//                    },
//                    Phone = admin.PhoneNumber,
//                    Email = admin.Email
//                };

//                if (!string.IsNullOrEmpty(company.VatIn))
//                {
//                    options.InvoiceSettings = new CustomerInvoiceSettingsOptions
//                    {
//                        CustomFields = new List<InvoiceCustomFieldOptions>
//                    {
//                        new InvoiceCustomFieldOptions
//                        {
//                            Name = "VATIN",
//                            Value = company.VatIn
//                        }
//                    }
//                    };
//                }

//                var service = new CustomerService();
//                await service.UpdateAsync(company.BillingCustomerId, options);
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogCritical(ex, "Can't update customer", company.BillingCustomerId);
//            }
//        }

//        private async Task<Customer> CreateCustomer(User admin)
//        {
//            var company = admin.Company;
//            var options = new CustomerCreateOptions
//            {
//                Name = company.Name ?? admin.FullName,
//                Address = new AddressOptions
//                {
//                    Line1 = company.AddressLine1,
//                    Line2 = company.AddressLine2,
//                    PostalCode = company.AddressPostalCode,
//                    City = company.AddressCity,
//                    State = company.AddressState,
//                    Country = company.AddressCountry
//                },
//                Phone = admin.PhoneNumber,
//                Email = admin.Email
//            };

//            if (!string.IsNullOrEmpty(company.VatIn))
//            {
//                options.InvoiceSettings = new CustomerInvoiceSettingsOptions
//                {
//                    CustomFields = new List<InvoiceCustomFieldOptions>
//                    {
//                        new InvoiceCustomFieldOptions
//                        {
//                            Name = "VATIN",
//                            Value = company.VatIn
//                        }
//                    }
//                };
//            }

//            var service = new CustomerService();
//            return await service.CreateAsync(options);
//        }
//    }
//}
