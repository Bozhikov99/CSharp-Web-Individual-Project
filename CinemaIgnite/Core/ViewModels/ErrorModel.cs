namespace Core.ViewModels
{
    public class ErrorModel
    {
        public string Message { get; init; }

        public ErrorModel(string message)
        {
            Message = message;
        }
    }
}
