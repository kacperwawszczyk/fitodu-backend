using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Model.Extensions;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Fitodu.Service.Attributes;
using System.Drawing.Drawing2D;

namespace Fitodu.Service.Concrete
{
    public class CoachService : ICoachService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly UserManager<User> _userManager;
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IEmailMarketingService _emailMarketingService;
        private string azureConnectionString;
        //private readonly IEmailService _emailService;
        //private readonly SignInManager<User> _signInManager;
        //private readonly IBillingService _billingService;

        public CoachService(
            ILogger<UserService> logger,
            IConfiguration configuration,
            IDateTimeService dateTimeService,
            UserManager<User> userManager,
            Context context,
            IMapper mapper,
            IEmailMarketingService emailMarketingService)
        {
            _logger = logger;
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _emailMarketingService = emailMarketingService;
            azureConnectionString = _configuration.GetConnectionString("StorageConnection");
        }

        public async Task<Result> CoachRegister(RegisterCoachInput model)
        {
            var result = new Result();

            var existingUser = await _userManager.FindByNameAsync(model.Email);
            if (existingUser != null)
            {
                result.ErrorMessage = Resource.Validation.EmailTaken;
                result.Error = ErrorType.BadRequest;
                return result;
            }

            //var existingCompany = await _context.Companies.FirstOrDefaultAsync(x => x.Url == model.Url);
            //if (existingCompany != null)
            //{
            //    result.ErrorMessage = Resource.Validation.UrlTaken;
            //    result.Error = ErrorType.BadRequest;
            //    return result;
            //}

            var user = new User();
            user.Role = UserRole.Coach;
            user.FullName = model.FullName;
            user.Email = model.Email;
            //user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;
            user.Id = Guid.NewGuid().ToString();
            user.Company = new Company()
            {
                //Url = model.Url,
                Plan = PricingPlan.Trial,
                PlanExpiredOn = _dateTimeService.Now().AddDays(30)
            };

            var coach = new Coach();
            coach.Id = user.Id;
            coach.Name = model.Name;
            coach.Surname = model.Surname;

            var weekPlan = new WeekPlan();
            weekPlan.IsDefault = true;
            weekPlan.IdCoach = coach.Id;
            weekPlan.StartDate = null;
            weekPlan.DayPlans = null;

            user.SetCredentials(model.Password);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(coach.Id);
                blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(coach.Id);
                blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                //var subscribeResult = await _emailMarketingService.Subscribe(user);
                //if (subscribeResult.Success)
                //    user.SubscriberId = subscribeResult.Data;
                //else
                //    _logger.LogCritical("Can't subscribe to email marketing", user.UserName);

                var addToRoleResult = await _userManager.AddToRoleAsync(user, user.Role.GetName());
                if (!addToRoleResult.Succeeded)
                {
                    transaction.Rollback();

                    var unsubscribeResult = await _emailMarketingService.Unsubscribe(user.SubscriberId);
                    if (!unsubscribeResult)
                        _logger.LogCritical("Can't unsubscribe from email marketing", user.UserName);

                    result.Error = ErrorType.InternalServerError;
                    return result;
                }


                var createCoachResult = await _context.Coaches.AddAsync(coach);

                if (createCoachResult.State != EntityState.Added)
                {
                    transaction.Rollback();
                    result.ErrorMessage = "Couldn't add coach to the database";
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }


                var addWeekPlanResult = await _context.WeekPlans.AddAsync(weekPlan);

                if (addWeekPlanResult.State != EntityState.Added)
                {
                    transaction.Rollback();
                    result.ErrorMessage = "Couldn't add coachs weekplan to the database";
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                _context.SaveChanges();
                //if (await _context.SaveChangesAsync() == 0)
                //{
                //    transaction.Rollback();
                //    result.ErrorMessage = "Couldn't save changes to the database";
                //    result.Error = ErrorType.InternalServerError;
                //    return result;
                //}
                //else
                //{
                transaction.Commit();
                //}
            }

            return result;
        }

        //public async Task<Result<ICollection<CoachOutput>>> GetAllCoaches()
        //{
        //    var result = new Result<ICollection<CoachOutput>>();
        //    var coaches = await _context.Coaches.ToListAsync();
        //    if (coaches == null)
        //    {
        //        result.Error = ErrorType.NoContent;
        //        result.ErrorMessage = "No coaches found";
        //        return result;
        //    }
        //    var coachesResult = new List<CoachOutput>();
        //    foreach (Coach c in coaches)
        //    {
        //        CoachOutput nc = new CoachOutput();
        //        nc.Id = c.Id;
        //        nc.AddressCity = c.AddressCity;
        //        nc.AddressCountry = c.AddressCountry;
        //        nc.AddressLine1 = c.AddressLine1;
        //        nc.AddressLine2 = c.AddressLine2;
        //        nc.AddressPostalCode = c.AddressPostalCode;
        //        nc.AddressState = c.AddressState;
        //        nc.Name = c.Name;
        //        nc.Rules = c.Rules;
        //        nc.Surname = c.Surname;
        //        nc.CancelTimeHours = c.CancelTimeHours;
        //        nc.CancelTimeMinutes = c.CancelTimeMinutes;
        //        coachesResult.Add(nc);
        //    }
        //    result.Data = coachesResult;
        //    return result;
        //}

