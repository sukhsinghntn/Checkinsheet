using NDAProcesses.Shared.Models;
using System.Threading.Tasks;

namespace NDAProcesses.Shared.Services
{
    public interface IEmailService
    {
        Task SendBugReportEmail(EmailModel email);
    }
}
