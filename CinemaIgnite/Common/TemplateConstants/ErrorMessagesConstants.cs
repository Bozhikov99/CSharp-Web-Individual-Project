namespace Common.ValidationConstants
{
    public class ErrorMessagesConstants
    {
        public const string InvalidLengthMessage = "{0} must be between {2} and {1} characters long";
        public const string PasswordsNotMatchingMessage = "Passwords should match";

        public const string ErrorCreatingGenre = "Error creating a genre";
        public const string ErrorEditingGenre = "Error editing a genre";
        public const string ErrorDeletingGenre = "Error deleting a genre";
        public const string GenreDoesNotExist = "Such genre does not exist";

        public const string ErrorDeletingMovie= "Error deleting a movie";
        public const string ErrorCreatingMovie= "Error creating a movie";
        public const string ErrorEditingMovie= "Error editing a movie";
        public const string MovieDoesNotExist = "Such movie does not exist";

        public const string ErrorDeletingUser= "Unexpected error deleting an user";

        public const string ErrorEditingRoles= "Error editing user roles";

        public const string GenreNameException = "Genre with name {0} already exists";
        public const string MovieNameException = "Movie with name {0} already exists";
    }
}