        public async Task<Result<CoachOutput>> GetCoach(string Id)
        {
            var result = new Result<CoachOutput>();
            CoachOutput coach = await _context.Coaches
                .Select(nc => new CoachOutput
                {
                    Id = nc.Id,
                    Name = nc.Name,
                    Surname = nc.Surname,
                    Rules = nc.Rules,
                    AddressLine1 = nc.AddressLine1,
                    AddressLine2 = nc.AddressLine2,
                    AddressPostalCode = nc.AddressPostalCode,
                    AddressCity = nc.AddressCity,
                    AddressState = nc.AddressState,
                    AddressCountry = nc.AddressCountry,
                    CancelTimeHours = nc.CancelTimeHours,
                    CancelTimeMinutes = nc.CancelTimeMinutes
                })
                .FirstOrDefaultAsync(x => x.Id == Id);

            User coachAcc = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (coachAcc != null)
            {
                coach.PhoneNumber = coachAcc.PhoneNumber;
                coach.Email = coachAcc.Email;
                coach.Avatar = coachAcc.Avatar;
            }


            if (coach == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Coach with given id does not exist";
                return result;
            }
            else
            {
                //BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(coach.Id);
                //if (await blobContainerClient.ExistsAsync() == false)
                //{
                //    coach.Avatar = null;
                //}
                //else
                //{
                //    BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                //    if (await blobClient.ExistsAsync() == false)
                //    {
                //        coach.Avatar = null;
                //    }
                //    else
                //    {
                //        coach.Avatar = blobClient.Uri.AbsoluteUri;
                //    }
                //}
                result.Data = coach;
                return result;
            }
        }

        public async Task<Result<long>> UpdateCoach(string Id, UpdateCoachInput coachNew)
        {
            var result = new Result<long>();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            var _tmpCoach = coach;
            User coachAcc = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            coach.Name = coachNew.Name;
            coach.Surname = coachNew.Surname;
            if (coachNew.CancelTimeMinutes > 59 || coachNew.CancelTimeHours > 23)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Cancel time must be between 0 and 23:59";
                return result;
            }
            coach.CancelTimeHours = coachNew.CancelTimeHours;
            coach.CancelTimeMinutes = coachNew.CancelTimeMinutes;
            coach.AddressCity = coachNew.AddressCity;
            coach.AddressCountry = coachNew.AddressCountry;
            coach.AddressPostalCode = coachNew.AddressPostalCode;
            coach.AddressState = coachNew.AddressState;
            coach.Rules = coachNew.Rules;
            coach.AddressLine1 = coachNew.AddressLine1;
            coach.AddressLine2 = coachNew.AddressLine2;
            coach.UpdatedOn = DateTime.UtcNow;

            coachAcc.PhoneNumber = coachNew.PhoneNumber;

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Coaches.Update(coach);
                _context.Users.Update(coachAcc);

