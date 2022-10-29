﻿namespace AuctionExpert.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "AuctionExpert";

        public const string AdministratorRoleName = "Administrator";

        public class RegisterConstraints
        {
            public const int UsernameMaxLength = 15;

            public const int UsernameMinLength = 2;

            public const int PasswordMaxLength = 100;

            public const int PasswordMinLength = 6;

            public const string UsernameTaken = "This username is already taken.";

            public const string EmailTaken = "This Email adress is already taken.";
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