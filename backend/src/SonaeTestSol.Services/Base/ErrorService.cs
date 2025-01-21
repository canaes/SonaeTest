using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Domain.Models;
using System.Security.Principal;


namespace SonaeTestSol.Services.Base
{
    public class ErrorService : IErrorService
    {
        private readonly List<Error> _errors;

        public ErrorService() => _errors = new List<Error>();

        public List<Error> Get() => _errors;

        public void Add(Error erro) => _errors.Add(erro);

        public void Clear() => _errors.Clear();

        public bool Exists() => _errors.Any();
    }
}
