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

        [Display(Name = "Patient Name")]
        public string name { get; set; }
        public int room_num { get; set; }

        [ForeignKey("followUpDoctor")]
        public int followUp_doctorID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Treatment Type")]
        public TreatmentType? treatment { get; set; }

        [ForeignKey("department")]
        public int dept_id { get; set; }

        [Display(Name = "Department Name")]
        public Department? department { get; set; }

        [Display(Name = "Doctor Name")]
        public Staff? followUpDoctor { get; set; }

        public List<Bills>? bills { get; set; }
    }
}