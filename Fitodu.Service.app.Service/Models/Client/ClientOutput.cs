using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class ClientOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int AvailableTrainings { get; set; }
        public bool IsRegistered { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? FatPercentage { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string Avatar { get; set; }


        //public static ClientOutput Convert(Client client)
        //{
        //    ClientOutput clientOutput = new ClientOutput();
        //    clientOutput.Id = client.Id;
        //    clientOutput.Name = client.Name;
        //    clientOutput.Surname = client.Surname;
        //    clientOutput.Height = client.Height;
        //    clientOutput.Weight = client.Weight;
        //    clientOutput.FatPercentage = client.FatPercentage;
        //    clientOutput.IsRegistered = client.IsRegistered;
        //    clientOutput.AddressCity = client.AddressCity;
        //    clientOutput.AddressCountry = client.AddressCountry;
        //    clientOutput.AddressLine1 = client.AddressLine1;
        //    clientOutput.AddressLine2 = client.AddressLine2;
        //    clientOutput.AddressPostalCode = client.AddressPostalCode;
        //    clientOutput.AddressState = client.AddressState;
        //    return clientOutput;
        //}
    }
}
