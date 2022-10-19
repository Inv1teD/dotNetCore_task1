using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DotNetMentorship.TestAPI
{
    // File name should be the same as class name(rename ModelUkrainian.cs to Ukrainian.cs)
    // Setup primary key for this entity using FluentAPI
    public class Ukrainian
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public bool IsCalm { get; set; }
        
    }
}