using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareNet_System.Models
{
    public enum TreatmentType
    {
        Drug, Surgery, Radiation, Chemotherapy, PhysicalTherapy
    }

    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [Display(Name = "Patient Name")]
        [StringLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [Display(Name = "Room Number")]
        [Range(1, 1000, ErrorMessage = "Room number must be between 1 and 1000.")]
        public int room_num { get; set; }

        [Required(ErrorMessage = "Follow-up doctor is required.")]
        [ForeignKey("followUpDoctor")]
        [Display(Name = "Follow-up Doctor")]
        public int followUp_doctorID { get; set; }

        [Display(Name = "Treatment Type")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Treatment type is required.")]
        public TreatmentType? treatment { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [ForeignKey("department")]
        [Display(Name = "Department")]
        public int dept_id { get; set; }

        [Display(Name = "Department Name")]
        public Department? department { get; set; }

        [Display(Name = "Doctor Name")]
        public Staff? followUpDoctor { get; set; }

        public List<Bills>? bills { get; set; }
    }
}
