// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace BAMApp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        //const string NameKey = "name";
        //static readonly string NameDefault = string.Empty;

        //const string EmailKey = "email";
        //static readonly string EmailDefault = string.Empty;

        //const string PhoneNumberKey = "phonenumber";
        //static readonly string PhoneNumberDefault = string.Empty;

        //const string ZipCodeKey = "zipcode";
        //static readonly string ZipCodeDefault = string.Empty;

        //const string GenderKey = "gender";
        //static readonly string GenderDefault = string.Empty;

        //const string BirthdayKey = "Birthday";
        //static readonly DateTime BirthdayDefault = DateTime.Now.AddYears(-18);

        //const string LastSyncKey = "last_sync";
        //static readonly DateTime LastSyncDefault = DateTime.Now.AddDays(-30);

        const string LoginAttemptsKey = "login_attempts";
        const int LoginAttemptsDefault = 0;

        //const string NeedsSyncKey = "needs_sync";
        //const bool NeedsSyncDefault = true;

        //const string HasSyncedDataKey = "has_synced";
        //const bool HasSyncedDataDefault = false;

        #endregion


        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(UserIdKey, value);
            }
        }
        public static string AuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(AuthTokenKey, value);
            }
        }
        //public static string Name
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<string>(NameKey, NameDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<string>(NameKey, value);
        //    }
        //}
        //public static string Email
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<string>(EmailKey, EmailDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<string>(EmailKey, value);
        //    }
        //}
        //public static string PhoneNumber
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<string>(PhoneNumberKey, PhoneNumberDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<string>(PhoneNumberKey, value);
        //    }
        //}
        //public static string ZipCode
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<string>(ZipCodeKey, ZipCodeDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<string>(ZipCodeKey, value);
        //    }
        //}
        //public static string Gender
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<string>(GenderKey, GenderDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<string>(GenderKey, value);
        //    }
        //}
        //public static DateTime Birthday
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<DateTime>(BirthdayKey, BirthdayDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<DateTime>(BirthdayKey, value);
        //    }
        //}
        public static int LoginAttempts
        {
            get
            {
                return AppSettings.GetValueOrDefault<int>(LoginAttemptsKey, LoginAttemptsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(LoginAttemptsKey, value);
            }
        }
#if DEBUG
        //public static bool NeedsSync
        //{
        //    get { return true; }
        //    set { }
        //}
#else
        public static bool NeedsSync
        {
        get { return AppSettings.GetValueOrDefault<bool>(NeedsSyncKey, NeedsSyncDefault) || LastSync < DateTime.Now.AddDays(-1); }
        set { AppSettings.AddOrUpdateValue<bool>(NeedsSyncKey, value); }

        }
#endif
        //public static bool HasSyncedData
        //{
        //    get { return AppSettings.GetValueOrDefault<bool>(HasSyncedDataKey, HasSyncedDataDefault); }
        //    set { AppSettings.AddOrUpdateValue<bool>(HasSyncedDataKey, value); }

        //}
        //public static DateTime LastSync
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault<DateTime>(LastSyncKey, LastSyncDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue<DateTime>(LastSyncKey, value);
        //    }
        //}
        public static bool IsLoggedIn { get { return !string.IsNullOrWhiteSpace(UserId); } }
    }
}