using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Service.Models
{
    public class CoachOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Rules { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string TimeToResign { get; set; }

        public static CoachOutput GetCoachOutput(Coach coach)
        {
            CoachOutput coachOutput = new CoachOutput();
            coachOutput.Id = coach.Id;
            coachOutput.Name = coach.Name;
            coachOutput.Surname = coach.Surname;
            coachOutput.Rules = coach.Rules;
            coachOutput.AddressLine1 = coach.AddressLine1;
            coachOutput.AddressLine2 = coach.AddressLine2;
            coachOutput.AddressPostalCode = coach.AddressPostalCode;
            coachOutput.AddressCity = coach.AddressCity;
            coachOutput.AddressState = coach.AddressState; 
            coachOutput.AddressCountry = coach.AddressCountry;
            coachOutput.TimeToResign = coach.TimeToResign;
            return coachOutput;
        }
    }
}
