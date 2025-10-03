using System.Threading.Tasks;

namespace NDAProcesses.Server.Services
{
    public interface INotificationEmailStore
    {
        Task<string?> GetAsync();
        Task SetAsync(string? email);
    }
}
