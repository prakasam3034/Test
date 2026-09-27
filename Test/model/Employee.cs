using Test.Enum;

namespace Test.model
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public EmployeeStatus status { get; set; }  
        public string Department { get; set; }

        public decimal Salary { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
