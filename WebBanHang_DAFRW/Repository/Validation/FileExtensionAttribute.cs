using System.ComponentModel.DataAnnotations;

namespace WebBanHang_DAFRW.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
	{
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName); // Không chuyển phần mở rộng thành chữ thường
                string[] extensions = { ".jpg", ".png", ".jpeg" }; // Đảm bảo phần mở rộng bắt đầu bằng dấu chấm
                if (!extensions.Contains(extension))
                {
                    return new ValidationResult("Chỉ cho phép tệp có phần mở rộng .jpg, .png hoặc .jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
