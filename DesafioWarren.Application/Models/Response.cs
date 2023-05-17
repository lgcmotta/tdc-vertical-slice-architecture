using System.Collections.Generic;
using System.Linq;

namespace DesafioWarren.Application.Models
{
    public class Response
    {
        private string _responsePath = string.Empty;

        public object Payload { get; set; }
        
        public List<Failure> Failures { get; } = new();
        
        public Response(object payload)
        {
            Payload = payload;
        }

        public Response()
        {
                
        }

        public void AddValidationFailure(Failure validationFailure) => Failures.Add(validationFailure);
        
        public void AddValidationFailures(IEnumerable<Failure> validationFailures) => Failures.AddRange(validationFailures);

        public void RemoveValidationFailure(Failure validationFailure) => Failures.Remove(validationFailure);

        public void ClearValidationErrors() => Failures.Clear();
        
        public bool IsErrorResponse() => Failures.Any();


        public void SetResponsePath(string responsePath) => _responsePath = responsePath;

        public string GetResponsePath() => _responsePath;
    }
}