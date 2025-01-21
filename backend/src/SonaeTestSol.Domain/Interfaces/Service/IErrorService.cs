

using SonaeTestSol.Domain.Models;

namespace SonaeTestSol.Domain.Interfaces.Service
{
    public interface IErrorService
    {
        void Add(Error erro);
        void Clear();
        bool Exists();
        List<Error> Get();
    }
}