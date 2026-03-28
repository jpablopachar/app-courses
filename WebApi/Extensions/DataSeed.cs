using Bogus;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;

namespace WebApi.Extensions
{
    /// <summary>
    /// Proporciona métodos para inicializar datos de autenticación y relaciones de cursos en la base de datos.
    /// </summary>
    public static class DataSeed
    {
        /// <summary>
        /// Inicializa los datos de autenticación y relaciones de cursos en la base de datos si no existen.
        /// Crea usuarios predeterminados, asigna instructores, precios y calificaciones a los cursos.
        /// </summary>
        /// <param name="app">Instancia de <see cref="IApplicationBuilder"/> para acceder a los servicios de la aplicación.</param>
        /// <returns>Una tarea que representa la operación asincrónica de inicialización.</returns>
        public static async Task SeedDataAuthentication(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<AppCoursesDbContext>();

                await context.Database.MigrateAsync();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                if (!userManager.Users.Any())
                {
                    await CreateUserAsync(userManager, "Admin User", "admin", "admin@yopmail.com", "Admin123*", CustomRoles.ADMIN);
                    await CreateUserAsync(userManager, "Juan Pachar", "jppachar", "jppachar@yopmail.com", "Client123*", CustomRoles.CLIENT);
                }

                var courses = await context.Courses!.Take(10).Skip(0).ToListAsync();

                if (!context.Set<CourseInstructor>().Any())
                {
                    var instructors = await context.Instructors!.Take(10).Skip(0).ToListAsync();

                    foreach (var course in courses)
                    {
                        course.Instructors = instructors;
                    }
                }

                if (!context.Set<CoursePrice>().Any())
                {
                    var prices = await context.Prices!.ToListAsync();

                    foreach (var course in courses)
                    {
                        course.Prices = prices;
                    }
                }

                if (!context.Set<Qualification>().Any())
                {
                    foreach (var course in courses)
                    {
                        var fakerQualification = new Faker<Qualification>()
                            .RuleFor(q => q.Id, _ => Guid.NewGuid())
                            .RuleFor(q => q.Student, f => f.Name.FullName())
                            .RuleFor(q => q.Comment, f => f.Commerce.ProductDescription())
                            .RuleFor(q => q.Score, 5)
                            .RuleFor(q => q.CourseId, course.Id);

                        var qualifications = fakerQualification.Generate(10);

                        context.AddRange(qualifications);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AppCoursesDbContext>();

                logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Crea un usuario y lo asigna a un rol específico.
        /// </summary>
        /// <param name="userManager">Administrador de usuarios de identidad.</param>
        /// <param name="fullName">Nombre completo del usuario.</param>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <param name="role">Rol a asignar al usuario.</param>
        /// <returns>Una tarea que representa la operación asincrónica de creación y asignación de rol.</returns>
        private static async Task CreateUserAsync(
            UserManager<AppUser> userManager,
            string fullName, string userName, string email,
            string password, string role)
        {
            var user = new AppUser { FullName = fullName, UserName = userName, Email = email };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}