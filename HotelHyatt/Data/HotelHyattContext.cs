using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelHyatt.Data
{
    public class HotelHyattContext : IdentityDbContext
    {
        public HotelHyattContext(DbContextOptions<HotelHyattContext> options)
            : base(options)
        {
        }
        public DbSet<HotelHyatt.Models.Room> Movie { get; set; } = default!;
        public DbSet<HotelHyatt.Models.Booking> Booking { get; set; } = default!;
    }
}
