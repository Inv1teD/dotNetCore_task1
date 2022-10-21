using Newtonsoft.Json;
using System.Net;

namespace DotNetMentorship.TestAPI
{
    public class UkrainianBadResponse
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public UkrainianBadResponse()
        {
            this.StatusCode = 400;
            this.StatusDescription = null;
            this.Message = null;
        }

        public UkrainianBadResponse(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        public UkrainianBadResponse(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }

        public class UkrainianValidationError : UkrainianBadResponse
        {
            public UkrainianValidationError()
                : base(500, HttpStatusCode.InternalServerError.ToString())
            {
            }


            public UkrainianValidationError(string message)
                : base(500, HttpStatusCode.InternalServerError.ToString(), message)
            {
            }
        }
    }
}
