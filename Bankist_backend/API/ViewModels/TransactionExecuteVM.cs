using API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Endpoints.TransactionEndpoints.Execute
{
    public class TransactionExecuteVM
    {
        public DateTime transactionDate { get; set; }
        public float amount { get; set; }
        public string type { get; set; }
        public string status { get; set; }

        public int senderCardId { get; set; }
        
        public int recieverCardId { get; set; }
        }
}
