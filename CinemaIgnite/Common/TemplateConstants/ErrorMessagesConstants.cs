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

        public const string ErrorDeletingMovie = "Error deleting a movie";
        public const string ErrorCreatingMovie = "Error creating a movie";
        public const string ErrorEditingMovie = "Error editing a movie";
        public const string MovieDoesNotExist = "Such movie does not exist";

        public const string ErrorCreatingProjection = "Неочаквана грешка при създаване на проекция";
        public const string ErrorDeletingProjection = "Неочаквана грешка при изтриване на проекция";
        public const string InvalidDate = "Invalid date";

        public const string SeatTaken = "One or more seats you have chosen are taken";
        public const string ErrorBuyingTicket = "Error buying ticket";

        public const string InvalidLoginData = "Невалидни данни";
        public const string ErrorLoggingIn = "Неочаква грешка при вписване";
        public const string ErrorRegistering = "Неочаква грешка при регистрация";
        public const string ErrorEditingUser = "Неочаквана грешка при редактиране на потребител";
        public const string ErrorEditingUserRoles = "Неочаквана грешка при задаване на потребителски роли";
        public const string ErrorDeletingUser = "Неочаквана грешка при изтриване на потребител";
        public const string ErrorEditingRoles = "Error editing user roles";

        public const string GenreNameException = "Жанр с име {0} вече съществува";
        public const string MovieNameException = "Филм с име {0} вече съществува";
    }
}
