using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;

namespace _4Dorms.Repositories.implementation
{
    public class SignUpService  : ISignUpService
    {
        private readonly IGenericRepository<SignUp> _genericRepository;
    }
}
