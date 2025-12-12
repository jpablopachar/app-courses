namespace Domain
{
    /// <summary>
    /// Contains policy names used for authorization throughout the application.
    /// </summary>
    public static class PolicyMaster
    {
        /// <summary>
        /// Policy for granting write access to courses.
        /// </summary>
        public const string COURSE_WRITE = nameof(COURSE_WRITE);

        /// <summary>
        /// Policy for granting read access to courses.
        /// </summary>
        public const string COURSE_READ = nameof(COURSE_READ);

        /// <summary>
        /// Policy for granting update access to courses.
        /// </summary>
        public const string COURSE_UPDATE = nameof(COURSE_UPDATE);

        /// <summary>
        /// Policy for granting delete access to courses.
        /// </summary>
        public const string COURSE_DELETE = nameof(COURSE_DELETE);

        /// <summary>
        /// Policy for granting read access to instructors.
        /// </summary>
        public const string INSTRUCTOR_READ = nameof(INSTRUCTOR_READ);

        /// <summary>
        /// Policy for granting update access to instructors.
        /// </summary>
        public const string INSTRUCTOR_UPDATE = nameof(INSTRUCTOR_UPDATE);

        /// <summary>
        /// Policy for granting create access to instructors.
        /// </summary>
        public const string INSTRUCTOR_CREATE = nameof(INSTRUCTOR_CREATE);

        /// <summary>
        /// Policy for granting read access to comments.
        /// </summary>
        public const string COMMENT_READ = nameof(COMMENT_READ);

        /// <summary>
        /// Policy for granting delete access to comments.
        /// </summary>
        public const string COMMENT_DELETE = nameof(COMMENT_DELETE);

        /// <summary>
        /// Policy for granting create access to comments.
        /// </summary>
        public const string COMMENT_CREATE = nameof(COMMENT_CREATE);
    }
}