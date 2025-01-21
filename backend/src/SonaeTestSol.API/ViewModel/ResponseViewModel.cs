namespace SonaeTestSol.API.ViewModel
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
