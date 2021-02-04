using Microsoft.EntityFrameworkCore;
using PhoneListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneListAPI.Data
{
    public class PhoneListContext : DbContext
    {
        public PhoneListContext(DbContextOptions options) : base(options) { }

        public DbSet<PhoneList> PhoneList { get; set; }
    }
}
