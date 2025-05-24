using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareNet_System.Models
{
    public enum billMethod
    {
        Cash,
        Visa
    }

    public class Bills
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0, 1000000, ErrorMessage = "Amount must be a positive value.")]
        public double total_amount { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        [Column(TypeName = "nvarchar(50)")]
        public billMethod Payment_Method { get; set; }

        public int? insurance_id { get; set; }

        [ForeignKey("patient")]
        [Required(ErrorMessage = "Patient ID is required.")]
        public int? patient_id { get; set; }

        public Patient? patient { get; set; }
    }

}
