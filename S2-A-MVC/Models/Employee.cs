namespace S2_A_MVC.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset HiringDate { get; set; }
        public double YearlySalary { get; set; }
        
        public double YearsOfExperience => Math.Round((DateTimeOffset.Now - HiringDate).TotalDays / 365, 3);

        public override string ToString()
        {
            var hiringDate = HiringDate.DateTime.ToShortDateString();
            return string.Join(",", Id, Name, hiringDate, YearsOfExperience, YearlySalary);
        }
    }
}
