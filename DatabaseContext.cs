using Microsoft.EntityFrameworkCore;
using FileModel.Models;

public class FileDbContext : DbContext
{
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { }

    public DbSet<FileMetadata> FileMetadata { get; set; }
}
