using System.Text.Json.Serialization;

namespace TestTask.Entities
{
    public class Passport
    {
        public string Type { get; set; }
        public string Number { get; set; }
        [JsonIgnore]
        public int EmployeeId { get; set; }
    }
}
