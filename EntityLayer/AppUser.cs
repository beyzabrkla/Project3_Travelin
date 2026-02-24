using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EntityLayer
{
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Role { get; set; } = "Customer"; // "Admin" | "Guide" | "Customer"
        public string GuideId { get; set; }            // Role == "Guide" ise bağlı Guide kaydı
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
