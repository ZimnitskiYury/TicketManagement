using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<Purchase> Purchases { get; set; }

        public virtual DbSet<Area> Areas { get; set; }

        public virtual DbSet<EventData> Events { get; set; }

        public virtual DbSet<EventArea> EventAreas { get; set; }

        public virtual DbSet<EventSeat> EventSeats { get; set; }

        public virtual DbSet<LayoutData> Layouts { get; set; }

        public virtual DbSet<Seat> Seats { get; set; }

        public virtual DbSet<Venue> Venues { get; set; }
        }
    }
