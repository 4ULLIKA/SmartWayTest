using System.Text.Json.Serialization;

namespace TestTask.Entities
{
    public class Departament
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public int EmployeeId { get; set; }
    }
}
