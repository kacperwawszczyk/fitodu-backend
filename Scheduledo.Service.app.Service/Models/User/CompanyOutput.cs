
namespace Scheduledo.Service.Models
{
    public class CompanyOutput : UpdateCompanyInput
    {
        public string Url { get; set; }

        public int SmsCounter { get; set; }
        public int EmailCounter { get; set; }
    }
}