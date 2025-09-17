using System;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace API.Data;

public sealed class ApiContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Device> Devices { get; set; }
    
    public ApiContext(DbContextOptions<ApiContext> options)  : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Room>().ToTable("Rooms");
        builder.Entity<Device>().ToTable("Devices");
    }
}