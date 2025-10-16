namespace V9MvcCoreProject.Helpers;

public class WebResponse<T> //where T : class (When you use Generic type response
{
    public T Value { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }

    public static WebResponse<T> Success(T value, string message = "Success", bool isSuccess = true)
        => new WebResponse<T> { Value = value, Message = message, IsSuccess = isSuccess };

    public static WebResponse<T> Failed(string message = "Error", bool isSuccess = false)
        => new WebResponse<T> { Message = message, IsSuccess = isSuccess };
}


public class GenericServiceResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}