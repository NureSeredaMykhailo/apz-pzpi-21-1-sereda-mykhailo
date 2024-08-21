using Microsoft.AspNetCore.Identity;

namespace EQueue.Db.Models
{
    public class User : IdentityUser<long>, IEntity
    {
    }
}
