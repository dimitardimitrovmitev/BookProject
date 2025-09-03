using BookProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookProject.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            :base(dbContextOptions)
        {
            
        }

        public DbSet<Book> Books { get; set; } = null!;

        public DbSet<Character> Characters { get; set; } = null!;

        public DbSet<Scene> Scenes { get; set; } = null!;

        public DbSet<SceneCharacter> SceneCharacters { get; set; } = null!;

        public DbSet<UserBook> UserBooks { get; set; } = null!;

        public DbSet<CharacterReview> CharacterReviews { get; set; } = null!;

        public DbSet<ImageGeneration> ImageGenerations { get; set; } = null!;

        public DbSet<Report> Reports { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Scene (keep cascade if you want user deletion to remove scenes)
            modelBuilder.Entity<Scene>()
                .HasOne(s => s.User)
                .WithMany(u => u.Scenes)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → ImageGeneration (disable cascade to prevent multiple paths)
            modelBuilder.Entity<ImageGeneration>()
                .HasOne(ig => ig.User)
                .WithMany(u => u.Generations)
                .HasForeignKey(ig => ig.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Scene → ImageGeneration (keep cascade if you want scene deletion to remove generations)
            modelBuilder.Entity<ImageGeneration>()
                .HasOne(ig => ig.Scene)
                .WithMany(s => s.Generations)
                .HasForeignKey(ig => ig.SceneId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
