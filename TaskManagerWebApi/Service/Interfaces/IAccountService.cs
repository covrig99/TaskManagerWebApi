using TaskManagerWebApi.DataAccessLayer.Interfaces;

namespace TaskManagerWebApi.Service.Interfaces
{
    public interface IAccountService
    {
        bool Register(RegisterViewModel viewModel);
    }
}
