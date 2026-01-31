using Microsoft.EntityFrameworkCore;

namespace Insure.Context
{
    public class MyApplicationDbContext : DbContext
    {
        public MyApplicationDbContext(DbContextOptions<MyApplicationDbContext> options) : base(options) { }
    }
}
