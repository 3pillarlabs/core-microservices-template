namespace Core.Services.Entities
{
    public class ResponseMessage 
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
        public string FriendlyMessage { get; set; }
        public string MessageType { get; set; }
        public string StackTrace { get; set; }
    }
}