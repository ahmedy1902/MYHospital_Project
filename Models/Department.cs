    using System.ComponentModel.DataAnnotations;

    namespace CareNet_System.Models
    {
        public class Department
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Department name is required.")]
            [MaxLength(20, ErrorMessage = "Name must NOT be more than 20 characters.")]
            [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
            [Unique]
            public string name { get; set; }

            [Required(ErrorMessage = "Manager name is required.")]
            [StringLength(50, ErrorMessage = "Manager name must not exceed 50 characters.")]
            public string manager { get; set; }

            [Range(0, 1000, ErrorMessage = "Patients number must be between 0 and 1000.")]
            [Required(ErrorMessage = "Number of patients is required.")]

            public int Patients_num { get; set; }

            [Range(0, 1000, ErrorMessage = "Employees number must be between 0 and 1000.")]
            [Required(ErrorMessage = "Number of employees is required.")]

            public int employees_num { get; set; }

            public List<Staff>? staff { get; set; }
            public List<Patient>? patients { get; set; }
        }
    }
