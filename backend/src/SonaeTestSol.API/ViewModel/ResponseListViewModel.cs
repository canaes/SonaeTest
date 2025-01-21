namespace SonaeTestSol.API.ViewModel
{
    public class ResponseListViewModel<T>
    {
        public bool Success { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
