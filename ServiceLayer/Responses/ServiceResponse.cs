namespace ServiceLayer.Responses
{
    public enum Status
    {
        Ok,
        BadRequest,
        Error
    }
    public class ServiceResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }
    }
}