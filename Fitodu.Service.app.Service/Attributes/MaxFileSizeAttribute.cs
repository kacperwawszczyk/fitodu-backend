using System.ComponentModel.DataAnnotations;
using Fitodu.Resource;
using Microsoft.AspNetCore.Http;

namespace Fitodu.Service.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize * 1024 * 1024)
                {
                    return new ValidationResult(
                        string.Format(Validation.MaxFileSizeError, _maxFileSize));
                }
            }

            return ValidationResult.Success;
        }
    }
}
