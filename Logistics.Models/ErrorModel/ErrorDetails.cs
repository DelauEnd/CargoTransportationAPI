using Newtonsoft.Json;

namespace Logistics.Entities.ErrorModel
{
    public class ErrorDetails
    {    
        public string Message { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
