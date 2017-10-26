using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;

namespace Toolshed
{
    public static class ConfigurationHelper
    {
        const string NullEmptyAppSetting = "AppSetting had no value or the key was missing";

        /// <summary>
        /// Get the value of the specified key from AppSettings
        /// </summary>
        /// <param name="key">The key in the appSettings</param>
        /// <param name="throwExceptionOnMissingValue">Indicates whether to throw an exception on a missing or empty value</param>
        /// <returns>A string with the value of the entry or empty of the key is not found and throwExceptionOnMissingValue = false</returns>
        /// <exception cref="">ArgumentNullException (if throwExceptionOnMissingValue = true)</exception>
        private static string GetAppSetting(string key, bool throwExceptionOnMissingValue)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            if (throwExceptionOnMissingValue && string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(key, NullEmptyAppSetting);
            }

            return value;
        }

        /// <summary>
        /// Returns the value of the specified key from the AppSetting in the config file
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <returns>A string representing the value found</returns>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if no value is found.</exception>
        public static string GetAppSetting(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(key, NullEmptyAppSetting);
            }

            return value;
        }

        /// <summary>
        /// Returns the value of the specfied key from the AppSetting in the config file or the default value provided if no entry is found
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="defaultValue">The default value to return if no value or entry is found. This can be null.</param>
        /// <returns>A string representing the value found</returns>
        /// <exception cref="ArgumentNullException">An ArgumentNullException is thrown if no value is found and no default value specified.</exception>
        public static string GetAppSetting(string key, string defaultValue)
        {
            var value = GetAppSetting(key, false);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Returns the value of the specified key from the AppSetting in the config file or the default value provided if no entry is found. If value or default value is provided an exception will be thrown.
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="defaultValue">The default value to return if no value or entry is found. This can be null.</param>
        /// <returns>A boolean representing the value found</returns>
        public static bool GetAppSetting(string key, bool? defaultValue = null)
        {
            var val = GetInternalAppSetting(key, defaultValue.HasValue);
            return (string.IsNullOrEmpty(val)) ? defaultValue.Value : Convert.ToBoolean(val);
        }

        /// <summary>
        /// Returns the value of the specified key from the AppSetting or the default value provided if no entry is found. If no value or default value is provided an exception will be thrown.
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="defaultValue">The default value to return if no value or entry is found. This can be null.</param>
        /// <returns>An int representing the value found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int GetAppSetting(string key, int? defaultValue = null)
        {
            var val = GetInternalAppSetting(key, defaultValue.HasValue);
            return (string.IsNullOrEmpty(val)) ? defaultValue.Value : Convert.ToInt32(val);
        }

        /// <summary>
        /// Returns the value of the specified key from the AppSetting or the default value provided if no entry is found. If no value or default value is provided an exception will be thrown.
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="defaultValue">The default value to return if no value or entry is found. This can be null.</param>
        /// <returns>A long representing the value found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static long GetAppSetting(string key, long? defaultValue = null)
        {
            var val = GetInternalAppSetting(key, defaultValue.HasValue);
            return (string.IsNullOrEmpty(val)) ? defaultValue.Value : Convert.ToInt64(val);
        }

        /// <summary>
        /// Returns the value of the specified key from the AppSetting or the default value provided if no entry is found. If no value or default value is provided an exception will be thrown.
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="defaultValue">The default value to return if no value or entry is found. This can be null.</param>
        /// <returns>A double representing the value found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double GetAppSetting(string key, double? defaultValue = null)
        {
            var val = GetInternalAppSetting(key, defaultValue.HasValue);
            return (string.IsNullOrEmpty(val)) ? defaultValue.Value : Convert.ToDouble(val);
        }

        /// <summary>
        /// Returns the value of the specified key from the AppSetting or the default value provided if no entry is found. If no value or default value is provided an exception will be thrown.
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="defaultValue">The default value to return if no value or entry is found. This can be null.</param>
        /// <returns>A DateTime representing the value found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DateTime GetAppSetting(string key, DateTime? defaultValue = null)
        {
            var val = GetInternalAppSetting(key, defaultValue.HasValue);
            return (string.IsNullOrEmpty(val)) ? defaultValue.Value : Convert.ToDateTime(val);
        }


        /// <summary>
        /// Returns a string[] of the delimited values with specified key from the AppSetting.
        /// </summary>
        /// <param name="key">The key of the AppSetting entry</param>
        /// <param name="delimiter">The delimiter to use to convert the value into a string. Default is a comma (,)</param>
        /// <returns>A string array (string[]) or null if no value is found</returns>
        public static string[] GetAppSettingStringArray(string appSettingName, char delimiter = ',', StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            string s = GetAppSetting(appSettingName);
            if (!string.IsNullOrWhiteSpace(s))
            {
                if (s.IndexOf(delimiter) > 0)
                {
                    return s.Split(new char[] { delimiter }, options);
                }
                else
                {
                    return new string[1] { s };
                }
            }

            return null;
        }


        /// <summary>
        /// Returns a string value of the matching key in the provided NameValueCollection.
        /// </summary>
        /// <exception cref="">Throws an exception if a matching key is not found and no default value provided</exception>
        /// <param name="config"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        public static string GetStringValue(NameValueCollection config, string valueName, string defaultValue = "")
        {
            //TODO: does the exception comments show and whats the error
            var value = config[valueName];



            if (string.IsNullOrEmpty(valueName) && string.IsNullOrWhiteSpace(defaultValue))
            {
                throw new ProviderException(valueName + " must have a value");
            }

            if (string.IsNullOrEmpty(valueName))
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Returns a boolean value of the matching key in the provided NameValueCollection.
        /// </summary>
        /// <exception cref="">Throws an exception no default value is provided and a matching key is not found or cannot be converted to boolean.</exception>
        /// <param name="config"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        public static bool GetBooleanValue(NameValueCollection config, string valueName, bool? defaultValue = null)
        {
            string sValue = config[valueName];

            if (sValue == null)
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                else
                {

                }
            }

            bool result;
            if (bool.TryParse(sValue, out result))
            {
                return result;
            }
            else
            {
                throw new ConfigurationErrorsException(valueName + " must be boolean");
            }
        }

        /// <summary>
        /// Returns an int value of the matching key in the provided NameValueCollection.
        /// </summary>
        /// <exception cref="">Throws an exception no default value is provided and a matching key is not found or cannot be converted to int.</exception>
        /// <param name="config"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        public static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, int minValueAllowed = 0, int maxValueAllowed = Int32.MaxValue)
        {
            string sValue = config[valueName];

            if (sValue == null)
            {
                return defaultValue;
            }

            int v;
            if (!Int32.TryParse(sValue, out v))
            {
                throw new ProviderException(valueName + " must be a number");
            }

            if (v < minValueAllowed || v > maxValueAllowed)
            {
                throw new ConfigurationErrorsException(string.Format("{0} must be > than {1} and < {2}", valueName, minValueAllowed, maxValueAllowed));
            }

            return v;
        }

        /// <summary>
        /// Returns the connectionstring value using the key provided
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultKeyValue"></param>
        public static string GetConnectionStringValue(string key, string defaultKeyValue = null)
        {
            var value = ConfigurationManager.ConnectionStrings[key];
            if (value == null)
            {
                if (!string.IsNullOrEmpty(defaultKeyValue))
                {
                    return GetConnectionStringValue(defaultKeyValue);
                }

                throw new ArgumentNullException(key, "The specified key for the connection string was not found");
            }


            if (string.IsNullOrEmpty(value.ConnectionString))
            {
                throw new ArgumentNullException(key, "The value for the specified key for the connection string was empty");
            }

            return value.ConnectionString;
        }


        private static string GetInternalAppSetting(string key, bool hasValue)
        {
            var val = GetAppSetting(key, false);
            if (string.IsNullOrEmpty(val) && !hasValue)
            {
                throw new ProviderException("No AppSetting with a key of " + key + " or it has no value. The key must have a value or a default provided");
            }

            return val;
        }
    }
}