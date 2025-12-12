using Bogus;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence
{
    /// <summary>
    /// Represents the Entity Framework database context for the App Courses application, including identity and domain entities.
    /// </summary>
    /// <remarks>
    /// This context manages the application's data access, entity configuration, and seeding of initial data for courses, instructors, prices, qualifications, and security roles.
    /// </remarks>
    public class AppCoursesDbContext(DbContextOptions<AppCoursesDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Course>? Courses { get; set; }
        public DbSet<Instructor>? Instructors { get; set; }
        public DbSet<Price>? Prices { get; set; }
        public DbSet<Qualification>? Qualifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().ToTable("courses");
            modelBuilder.Entity<Instructor>().ToTable("instructors");
            modelBuilder.Entity<CourseInstructor>().ToTable("course_instructors");
            modelBuilder.Entity<Price>().ToTable("prices");
            modelBuilder.Entity<CoursePrice>().ToTable("course_prices");
            modelBuilder.Entity<Qualification>().ToTable("qualifications");
            modelBuilder.Entity<Photo>().ToTable("photos");

            modelBuilder.Entity<Price>()
                .Property(b => b.CurrentPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Price>()
                .Property(b => b.PromotionalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Price>()
                .Property(b => b.Name)
                .HasColumnType("VARCHAR")
                .HasMaxLength(250);


            modelBuilder.Entity<Course>()
                .HasMany(m => m.Photos)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasMany(m => m.Qualifications)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(m => m.Prices)
                .WithMany(m => m.Courses)
                .UsingEntity<CoursePrice>(
                    j => j
                        .HasOne(p => p.Price)
                        .WithMany(p => p.CoursePrices)
                        .HasForeignKey(p => p.PriceId)

                        ,
                    j => j
                        .HasOne(p => p.Course)
                        .WithMany(p => p.CoursePrices)
                        .HasForeignKey(p => p.CourseId)

                        ,
                    j =>
                    {
                        j.HasKey(t => new { t.PriceId, t.CourseId });
                    }

                );


            modelBuilder.Entity<Course>()
            .HasMany(m => m.Instructors)
            .WithMany(m => m.Courses)
            .UsingEntity<CourseInstructor>(
                j => j
                    .HasOne(p => p.Instructor)
                    .WithMany(p => p.CourseInstructors)
                    .HasForeignKey(p => p.InstructorId)

                    ,
                j => j
                    .HasOne(p => p.Course)
                    .WithMany(p => p.CourseInstructors)
                    .HasForeignKey(p => p.CourseId)
                    ,
                j =>
                {
                    j.HasKey(t => new { t.InstructorId, t.CourseId });
                }
            );


            modelBuilder.Entity<Course>().HasData(LoadMasterData().Item1);
            modelBuilder.Entity<Price>().HasData(LoadMasterData().Item2);
            modelBuilder.Entity<Instructor>().HasData(LoadMasterData().Item3);

            LoadSecurityData(modelBuilder);

        }

        /// <summary>
        /// Seeds the database with security-related data, including roles and role claims for authorization policies.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the model for the context.</param>
        private static void LoadSecurityData(ModelBuilder modelBuilder)
        {
            var adminId = Guid.NewGuid().ToString();
            var clientId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminId,
                    Name = CustomRoles.ADMIN,
                    NormalizedName = CustomRoles.ADMIN
                }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = clientId,
                    Name = CustomRoles.CLIENT,
                    NormalizedName = CustomRoles.CLIENT
                }
            );

            modelBuilder.Entity<IdentityRoleClaim<string>>()
            .HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 1,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COURSE_READ,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 2,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COURSE_UPDATE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 3,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COURSE_WRITE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 4,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COURSE_DELETE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 5,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.INSTRUCTOR_CREATE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 6,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.INSTRUCTOR_READ,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 7,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.INSTRUCTOR_UPDATE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 8,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMMENT_READ,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 9,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMMENT_DELETE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 10,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMMENT_CREATE,
                    RoleId = adminId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 11,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COURSE_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 12,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.INSTRUCTOR_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 13,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMMENT_READ,
                    RoleId = clientId
                },
                new IdentityRoleClaim<string>
                {
                    Id = 14,
                    ClaimType = CustomClaims.POLICIES,
                    ClaimValue = PolicyMaster.COMMENT_CREATE,
                    RoleId = clientId
                }
            );
        }

        /// <summary>
        /// Loads master data for seeding the database with sample courses, prices, and instructors.
        /// </summary>
        /// <returns>A tuple containing arrays of <see cref="Course"/>, <see cref="Price"/>, and <see cref="Instructor"/> objects.</returns>
        private static Tuple<Course[], Price[], Instructor[]> LoadMasterData()
        {
            var courses = new List<Course>();
            var faker = new Faker();

            for (var i = 1; i < 10; i++)
            {
                var courseId = Guid.NewGuid();

                courses.Add(new Course
                {
                    Id = courseId,
                    Title = faker.Lorem.Sentence(5, 5),
                    Description = faker.Lorem.Paragraphs(1, 3),
                    PublicationDate = DateTime.UtcNow
                });
            }

            var priceId = Guid.NewGuid();
            var price = new Price
            {
                Id = priceId,
                CurrentPrice = 10.0m,
                PromotionalPrice = 8.0m,
                Name = "Regular Price",
            };

            var prices = new List<Price> { price };

            var fakerInstructor = new Faker<Instructor>().RuleFor(i => i.Id, _ => Guid.NewGuid())
            .RuleFor(i => i.Name, f => f.Name.FirstName())
            .RuleFor(i => i.LastName, f => f.Name.LastName())
            .RuleFor(i => i.Degree, f => f.Name.JobTitle());

            var instructors = fakerInstructor.Generate(10);

            return Tuple.Create(courses.ToArray(), prices.ToArray(), instructors.ToArray());
        }
    }
}