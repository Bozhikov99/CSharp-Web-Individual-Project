namespace Common.ValidationConstants
{
    public class ErrorMessagesConstants
    {
        public const string InvalidLengthMessage = "{0} must be between {2} and {1} characters long";
        public const string PasswordsNotMatchingMessage = "Passwords should match";

        public const string InvalidUserName = "{0} must start with an uppercase letter and all other letters should be lowercase";

        public const string ErrorCreatingGenre = "Error creating a genre";
        public const string ErrorEditingGenre = "Error editing a genre";
        public const string ErrorDeletingGenre = "Error deleting a genre";
        public const string GenreDoesNotExist = "Such genre does not exist";

        public const string ErrorDeletingMovie = "Error deleting a movie";
        public const string ErrorCreatingMovie = "Error creating a movie";
        public const string ErrorEditingMovie = "Error editing a movie";
        public const string MovieDoesNotExist = "Such movie does not exist";

        public const string ErrorCreatingProjection = "Неочаквана грешка при създаване на проекция";
        public const string ErrorDeletingProjection = "Неочаквана грешка при изтриване на проекция";
        public const string InvalidDate = "Invalid date";

        public const string SeatTaken = "One or more seats you have chosen are taken";
        public const string ErrorBuyingTicket = "Error buying ticket";

        public const string InvalidLoginData = "Invalid Data";
        public const string ErrorLoggingIn = "Error loging in";
        public const string ErrorRegistering = "Error registering";
        public const string ErrorEditingUser = "Error creating an user";
        public const string ErrorEditingUserRoles = "Error editing user roles";
        public const string ErrorDeletingUser = "Error deleting user";
        public const string ErrorEditingRoles = "Error editing user roles";

        public const string GenreNameException = "Genre {0} already exists";
        public const string MovieNameException = "Movie {0} already exists";
    }
}
