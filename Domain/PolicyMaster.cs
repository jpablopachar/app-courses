namespace Domain
{
    /// <summary>
    /// Contiene los nombres de políticas utilizados para la autorización en toda la aplicación.
    /// </summary>
    public static class PolicyMaster
    {
        /// <summary>
        /// Política para otorgar acceso de escritura a cursos.
        /// </summary>
        public const string COURSE_WRITE = nameof(COURSE_WRITE);

        /// <summary>
        /// Política para otorgar acceso de lectura a cursos.
        /// </summary>
        public const string COURSE_READ = nameof(COURSE_READ);

        /// <summary>
        /// Política para otorgar acceso de actualización a cursos.
        /// </summary>
        public const string COURSE_UPDATE = nameof(COURSE_UPDATE);

        /// <summary>
        /// Política para otorgar acceso de eliminación a cursos.
        /// </summary>
        public const string COURSE_DELETE = nameof(COURSE_DELETE);

        /// <summary>
        /// Política para otorgar acceso de lectura a instructores.
        /// </summary>
        public const string INSTRUCTOR_READ = nameof(INSTRUCTOR_READ);

        /// <summary>
        /// Política para otorgar acceso de actualización a instructores.
        /// </summary>
        public const string INSTRUCTOR_UPDATE = nameof(INSTRUCTOR_UPDATE);

        /// <summary>
        /// Política para otorgar acceso de creación a instructores.
        /// </summary>
        public const string INSTRUCTOR_CREATE = nameof(INSTRUCTOR_CREATE);

        /// <summary>
        /// Política para otorgar acceso de lectura a comentarios.
        /// </summary>
        public const string COMMENT_READ = nameof(COMMENT_READ);

        /// <summary>
        /// Política para otorgar acceso de eliminación a comentarios.
        /// </summary>
        public const string COMMENT_DELETE = nameof(COMMENT_DELETE);

        /// <summary>
        /// Política para otorgar acceso de creación a comentarios.
        /// </summary>
        public const string COMMENT_CREATE = nameof(COMMENT_CREATE);
    }
}