                if (await _context.SaveChangesAsync() == 0)
                {
                    if (coach.Equals(_tmpCoach))
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.Forbidden;
                        result.ErrorMessage = "Couldn't save changes to the database";
                        return result;
                    }
                }
                else
                {
                    transaction.Commit();
                    return result;
                }
            }
        }

        public async Task<Result<ICollection<ClientOutput>>> GetAllClients(string Id)
        {
            var result = new Result<ICollection<ClientOutput>>();
            Coach coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            if (coach == null)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No coach found";
                return result;
            }
            List<CoachClient> coachClients = await _context.CoachClients
                .Where(x => x.IdCoach == Id)
                .ToListAsync();
            if (coachClients.Count == 0)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No clients found";
                return result;
            }
            List<ClientOutput> clients = new List<ClientOutput>();
            foreach (var y in coachClients)
            {
                ClientOutput client = await _context.Clients
                .Where(x => x.Id == y.IdClient)
                .Select(x => new ClientOutput
                {
                    Id = x.Id,
                    AddressCity = x.AddressCity,
                    AddressCountry = x.AddressCountry,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    AddressPostalCode = x.AddressPostalCode,
                    AddressState = x.AddressState,
                    FatPercentage = x.FatPercentage,
                    Height = x.Height,
                    Name = x.Name,
                    Surname = x.Surname,
                    Weight = x.Weight,
                    IsRegistered = x.IsRegistered,
                    AvailableTrainings = y.AvailableTrainings,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefaultAsync();
                //BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(client.Id);
                //if (await blobContainerClient.ExistsAsync() == false)
                //{
                //    client.Avatar = null;
                //}
                //else
                //{
                //    BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                //    if (await blobClient.ExistsAsync() == false)
                //    {
                //        client.Avatar = null;
                //    }
                //    else
                //    {
                //        client.Avatar = blobClient.Uri.AbsoluteUri;
                //    }
                //}
                User clientAcc = await _context.Users.FirstOrDefaultAsync(x => x.Id == client.Id);
                if (clientAcc != null)
                {
                    client.PhoneNumber = clientAcc.PhoneNumber;
                    client.Email = clientAcc.Email;
                    client.Avatar = clientAcc.Avatar;
                }

                clients.Add(client);
            }
            result.Data = clients;
            return result;
        }

        public async Task<Result> SetClientsTrainingsAvailable(string requesterId, UserRole role, string clientId, int value)
        {
            var result = new Result();
            using (var transcation = _context.Database.BeginTransaction())
            {
                var coachClient = await _context.CoachClients.Where(x => x.IdCoach == requesterId && x.IdClient == clientId).FirstOrDefaultAsync();

                if (role != UserRole.Coach || coachClient == null || value < 0)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "Invalid value of available trainings or user is not a coach or this user is not client of this coach.";
                    return result;
                }

                coachClient.AvailableTrainings = value;

                if (await _context.SaveChangesAsync() == 0)
                {
                    transcation.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transcation.Commit();
            }
            return result;
        }

        //public async Task<Result<string>> UpdateAvatar(string id, UserRole role, IFormFile file)
        //{
        //    var result = new Result<string>();

        //    if (role != UserRole.Coach)
        //    {
        //        result.Error = ErrorType.Forbidden;
        //        result.ErrorMessage = "User is not a coach";
        //        return result;
        //    }
        //    else
        //    {
        //        var client = _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

        //        if (client == null)
        //        {
        //            result.Error = ErrorType.BadRequest;
        //            result.ErrorMessage = "User does not exist";
        //            return result;
        //        }

        //        BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
        //        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(id);

        //        if (await blobContainerClient.ExistsAsync() == false)
        //        {
        //            blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(id);
        //            blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        //        }

        //        if (CheckIfImageFile(file))
        //        {
        //            Image image = Image.FromStream(file.OpenReadStream(), true, true);
        //            //if (image.Width < 128 || image.Width > 1024 || image.Height < 128 || image.Height > 1024)
        //            //{
        //            //    result.Error = ErrorType.Forbidden;
        //            //    result.ErrorMessage = "Image has to be between 128x128 and 1024x1024";
        //            //    return result;
        //            //}

        //            //Bitmap newImage = ResizeImage(image, 150, 150);
        //            Image newImage = ResizeImage(image, 150, 150);
        //            BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
        //            MemoryStream msImage = new MemoryStream();
        //            newImage.Save(msImage, ImageFormat.Jpeg);
        //            msImage.Position = 0;
        //            using (var ms = msImage)
        //            {
        //                await blobClient.UploadAsync(ms, true);
        //            }
        //            result.Data = blobClient.Uri.AbsoluteUri;
        //            //await blobClient.UploadAsync(file.OpenReadStream());
        //        }
        //        else
        //        {
        //            result.Error = ErrorType.Forbidden;
        //            result.ErrorMessage = "Image format is not jpeg or png";
        //            return result;
        //        }
        //    }

        //    return result;
        //}

        //private bool CheckIfImageFile(IFormFile file)
        //{
        //    byte[] fileBytes;
        //    using (var ms = new MemoryStream())
        //    {
        //        file.CopyTo(ms);
        //        fileBytes = ms.ToArray();
        //    }

        //    return (WriterHelper.GetImageFormat(fileBytes) == WriterHelper.ImageFormat.jpeg || WriterHelper.GetImageFormat(fileBytes) == WriterHelper.ImageFormat.png);
        //}

        //public static Bitmap ResizeImage(Image image, int width, int height)
        //{
        //    var destRect = new Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }

        //    return destImage;
        //}

        //public static Image FixedSize(Image imgPhoto, int Width, int Height)
        //{
        //    int sourceWidth = imgPhoto.Width;
        //    int sourceHeight = imgPhoto.Height;
        //    int sourceX = 0;
        //    int sourceY = 0;
        //    int destX = 0;
        //    int destY = 0;

        //    float nPercent = 0;
        //    float nPercentW = 0;
        //    float nPercentH = 0;

        //    nPercentW = ((float)Width / (float)sourceWidth);
        //    nPercentH = ((float)Height / (float)sourceHeight);
        //    if (nPercentH < nPercentW)
        //    {
        //        nPercent = nPercentH;
        //        destX = System.Convert.ToInt16((Width -
        //                      (sourceWidth * nPercent)) / 2);
        //    }
        //    else
        //    {
        //        nPercent = nPercentW;
        //        destY = System.Convert.ToInt16((Height -
        //                      (sourceHeight * nPercent)) / 2);
        //    }

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);

        //    Bitmap bmPhoto = new Bitmap(Width, Height,
        //                      PixelFormat.Format24bppRgb);
        //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
        //                     imgPhoto.VerticalResolution);

        //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //    grPhoto.Clear(Color.Black);
        //    grPhoto.InterpolationMode =
        //            InterpolationMode.HighQualityBicubic;

        //    grPhoto.DrawImage(imgPhoto,
        //        new Rectangle(destX, destY, destWidth, destHeight),
        //        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
        //        GraphicsUnit.Pixel);

        //    grPhoto.Dispose();
        //    return bmPhoto;
        //}
    }
}
