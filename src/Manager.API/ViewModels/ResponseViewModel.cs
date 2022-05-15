namespace Manager.API.ViewModels
{
    public class ResponseViewModel
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public dynamic Data { get; set; } = null;
    }
}
