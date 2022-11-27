namespace AuctionExpert.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "AuctionExpert";

        public const string AdministratorRoleName = "Administrator";

        public class RegisterConstraintsAndMessages
        {
            public const int FirstNameMaxLenth = 20;

            public const int FirstNameMinLength = 3;

            public const int LastNameMaxLenth = 20;

            public const int LastNameMinLenth = 3;

            public const int UsernameMaxLength = 15;

            public const int UsernameMinLength = 2;

            public const int PasswordMaxLength = 100;

            public const int PasswordMinLength = 6;

            public const int AgeMinLength = 18;

            public const int AgeMaxLength = 100;

            public const string UsernameTaken = "This username is already taken.";

            public const string EmailTaken = "This Email adress is already taken.";

            public const string RangeMessage = "The {0} must be at least {2} and maximum {1} characters long!";

            public const string PasswordDoesNotMatchMessage = "The password and confirmation password do not match!";

            public const string AgeMinMessage = "You cannot register until you are 18 years old!";

            public const string AgeMaxMessage = "You should be dead!";
        }

        public class AuctionConstraintsAndMessages
        {
            public const int TitleMinLenght = 10;

            public const int TitleMaxLenght = 50;

            public const int DurationMaxDays = 7;

            public const int DescriptionMinLength = 20;

            public const int DescriptionMaxLength = 2000;

            public const string LengthMessage = "The {0} must be at least {2} and maximum {1} characters long!";

            public const string AuctionDoesNotExist = "The auction you are looking for does not exist!";

            public const string InvalidInputValueForBid = "The value you entered is invalid!";

            public const string InputBidIsLessThanLastOne = "You cannot place bid that is less than the last one!";

            public const string InputBidIsLessThanAuctionStartPrice = "Your bid should be higher than the auction's minimum step!";
        }

        public class FilePaths
        {
            public const string DirectoryUp = "..";

            public const string CountriesWithCitiesFileName = "CountriesWithCities.json";

            public const string DataFolder = "Data";

            public const string SystemNameData = $"{SystemName}.{DataFolder}";

            public const string FilesFolder = "Files";
        }
    }
}
