namespace SonaeTestSol.API.ViewModel
{
    public class BadRequestViewModel
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
