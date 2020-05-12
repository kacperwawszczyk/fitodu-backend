using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Storage.Blobs;
using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Model.Extensions;
using Fitodu.Service.Abstract;
using Fitodu.Service.Attributes;
using Fitodu.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Fitodu.Service.Concrete
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly Context _context;
        private readonly IMapper _mapper;

        private readonly IEmailService _emailService;
        private readonly IEmailMarketingService _emailMarketingService;
        private readonly IBillingService _billingService;

        private string azureConnectionString;

        public UserService(
           ILogger<UserService> logger,
           IConfiguration configuration,
           IDateTimeService dateTimeService,
           SignInManager<User> signInManager,
           UserManager<User> userManager,
           Context context,
           IMapper mapper,
           IEmailService emailService,
           IEmailMarketingService emailMarketingService,
           IBillingService billingService)
        {
            _logger = logger;
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _emailMarketingService = emailMarketingService;
            _billingService = billingService;

            azureConnectionString = _configuration.GetConnectionString("StorageConnection");
        }

        //public async Task<Result<ICollection<UserListItemOutput>>> GetList(string userId)
        //{
        //    var result = new Result<ICollection<UserListItemOutput>>();

        //    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //    if (user == null)
        //    {
        //        result.Error = ErrorType.NotFound;
        //        return result;
        //    }

        //    /*IQueryable users = null;*/

        //    if (user.Role != UserRole.SuperAdmin)
        //    {
        //        users = _context.Users.Where(x => x.CompanyId == user.CompanyId);
        //    }
        //    else
        //    {
        //        users = _context.Users.Where(x => x.Id != user.Id);
        //    }

        //result.Data = await users
        //    .ProjectTo<UserListItemOutput>(_mapper.ConfigurationProvider)
        //        .ToListAsync();

        //    return result;
        //}

        public async Task<Result<UserOutput>> Get(string userId)
        {
            var result = new Result<UserOutput>();

            var user = await _context.Users.Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            var data = _mapper.Map<UserOutput>(user);
            result.Data = data;

            return result;
        }

        public async Task<Result<CompanyOutput>> GetCompany(string userId)
        {
            var result = new Result<CompanyOutput>();

            var user = await _context.Users.Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            var data = _mapper.Map<CompanyOutput>(user.Company);
            result.Data = data;

            return result;
        }



        //public async Task<Result<string>> Create(CreateUserInput model)
        //{
        //    var result = new Result<string>();

        //    var admin = await _context.Users.Include(x => x.Company)
        //        .FirstOrDefaultAsync(x => x.Id == model.AdminId);

        //    if (admin == null)
        //    {
        //        result.Error = ErrorType.NotFound;
        //        return result;
        //    }

        //    var existingUser = await _userManager.FindByNameAsync(model.Email);
        //    if (existingUser != null)
        //    {
        //        result.ErrorMessage = Resource.Validation.EmailTaken;
        //        result.Error = ErrorType.BadRequest;
        //        return result;
        //    }

        //    var userLimit = int.Parse(_configuration[$"Limits:{admin.Company.Plan.GetDescription()}User"]);
        //    if (userLimit != 0)
        //    {
        //        var userCount = await _context.Users.CountAsync(x => x.CompanyId == admin.CompanyId);
        //        if (userCount >= userLimit)
        //        {
        //            result.ErrorMessage = Resource.Validation.LimitReached;
        //            result.Error = ErrorType.BadRequest;
        //            return result;
        //        }
        //    }

        //    var user = new User();
        //    user.Role = UserRole.User;
        //    //user.FullName = model.FullName;
        //    user.Email = model.Email;
        //    user.PhoneNumber = model.PhoneNumber;
        //    user.UserName = model.Email;
        //    user.Company = admin.Company;

        //    user.SetCredentials(model.Password);

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        var createResult = await _userManager.CreateAsync(user);
        //        if (!createResult.Succeeded)
        //        {
        //            transaction.Rollback();
        //            result.Error = ErrorType.InternalServerError;
        //            return result;
        //        }

        //        var addToRoleResult = await _userManager.AddToRoleAsync(user, user.Role.GetName());
        //        if (!addToRoleResult.Succeeded)
        //        {
        //            transaction.Rollback();
        //            result.Error = ErrorType.InternalServerError;
        //            return result;
        //        }

        //        transaction.Commit();
        //        result.Data = user.Id;
        //    }

        //    return result;
        //}

        public async Task<Result<string>> Update(UpdateUserInput model)
        {
            var result = new Result<string>();

            if (model.Id == model.AdminId)
            {
                result.Error = ErrorType.Forbidden;
                return result;
            }

            var admin = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.AdminId);
            if (admin == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var user = await _context.Users.FirstOrDefaultAsync(
                    x => x.CompanyId == admin.CompanyId && x.Id == model.Id);

                if (user == null)
                {
                    result.Error = ErrorType.NotFound;
                    return result;
                }

                var existingUser = await _userManager.FindByNameAsync(model.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    result.ErrorMessage = Resource.Validation.EmailTaken;
                    result.Error = ErrorType.BadRequest;
                    return result;
                }

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.Email;
                user.UpdatedOn = _dateTimeService.Now();

                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.SetCredentials(model.Password);
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                transaction.Commit();
                result.Data = user.Id;
            }

            return result;
        }

        //public async Task<Result<string>> UpdateMe(UpdateUserInput model)
        //{
        //    var result = new Result<string>();

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
        //        if (user == null)
        //        {
        //            result.Error = ErrorType.NotFound;
        //            return result;
        //        }

        //        var existingUser = await _userManager.FindByNameAsync(model.Email);
        //        if (existingUser != null && existingUser.Id != user.Id)
        //        {
        //            result.ErrorMessage = Resource.Validation.EmailTaken;
        //            result.Error = ErrorType.BadRequest;
        //            return result;
        //        }

        //        //user.FullName = model.FullName;
        //        user.Email = model.Email;
        //        user.PhoneNumber = model.PhoneNumber;
        //        user.UserName = model.Email;
        //        user.UpdatedOn = _dateTimeService.Now();

        //        if (!string.IsNullOrEmpty(model.Password))
        //        {
        //            user.SetCredentials(model.Password);
        //        }

        //        var updateResult = await _userManager.UpdateAsync(user);
        //        if (!updateResult.Succeeded)
        //        {
        //            transaction.Rollback();
        //            result.Error = ErrorType.InternalServerError;
        //            return result;
        //        }

        //        if (user.Role == UserRole.CompanyAdmin)
        //        {
        //            var emailUpdateResult = await _emailMarketingService.AddOrUpdate(user);
        //            if (emailUpdateResult.Success)
        //                user.SubscriberId = emailUpdateResult.Data;
        //            else
        //                _logger.LogCritical("Can't update email marketing contact", user.UserName);

        //            if (user.Company.BillingCustomerId != null)
        //            {
        //                await _billingService.UpdateCustomer(user);
        //            }
        //        }

        //        transaction.Commit();
        //        result.Data = user.Id;
        //    }

        //    return result;
        //}

        //public async Task<Result<long>> UpdateCompany(UpdateCompanyInput model)
        //{
        //    var result = new Result<long>();

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
        //        if (user == null)
        //        {
        //            result.Error = ErrorType.NotFound;
        //            return result;
        //        }

        //        var admin = await _context.Users.FirstAsync(
        //            x => x.CompanyId == user.CompanyId && x.Role == UserRole.CompanyAdmin);

        //        var company = admin.Company;

        //        company.Name = model.Name;
        //        company.AddressLine1 = model.AddressLine1;
        //        company.AddressLine2 = model.AddressLine2;
        //        company.AddressPostalCode = model.AddressPostalCode;
        //        company.AddressCity = model.AddressCity;
        //        company.AddressState = model.AddressState;
        //        company.AddressCountry = model.AddressCountry;
        //        company.VatIn = model.VatIn;

        //        company.UpdatedOn = _dateTimeService.Now();

        //        _context.Update(company);

        //        if (await _context.SaveChangesAsync() == 0)
        //        {
        //            transaction.Rollback();
        //            result.Error = ErrorType.InternalServerError;
        //            return result;
        //        }

        //        if (user.Company.BillingCustomerId != null)
        //        {
        //            await _billingService.UpdateCustomer(admin);
        //        }

        //        transaction.Commit();
        //        result.Data = company.Id;
        //    }

        //    return result;
        //}

        public async Task<Result<string>> Delete(string adminId, string userId)
        {
            var result = new Result<string>();

            var admin = await _context.Users.FirstOrDefaultAsync(x => x.Id == adminId);
            if (admin == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var user = await _context.Users.FirstOrDefaultAsync(
                    x => x.CompanyId == admin.CompanyId
                    && x.Id == userId && x.Id != adminId);

                if (user == null)
                {
                    result.Error = ErrorType.NotFound;
                    return result;
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                transaction.Commit();
                result.Data = user.Id;
            }

            return result;
        }

        public async Task<Result<TokenOutput>> CreateToken(CreateTokenInput model, bool auth)
        {
            var result = new Result<TokenOutput>();
            bool firstLogin = false;

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                result.ErrorMessage = Resource.Validation.InvalidGrant;
                result.Error = ErrorType.BadRequest;
                return result;
            }

            if (user.Role == UserRole.Coach)
            {
                var coach = await _context.Coaches.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
                if (coach.CancelTimeHours == null || coach.CancelTimeMinutes == null)
                {
                    firstLogin = true;
                }
            }

            if (auth)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!signInResult.Succeeded)
                {
                    result.ErrorMessage = Resource.Validation.InvalidGrant;
                    result.Error = ErrorType.BadRequest;
                    return result;
                }
            }

            var accessToken = CreateAccessToken(user);

            result.Data = new TokenOutput()
            {
                AccessToken = accessToken.Item1,
                Expires = accessToken.Item2,
                RefreshToken = await CreateRefreshToken(user.Id),
                Id = user.Id,
                Email = user.UserName,
                Role = user.Role,
                Plan = user.Company.Plan,
                PlanExpiredOn = user.Company.PlanExpiredOn,
                FirstLogin = firstLogin
            };

            return result;
        }

        public async Task<Result<TokenOutput>> RefreshToken(RefreshTokenInput model)
        {
            var result = new Result<TokenOutput>();

            var refreshToken = await _context.RefreshTokens.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == model.Token);

            if (refreshToken == null)
            {
                result.ErrorMessage = Resource.Validation.InvalidGrant;
                result.Error = ErrorType.BadRequest;
                return result;
            }

            if (!refreshToken.IsActive(_dateTimeService.Now()))
            {
                result.Error = ErrorType.Unauthorized;
                return result;
            }

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();

            var user = refreshToken.User;
            var accessToken = CreateAccessToken(user);

            result.Data = new TokenOutput()
            {
                AccessToken = accessToken.Item1,
                Expires = accessToken.Item2,
                RefreshToken = await CreateRefreshToken(user.Id),
                Id = user.Id,
                Email = user.UserName,
                Role = user.Role,
                Plan = user.Company.Plan,
                PlanExpiredOn = user.Company.PlanExpiredOn
            };

            return result;
        }

        public async Task<Result> SignOut(SignOutInput model)
        {
            var result = new Result();

            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.Token);
            if (refreshToken == null)
            {
                result.ErrorMessage = Resource.Validation.InvalidGrant;
                result.Error = ErrorType.BadRequest;
                return result;
            }

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<Result> ForgotPassword(string email)
        {
            var result = new Result();

            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                return result;
            }

            if (user.ResetTokenExpiresOn > _dateTimeService.Now())
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "You have to wait a bit before you can change your password again";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                user.ResetToken = resetToken;
                user.ResetTokenExpiresOn = _dateTimeService.Now().AddMinutes(30);

                _context.Update(user);

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                transaction.Commit();
            }

            var encodedToken = WebUtility.UrlEncode(user.ResetToken);
            var url = $"{_configuration["Environment:ClientUrl"]}/auth/reset?id={user.Id}&token={encodedToken}";

            var model = new EmailInput()
            {
                To = email,
                Subject = Resource.Template.ResetPasswordSubject,
                HtmlBody = Resource.Template.ResetPasswordBody
            };

            model.HtmlBody = model.HtmlBody.Replace("-fullName-", user.FullName).Replace("-url-", url);

            var response = await _emailService.Send(model);

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
            }

            return result;
        }

        public async Task<Result> ResetPassword(ResetPasswordInput model)
        {
            var result = new Result();

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                result.ErrorMessage = "Nie znaleziono uzytkownika";
                result.Error = ErrorType.BadRequest;
                return result;
            }

            if (_dateTimeService.Now() > user.ResetTokenExpiresOn)
            {
                result.ErrorMessage = "Token wygasl";
                result.Error = ErrorType.BadRequest;
                return result;
            }

            //var decoded = WebUtility.UrlDecode(model.ResetToken);


            var resetResult = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
            if (!resetResult.Succeeded)
            {
                result.ErrorMessage = "Blad zmiany";
                result.Error = ErrorType.BadRequest;
            }

            user.ResetTokenExpiresOn = _dateTimeService.Now();

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Update(user);

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                transaction.Commit();
            }

            return result;
        }

        public async Task<Result<string>> UpdateAvatar(string id, IFormFile file)
        {
            var result = new Result<string>();


            var client = _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (client == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Not a valid user";
                return result;
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(id);

            if (await blobContainerClient.ExistsAsync() == false)
            {
                blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(id);
                blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            if (CheckIfImageFile(file))
            {
                Image image = Image.FromStream(file.OpenReadStream(), true, true);
                //if (image.Width < 128 || image.Width > 1024 || image.Height < 128 || image.Height > 1024)
                //{
                //    result.Error = ErrorType.Forbidden;
                //    result.ErrorMessage = "Image have to be between 128x128 and 1024x1024";
                //    return result;
                //}
                //Bitmap newImage = ResizeImage(image, 150, 150);
                Image newImage = FixedSize(image, 150, 150);
                BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                MemoryStream msImage = new MemoryStream();
                newImage.Save(msImage, ImageFormat.Jpeg);
                msImage.Position = 0;
                using (var ms = msImage)
                {
                    await blobClient.UploadAsync(ms, true);
                }
                result.Data = blobClient.Uri.AbsoluteUri;
                //await blobClient.UploadAsync(file.OpenReadStream());
            }
            else
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "Image format is not jpeg or png";
                return result;
            }


            return result;
        }

        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return (WriterHelper.GetImageFormat(fileBytes) == WriterHelper.ImageFormat.jpeg || WriterHelper.GetImageFormat(fileBytes) == WriterHelper.ImageFormat.png);
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private (string, DateTime) CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Role.GetName()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var expires = _dateTimeService.Now().AddMinutes(15);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            return (jwtSecurityTokenHandler.WriteToken(jwtSecurityToken), expires);
        }

        private async Task<string> CreateRefreshToken(string userId)
        {
            var createdOn = _dateTimeService.Now();
            var expiresOn = createdOn.AddMonths(1);

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                CreatedOn = createdOn,
                ExpiresOn = expiresOn,
                Token = Guid.NewGuid().ToString()
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }
    }
}