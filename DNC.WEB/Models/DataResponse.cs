using Newtonsoft.Json;
using System.Runtime.Serialization;
namespace DNC.WEB.Models
{
    public class DataResponse
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }
        public string ResultCode { get; set; }

        [DataMember]
        public bool IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }

        [JsonConstructor]
        public DataResponse(string message, object result = null, int statusCode = 200, string resultCode = "00", string apiVersion = "1.0.0.0")
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            Version = apiVersion;
            IsError = false;
            ResultCode = resultCode;
        }
    }
}
