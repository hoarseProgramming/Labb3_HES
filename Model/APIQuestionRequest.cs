using System.Text.Json.Serialization;

namespace Labb3_HES.Model
{
    internal class APIQuestionRequest
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }

        [JsonPropertyName("results")]
        public List<Question> ImportedQuestions { get; set; }

        public string ActiveResponseCodeMessage { get; set; }


        public string[] ResponseCodeMessages = new string[]
        {
            "Returned results successfully.",
            "Could not return results. The API doesn't have enough questions for your query. (Ex. Asking for 20 Questions in a Category that only has 10.)",
            "Request contained an invalid parameter. Arguments passed in aren't valid. (Ex. Amount = Five)",
            "Token Not Found: Session Token does not exist.",
            "Token Empty: Session Token has returned all possible questions for the specified query. Resetting the Token is necessary.",
            "Rate Limit: Too many requests have occurred. Each IP can only access the API once every 5 seconds.",
            "Attempting to download questions..."
        };

        public APIQuestionRequest(int responseCode, List<Question> importedQuestions)
        {
            ResponseCode = responseCode;
            ImportedQuestions = importedQuestions;
            ActiveResponseCodeMessage = ResponseCodeMessages[ResponseCode];
        }
    }
}
