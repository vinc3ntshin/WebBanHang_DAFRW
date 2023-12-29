using System.ComponentModel.DataAnnotations;

namespace WebBanHang_DAFRW.Models
{
	public class UserModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Làm ơn nhập UserName")]
		public string? UserName { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Làm ơn nhập Password")]
		public string? Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
