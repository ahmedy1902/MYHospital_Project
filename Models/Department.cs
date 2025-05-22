using System.ComponentModel.DataAnnotations;

namespace CareNet_System.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name must NOT be more than 20 characters")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [Unique]
        public string name { get; set; }


        [Required]
        public string manager { get; set; }
        public int Patients_num { get; set; }
        public int employees_num { get; set; }

        public List<Staff>? staff { get; set; }
        public List<Patient>? patients { get; set; }
        

    }
}
