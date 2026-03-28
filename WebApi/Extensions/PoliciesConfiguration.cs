using Domain;

namespace WebApi.Extensions
{
    /// <summary>
    /// Clase de extensión para configurar las políticas de autorización de la aplicación.
    /// </summary>
    public static class PoliciesConfiguration
    {
        /// <summary>
        /// Agrega los servicios de autorización configurando todas las políticas de acceso necesarias.
        /// Define políticas para los recursos de cursos, instructores y comentarios con operaciones de lectura, escritura, actualización y eliminación.
        /// </summary>
        /// <param name="services">La colección de servicios de inyección de dependencias.</param>
        /// <returns>La colección de servicios configurada para permitir encadenamiento de métodos.</returns>
        public static IServiceCollection AddPoliciesServices(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(PolicyMaster.COURSE_READ, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COURSE_READ)))
                .AddPolicy(PolicyMaster.COURSE_WRITE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COURSE_WRITE)))
                .AddPolicy(PolicyMaster.COURSE_UPDATE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COURSE_UPDATE)))
                .AddPolicy(PolicyMaster.COURSE_DELETE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COURSE_DELETE)))
                .AddPolicy(PolicyMaster.INSTRUCTOR_READ, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.INSTRUCTOR_READ)))
                .AddPolicy(PolicyMaster.INSTRUCTOR_CREATE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.INSTRUCTOR_CREATE)))
                .AddPolicy(PolicyMaster.INSTRUCTOR_UPDATE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.INSTRUCTOR_UPDATE)))
                .AddPolicy(PolicyMaster.COMMENT_READ, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COMMENT_READ)))
                .AddPolicy(PolicyMaster.COMMENT_CREATE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COMMENT_CREATE)))
                .AddPolicy(PolicyMaster.COMMENT_DELETE, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COMMENT_DELETE)));

            return services;
        }
    }
}