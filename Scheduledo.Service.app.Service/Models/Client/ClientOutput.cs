using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Service.Models
{
    public class ClientOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
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
        
    }
}
