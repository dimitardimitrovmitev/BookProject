using BookProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookProject.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        // Entities
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<Scene> Scenes { get; set; } = null!;
        public DbSet<SceneCharacter> SceneCharacters { get; set; } = null!;
        public DbSet<UserBook> UserBooks { get; set; } = null!;
        public DbSet<BookReview> BookReviews { get; set; } = null!;        // ← add this
        public DbSet<CharacterReview> CharacterReviews { get; set; } = null!;
        public DbSet<ImageGeneration> ImageGenerations { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<ImageGenerationCharacter> ImageGenerationCharacters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER → SCENE
            modelBuilder.Entity<Scene>()
                .HasOne(s => s.User)
                .WithMany(u => u.Scenes)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER → IMAGE GENERATION
            modelBuilder.Entity<ImageGeneration>()
                .HasOne(ig => ig.User)
                .WithMany(u => u.Generations)
                .HasForeignKey(ig => ig.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // SCENE → IMAGE GENERATION
            modelBuilder.Entity<ImageGeneration>()
                .HasOne(ig => ig.Scene)
                .WithMany(s => s.Generations)
                .HasForeignKey(ig => ig.SceneId)
                .OnDelete(DeleteBehavior.Cascade);

            // BOOK → CHARACTER (one-to-many)
            modelBuilder.Entity<Character>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Characters)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // SCENECHARACTER many-to-many
            modelBuilder.Entity<SceneCharacter>()
                .HasKey(sc => new { sc.SceneId, sc.CharacterId }); // composite key

            modelBuilder.Entity<SceneCharacter>()
                .HasOne(sc => sc.Scene)
                .WithMany(s => s.SceneCharacters)
                .HasForeignKey(sc => sc.SceneId);

            modelBuilder.Entity<SceneCharacter>()
                .HasOne(sc => sc.Character)
                .WithMany()
                .HasForeignKey(sc => sc.CharacterId);

            // USERBOOK (many-to-many join with composite key)
            modelBuilder.Entity<UserBook>()
                .HasKey(ub => new { ub.UserId, ub.BookId });

            modelBuilder.Entity<UserBook>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBooks)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBook>()
                .HasOne(ub => ub.Book)
                .WithMany()
                .HasForeignKey(ub => ub.BookId);

            // BOOKREVIEW (one per user per book)
            modelBuilder.Entity<BookReview>()
                .HasIndex(br => new { br.UserId, br.BookId })
                .IsUnique();

            // CHARACTERREVIEW (one per user per character)
            modelBuilder.Entity<CharacterReview>()
                .HasIndex(cr => new { cr.UserId, cr.CharacterId })
                .IsUnique();



            modelBuilder.Entity<ImageGenerationCharacter>()
                .HasKey(igc => new { igc.ImageGenerationId, igc.CharacterId });

            modelBuilder.Entity<ImageGenerationCharacter>()
                .HasOne(igc => igc.ImageGeneration)
                .WithMany(ig => ig.ImageGenerationCharacters)
                .HasForeignKey(igc => igc.ImageGenerationId);

            modelBuilder.Entity<ImageGenerationCharacter>()
                .HasOne(igc => igc.Character)
                .WithMany()
                .HasForeignKey(igc => igc.CharacterId);
        }
    }
}