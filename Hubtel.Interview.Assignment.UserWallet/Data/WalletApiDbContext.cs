using Hubtel.Interview.Assignment.UserWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Interview.Assignment.UserWallet.Data;
public class WalletApiDbContext:DbContext
{
    public WalletApiDbContext
    (DbContextOptions<WalletApiDbContext> dbContextOptions): base(dbContextOptions){}

    public virtual DbSet<WalletModel> Wallets { get; set; } = null!;
}