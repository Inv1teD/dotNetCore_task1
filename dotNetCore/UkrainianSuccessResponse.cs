using Newtonsoft.Json;
using System.Net;

namespace DotNetMentorship.TestAPI
{
    public class UkrainianSuccessResponse
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Message { get; private set; }

        public Ukrainian? User { get; private set; }

        public UkrainianSuccessResponse(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        public UkrainianSuccessResponse(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }

        public UkrainianSuccessResponse(int statusCode, string statusDescription, string message, Ukrainian user)
           : this(statusCode, statusDescription)
        {
            this.Message = message;
            this.User = user;
        }

        public class UkrainianObjectCreatedResponse : UkrainianSuccessResponse
        {
            public UkrainianObjectCreatedResponse(string message, Ukrainian CreatedUser)
                : base(200, "Object Created", message, CreatedUser)
            {
            }
        }
    }
}
