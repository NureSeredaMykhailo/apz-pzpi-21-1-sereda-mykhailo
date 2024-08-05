using EQueue.Db.Models;

namespace EQueue.Server.Services.Interfaces
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateAsync(User user);
    }
}
