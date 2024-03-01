using Petsitter.DTOs;
using Petsitter.Models;

namespace Petsitter.Data.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
