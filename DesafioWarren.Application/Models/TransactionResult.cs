using System;

namespace DesafioWarren.Application.Models
{
    public class TransactionResult
    {
        public string Status { get; set; }

        public DateTime Occurrence { get; set; }

        public string Message { get; set; }

        public TransactionResult(string status, DateTime occurrence, string message)
        {
            Status = status;
            Occurrence = occurrence;
            Message = message;
        }
    }
}