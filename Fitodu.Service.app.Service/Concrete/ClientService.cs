using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Model.Extensions;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fitodu.Service.Models.Client;
using AutoMapper.QueryableExtensions;
using Fitodu.Service.Models.PrivateNote;
using AutoMapper;
using Fitodu.Service.Models.PublicNote;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using Fitodu.Service.Attributes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Fitodu.Service.Concrete
{
    public class ClientService : IClientService
    {
        private readonly Context _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private string azureConnectionString;

        public ClientService(Context context, IDateTimeService dateTimeService, IConfiguration configuration, IEmailService emailService, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _configuration = configuration;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
            azureConnectionString = _configuration.GetConnectionString("StorageConnection");
        }
        public async Task<Result> CreateClientAccount(RegisterClientInput model)
        {

            var result = new Result();

            var exist = await _context.CreateClientTokens.Where(x => x.UserId == model.Id && x.Token == model.Token).FirstOrDefaultAsync();

            if (exist == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid verification code";
                return result;
            }

            if (exist.ExpiresOn < DateTime.Now)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Code expired";
                return result;
            }

            var client = await _context.Clients.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (client == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid User";
                return result;
            }

            var coachClient = await _context.CoachClients.Where(x => x.IdClient == client.Id).FirstOrDefaultAsync();

            client.IsRegistered = true;

            var exisitingUser = await _context.Users.Where(x => x.NormalizedEmail == model.Email.ToUpper()).FirstOrDefaultAsync();

            if (exisitingUser != null)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Cannot create User (User already exists)";
                return result;
            }

            var user = new User
            {
                Role = UserRole.Client,
                FullName = $"{client.Name} {client.Surname}",
                Id = model.Id,
                Email = model.Email,
                UserName = model.Email,
                //PhoneNumber = model.PhoneNumber,
                Company = new Company()
                {
                    Plan = PricingPlan.Trial,
                    PlanExpiredOn = _dateTimeService.Now().AddDays(30)
                }
            };

            user.SetCredentials(model.Password);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create User (User already exists)";
                    return result;
                }

                var publicNotes = await _context.PublicNotes.Where(x => x.IdClient == client.Id).ToListAsync();
                if (publicNotes.Count == 0)
                {
                    PublicNote publicNote = new PublicNote();
                    publicNote.IdClient = client.Id;
                    publicNote.IdCoach = coachClient.IdCoach;
                    var notesResult = await _context.PublicNotes.AddAsync(publicNote);
                    if (notesResult.State != EntityState.Added)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Cannot create public note";
                        return result;
                    }
                    client.PublicNote = publicNote;
                }
                var privateNotes = await _context.PrivateNotes.Where(x => x.IdClient == client.Id).ToListAsync();
                if (privateNotes.Count == 0)
                {
                    PrivateNote privateNote = new PrivateNote();
                    privateNote.IdClient = client.Id;
                    privateNote.IdCoach = coachClient.IdCoach;
                    var notesResult = await _context.PrivateNotes.AddAsync(privateNote);
                    if (notesResult.State != EntityState.Added)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Cannot create private note";
                        return result;
                    }
                    client.PrivateNote = privateNote;
                }

                BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(model.Id);
                blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(model.Id);
                blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                var addToRoleResult = await _userManager.AddToRoleAsync(user, user.Role.GetName());

                if (!addToRoleResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add User to role.";
                    return result;
                }

                var updateRegisteredStatus = _context.Clients.Update(client);

                if (updateRegisteredStatus.State != EntityState.Modified)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot update Client register state";
                    return result;
                }

                if (await _context.SaveChangesAsync() <= 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot save changes";
                    return result;
                }

                transaction.Commit();
            }

            return result;
        }

        public async Task<Result<string>> DummyClientRegister(string CoachId, RegisterDummyClientInput model)
        {
            var result = new Result<string>();

            var coach = await _context.Coaches.Where(x => x.Id == CoachId).FirstOrDefaultAsync();

            if (coach == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Coach doesn't exist";
                return result;
            }

            var newClient = new Client
            {
                Name = model.Name,
                Surname = model.Surname,
                Id = Guid.NewGuid().ToString(),
                IsRegistered = false
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var publicNotes = await _context.PublicNotes.Where(x => x.IdClient == newClient.Id).ToListAsync();
                if (publicNotes.Count == 0)
                {
                    PublicNote publicNote = new PublicNote();
                    publicNote.IdClient = newClient.Id;
                    publicNote.IdCoach = CoachId;
                    var notesResult = await _context.PublicNotes.AddAsync(publicNote);
                    if (notesResult.State != EntityState.Added)
                    {
                        transaction.Rollback();

                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Cannot create public note";
                        return result;
                    }
                    newClient.PublicNote = publicNote;
                }

                var privateNotes = await _context.PrivateNotes.Where(x => x.IdClient == newClient.Id).ToListAsync();
                if (privateNotes.Count == 0)
                {
                    PrivateNote privateNote = new PrivateNote();
                    privateNote.IdClient = newClient.Id;
                    privateNote.IdCoach = CoachId;
                    var notesResult = await _context.PrivateNotes.AddAsync(privateNote);
                    if (notesResult.State != EntityState.Added)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Cannot create private note";
                        return result;
                    }
                    newClient.PrivateNote = privateNote;
                }
                var addClientResult = await _context.Clients.AddAsync(newClient);

                if (addClientResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create client";
                    return result;
                }

                var coachClient = new CoachClient
                {
                    IdCoach = CoachId,
                    IdClient = newClient.Id
                };

                var addCoachClientResult = await _context.CoachClients.AddAsync(coachClient);

                if (addCoachClientResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add Client to Coach";
                    return result;
                }

                _context.SaveChanges();

                transaction.Commit();

            }
            result.Data = newClient.Id;
            return result;
        }

        public async Task<Result> SelfCreateClientAccount(SelfRegisterClientInput model)
        {
            var result = new Result();

            var creationToken = await _context.CreateClientTokens.Where(x => x.Token == model.Token).FirstOrDefaultAsync();

            if (creationToken == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid token";
                return result;
            }

            if (creationToken.ExpiresOn < DateTime.Now)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Code expired";
                return result;
            }

            var coach = await _context.Coaches.Where(x => x.Id == model.IdCoach).FirstOrDefaultAsync();

            if (coach == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Coach doesn't exist";
                return result;
            }

            var newClient = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Surname = model.Surname,
                IsRegistered = true
            };

            var newUser = new User
            {
                Id = newClient.Id,
                Role = UserRole.Client,
                FullName = $"{model.Name} {model.Surname}",
                Email = model.Email,
                UserName = model.Email,
                //PhoneNumber = model.PhoneNumber,
                Company = new Company()
                {
                    Plan = PricingPlan.Trial,
                    PlanExpiredOn = _dateTimeService.Now().AddDays(30)
                }
            };

            newUser.SetCredentials(model.Password);

            var coachClient = new CoachClient
            {
                IdClient = newClient.Id,
                IdCoach = model.IdCoach
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var publicNotes = await _context.PublicNotes.Where(x => x.IdClient == newClient.Id).ToListAsync();
                if (publicNotes.Count == 0)
                {
                    PublicNote publicNote = new PublicNote();
                    publicNote.IdClient = newClient.Id;
                    publicNote.IdCoach = model.IdCoach;
                    var notesResult = await _context.PublicNotes.AddAsync(publicNote);
                    if (notesResult.State != EntityState.Added)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Cannot create public note";
                        return result;
                    }
                    newClient.PublicNote = publicNote;
                }
                var privateNotes = await _context.PrivateNotes.Where(x => x.IdClient == newClient.Id).ToListAsync();
                if (privateNotes.Count == 0)
                {
                    PrivateNote privateNote = new PrivateNote();
                    privateNote.IdClient = newClient.Id;
                    privateNote.IdCoach = model.IdCoach;
                    var notesResult = await _context.PrivateNotes.AddAsync(privateNote);
                    if (notesResult.State != EntityState.Added)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Cannot create private note";
                        return result;
                    }
                    newClient.PrivateNote = privateNote;
                }
                var createResult = await _userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create User.";
                    return result;
                }

                BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(newClient.Id);
                blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(newClient.Id);
                blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, newUser.Role.GetName());

                if (!addToRoleResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add User to role.";
                    return result;
                }

                var createClient = await _context.Clients.AddAsync(newClient);

                if (createClient.State != EntityState.Added)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create Client.";
                    return result;
                }

                var addCoachClient = await _context.CoachClients.AddAsync(coachClient);

                if (addCoachClient.State != EntityState.Added)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add Client to Coach.";
                    return result;
                }

                if (await _context.SaveChangesAsync() <= 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot save changes";
                    return result;
                }

                transaction.Commit();
            }

            return result;

        }

        public async Task<Result> SendCreationLinkToClient(string CoachId, CreateClientVerificationTokenInput model)
        {
            var result = new Result();

            var coach = await _context.Users.Where(x => x.Id == CoachId).FirstOrDefaultAsync();

            if (coach == null)
            {
                result.Error = ErrorType.BadRequest;

                result.ErrorMessage = "Requesting coach does not exist.";

                return result;
            }

            if (coach.Role != UserRole.Coach)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Requesting coach account role is not coach.";
                return result;
            }

            var client = await _context.Clients.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (client == null)
            {
                result.Error = ErrorType.BadRequest;

                result.ErrorMessage = $"Client (ID: {model.Id}) doesn't exist";

                return result;
            }

            var coachClient = await _context.CoachClients.Where(x => x.IdClient == model.Id && x.IdCoach == CoachId).FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;

                result.ErrorMessage = $"Client and coach are not connected.";

                return result;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.Role, UserRole.Client.GetName()),
                new Claim(ClaimTypes.Name, model.Name)
            };

            var expires = _dateTimeService.Now().AddMinutes(30);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var createClientToken = new CreateClientToken
            {
                UserId = model.Id,
                Token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken),
                ExpiresOn = _dateTimeService.Now().AddMinutes(30)
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var addTokenResult = await _context.CreateClientTokens.AddAsync(createClientToken);

                if (addTokenResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create token";
                    return result;
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            var encodedToken = WebUtility.UrlEncode(jwtSecurityTokenHandler.WriteToken(jwtSecurityToken));
            var url = $"{_configuration["Environment:ClientUrl"]}/auth/register?id={model.Id}&token={encodedToken}&email={model.Email}";

            var message = new EmailInput()
            {
                To = model.Email,
                Subject = Resource.VerifyClientTemplate.VerifyClientSubject,
                HtmlBody = Resource.VerifyClientTemplate.VerifyClientBody
            };

            message.HtmlBody = message.HtmlBody.Replace("-fullName-", model.FullName).Replace("-url-", url);

            var response = await _emailService.Send(message);

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Cannot send email.";
            }

            return result;
        }

        public async Task<Result> SendSelfCreationLinkToClient(string CoachId, CreateSelfClientVerificationTokenInput model)
        {
            var result = new Result();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "noId"),
                new Claim(ClaimTypes.Role, UserRole.Client.GetName()),
                new Claim(ClaimTypes.Name, "noName")
            };

            var expires = _dateTimeService.Now().AddMinutes(30);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var createClientToken = new CreateClientToken
            {
                UserId = "noId",
                Token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken),
                ExpiresOn = _dateTimeService.Now().AddMinutes(30)
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var addTokenResult = await _context.CreateClientTokens.AddAsync(createClientToken);

                if (addTokenResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create token.";
                    return result;
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            var encodedToken = WebUtility.UrlEncode(jwtSecurityTokenHandler.WriteToken(jwtSecurityToken));
            var url = $"{_configuration["Environment:ClientUrl"]}/auth/register?token={encodedToken}&coachId={CoachId}&email={model.Email}";

            var message = new EmailInput()
            {
                To = model.Email,
                Subject = Resource.VerifyClientTemplate.VerifyClientSubject,
                HtmlBody = Resource.VerifyClientTemplate.VerifyClientBody
            };

            message.HtmlBody = message.HtmlBody.Replace("-fullName-", model.Email).Replace("-url-", url);

            var response = await _emailService.Send(message);

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Cannot send email.";
            }

            return result;
        }

        public async Task<Result<ClientOutput>> GetClient(string Id)
        {
            var result = new Result<ClientOutput>();
            ClientOutput client = await _context.Clients
                .Where(x => x.Id == Id)
                .Select(x => new ClientOutput
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Height = x.Height,
                    Weight = x.Weight,
                    FatPercentage = x.FatPercentage,
                    IsRegistered = x.IsRegistered,
                    AddressCity = x.AddressCity,
                    AddressCountry = x.AddressCountry,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    AddressPostalCode = x.AddressPostalCode,
                    AddressState = x.AddressState,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email
                })
                .FirstOrDefaultAsync();



            if (client == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Client not found";
            }
            else
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(client.Id);
                if (await blobContainerClient.ExistsAsync() == false)
                {
                    client.Avatar = null;
                }
                else
                {
                    BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                    if (await blobClient.ExistsAsync() == false)
                    {
                        client.Avatar = null;
                    }
                    else
                    {
                        client.Avatar = blobClient.Uri.AbsoluteUri;
                    }
                }
                User clientAcc = await _context.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (clientAcc != null)
                {
                    client.PhoneNumber = clientAcc.PhoneNumber;
                    client.Email = clientAcc.Email;
                }
                CoachClient clientTrain = await _context.CoachClients.Where(x => x.IdClient == Id).FirstOrDefaultAsync();
                if (clientTrain != null)
                {
                    client.AvailableTrainings = clientTrain.AvailableTrainings;
                }
                result.Data = client;
            }
            return result;
        }

        public async Task<Result<long>> UpdateClient(string Id, UpdateClientInput model)
        {
            Client client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == Id);
            var result = new Result<long>();
            if (client == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Client not found";
                return result;
            }
            else
            {
                client.Name = model.Name;
                client.Surname = model.Surname;
                client.Height = model.Height;
                client.Weight = model.Weight;
                client.FatPercentage = model.FatPercentage;
                client.AddressCity = model.AddressCity;
                client.AddressCountry = model.AddressCountry;
                client.AddressLine1 = model.AddressLine1;
                client.AddressLine2 = model.AddressLine2;
                client.AddressPostalCode = model.AddressPostalCode;
                client.AddressState = model.AddressState;
                client.UpdatedOn = DateTime.UtcNow;

                User clientAcc = await _context.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (clientAcc != null)
                {
                    clientAcc.PhoneNumber = model.PhoneNumber;
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Users.Update(clientAcc);
                    _context.Clients.Update(client);
                    if (await _context.SaveChangesAsync() <= 0)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Couldn't execute changes to the database.";
                    }
                    else
                    {
                        transaction.Commit();
                    }
                    return result;
                }

            }
        }

        public async Task<Result<CoachOutput>> GetClientCoach(string Id)
        {
            Client client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == Id);
            var result = new Result<CoachOutput>();
            if (client == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Client not found";
                return result;
            }
            else
            {
                CoachClient coachClient = await _context.CoachClients.Where(x => Id == x.IdClient).FirstOrDefaultAsync();
                if (coachClient == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Coach not found";
                    return result;
                }
                else
                {
                    CoachOutput coach = await _context.Coaches
                        .Where(x => x.Id == coachClient.IdCoach)
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
                        .FirstOrDefaultAsync();
                    if (coach == null)
                    {
                        result.Error = ErrorType.NotFound;
                        result.ErrorMessage = "Coach does not exist";
                    }
                    else
                    {
                        BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(coach.Id);
                        if (await blobContainerClient.ExistsAsync() == false)
                        {
                            coach.Avatar = null;
                        }
                        else
                        {
                            BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                            if (await blobClient.ExistsAsync() == false)
                            {
                                coach.Avatar = null;
                            }
                            else
                            {
                                coach.Avatar = blobClient.Uri.AbsoluteUri;
                            }
                        }
                        User coachAcc = await _context.Users.Where(x => x.Id == coach.Id).FirstOrDefaultAsync();
                        if (coachAcc != null)
                        {
                            coach.PhoneNumber = coachAcc.PhoneNumber;
                            coach.Email = coachAcc.Email;
                        }
                        result.Data = coach;
                    }
                    return result;
                }
            }
        }

        public async Task<Result<ClientOutput>> GetClientById(string coachId, string clientId)
        {
            Coach coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == coachId);
            var result = new Result<ClientOutput>();
            if (coach == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Coach does not exist";
                //return result;
            }
            else
            {
                CoachClient coachClient = await _context.CoachClients.FirstOrDefaultAsync(x => (x.IdClient == clientId) && (x.IdCoach == coachId));
                if (coachClient == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "User is not a client of selected coach";
                }
                else
                {
                    ClientOutput client = await _context.Clients
                                                        .Where(x => x.Id == clientId)
                                                        .Select(x => new ClientOutput
                                                        {
                                                            Id = x.Id,
                                                            Name = x.Name,
                                                            Surname = x.Surname,
                                                            Height = x.Height,
                                                            Weight = x.Weight,
                                                            FatPercentage = x.FatPercentage,
                                                            IsRegistered = x.IsRegistered,
                                                            AddressCity = x.AddressCity,
                                                            AddressCountry = x.AddressCountry,
                                                            AddressLine1 = x.AddressLine1,
                                                            AddressLine2 = x.AddressLine2,
                                                            AddressPostalCode = x.AddressPostalCode,
                                                            AddressState = x.AddressState,
                                                            PhoneNumber = x.PhoneNumber,
                                                            Email = x.Email
                                                        })
                                                        .FirstOrDefaultAsync();
                    if (client == null)
                    {
                        result.Error = ErrorType.NotFound;
                        result.ErrorMessage = "Client does not exist";
                    }
                    else
                    {
                        BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(client.Id);
                        if (await blobContainerClient.ExistsAsync() == false)
                        {
                            client.Avatar = null;
                        }
                        else
                        {
                            BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                            if (await blobClient.ExistsAsync() == false)
                            {
                                client.Avatar = null;
                            }
                            else
                            {
                                client.Avatar = blobClient.Uri.AbsoluteUri;
                            }
                        }
                        User clientAcc = await _context.Users.Where(x => x.Id == clientId).FirstOrDefaultAsync();
                        if (clientAcc != null)
                        {
                            client.PhoneNumber = clientAcc.PhoneNumber;
                            client.Email = clientAcc.Email;
                        }
                        CoachClient clientTrain = await _context.CoachClients.Where(x => x.IdClient == clientId).FirstOrDefaultAsync();
                        if (clientTrain != null)
                        {
                            client.AvailableTrainings = clientTrain.AvailableTrainings;
                        }
                        result.Data = client;
                    }
                }
            }
            return result;
        }

        public async Task<Result<ClientNotes>> GetClientNotes(string requesterId, UserRole role, string clientId)
        {
            var result = new Result<ClientNotes>();

            IQueryable privateNote = null;
            IQueryable publicNote = null;

            if (role == UserRole.Coach)
            {
                privateNote = _context.PrivateNotes.Where(x => x.IdCoach == requesterId && x.IdClient == clientId);
                publicNote = _context.PublicNotes.Where(x => x.IdCoach == requesterId && x.IdClient == clientId);
            }
            else if (role == UserRole.Client)
            {
                if (requesterId != clientId)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "Cannot get notes of another client.";
                    return result;
                }

                privateNote = _context.PrivateNotes.Where(x => x.IdClient == clientId);
                publicNote = _context.PublicNotes.Where(x => x.IdClient == clientId);
            }

            ClientNotes clientNotes = new ClientNotes();
            clientNotes.PrivateNote = await privateNote.ProjectTo<PrivateNoteOutput>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            clientNotes.PublicNote = await publicNote.ProjectTo<PublicNoteOutput>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            result.Data = clientNotes;
            return result;
        }

        public async Task<Result> DummyClientDelete(string requesterId, UserRole requesterRole, string clientId)
        {
            Result result = new Result();
            if (requesterRole != UserRole.Coach)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "This user is not a coach";
                return result;
            }

            var coachClient = await _context.CoachClients.Where(x => x.IdCoach == requesterId && x.IdClient == clientId).FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Selected user is not a client of this coach.";
                return result;
            }

            var client = await _context.Clients.Where(x => x.Id == clientId).FirstOrDefaultAsync();

            if (client == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "This client does not exist.";
                return result;
            }

            var clientAcc = await _context.Users.Where(x => x.Id == client.Id).FirstOrDefaultAsync();

            if (clientAcc != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client is not a dummy client.";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.CoachClients.Remove(coachClient);
                _context.Clients.Remove(client);
                if (await _context.SaveChangesAsync() <= 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Could not save changes to the database.";
                    return result;
                }
                else
                {
                    transaction.Commit();
                }
            }
            return result;
        }

        public async Task<Result> DummyClientUpdate(string CoachId, UserRole role, string ClientId, DummyClientUpdateInput model)
        {
            Result result = new Result();

            if (role != UserRole.Coach)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "This user is not a coach";
                return result;
            }

            var coachClient = await _context.CoachClients.Where(x => x.IdCoach == CoachId && x.IdClient == ClientId).FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Selected user is not a client of this coach.";
                return result;
            }

            var client = await _context.Clients.Where(x => x.Id == ClientId).FirstOrDefaultAsync();

            if (client == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "This client does not exist.";
                return result;
            }

            var clientAcc = await _context.Users.Where(x => x.Id == client.Id).FirstOrDefaultAsync();

            if (clientAcc != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client is not a dummy client.";
                return result;
            }

            client.Name = model.Name;
            client.Surname = model.Surname;
            client.Email = model.Email;
            client.PhoneNumber = model.PhoneNumber;
            client.Weight = model.Weight;
            client.Height = model.Height;
            client.FatPercentage = model.FatPercentage;
            client.AddressCity = model.AddressCity;
            client.AddressCountry = model.AddressCountry;
            client.AddressLine1 = model.AddressLine1;
            client.AddressLine2 = model.AddressLine2;
            client.AddressPostalCode = model.AddressPostalCode;
            client.AddressState = model.AddressState;

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Clients.Update(client);
                if (await _context.SaveChangesAsync() <= 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Could not save changes to the database.";
                    return result;
                }
                else
                {
                    transaction.Commit();
                }
            }
            return result;
        }

        public async Task<Result<string>> UpdateAvatar(string id, UserRole role, IFormFile file)
        {
            var result = new Result<string>();

            if (role != UserRole.Client)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "User is not a client";
                return result;
            }
            else
            {
                var client = _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (client == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "Not a valid user (maybe 'dummy' client?)";
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
                    Bitmap newImage = ResizeImage(image, 150, 150);
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
                    result.ErrorMessage = "Image format is not jpeg";
                    return result;
                }
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

            return WriterHelper.GetImageFormat(fileBytes) == WriterHelper.ImageFormat.jpeg;
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
    }
}
