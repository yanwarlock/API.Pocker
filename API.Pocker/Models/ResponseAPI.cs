

namespace API.Pocker.Models
{
    public class ResponseAPI<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
        public T Data { get; set; }

        public ResponseAPI()
        {

        }
        public ResponseAPI(string message = null)
        {
            Message = message;
        }

        public ResponseAPI(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
    }

    public class ResponseAPI:ResponseAPI<object>
    {
        public ResponseAPI()
        {

        }
        public ResponseAPI(object data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ResponseAPI(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public static ResponseAPI Success(string message = null) => new ResponseAPI { Succeeded = true, Message = message };

        public static ResponseAPI Error(string message) => new ResponseAPI(message);


    }
}
