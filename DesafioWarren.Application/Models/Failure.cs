namespace DesafioWarren.Application.Models
{
    public class Failure
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }

        public Failure(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public Failure()
        {
            
        }
        
    }
}