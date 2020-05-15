using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class UpdateClientInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? FatPercentage { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressCity { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressState { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressCountry { get; set; }

        //public static Client Convert(UpdateClientInput client)
        //{
        //    Client clientOutput = new Client();
        //    clientOutput.Name = client.Name;
        //    clientOutput.Surname = client.Surname;
        //    clientOutput.Height = client.Height;
        //    clientOutput.Weight = client.Weight;
        //    clientOutput.FatPercentage = client.FatPercentage;
        //    clientOutput.AddressCity = client.AddressCity;
        //    clientOutput.AddressCountry = client.AddressCountry;
        //    clientOutput.AddressLine1 = client.AddressLine1;
        //    clientOutput.AddressLine2 = client.AddressLine2;
        //    clientOutput.AddressPostalCode = client.AddressPostalCode;
        //    clientOutput.AddressState = client.AddressState;
        //    clientOutput.UpdatedOn = DateTime.UtcNow;
        //    return clientOutput;
        //}
    }
}
