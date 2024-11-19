using System;
using System.IO;
using System.Text;
using System.Linq;

namespace Utilities
{
    public static class Tools
    {
        /// <summary>
        /// Converts a string to title case (each word starts with an uppercase letter).
        /// </summary>
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return string.Join(" ", input.Split(' ')
                .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }

        /// <summary>
        /// Checks if a given string is a palindrome.
        /// </summary>
        public static bool IsPalindrome(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var cleaned = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            return cleaned.SequenceEqual(cleaned.Reverse());
        }

        /// <summary>
        /// Generates a random alphanumeric string of a given length.
        /// </summary>
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Safely parses a string into an integer, returning a default value on failure.
        /// </summary>
        public static int SafeParseInt(string input, int defaultValue = 0)
        {
            return int.TryParse(input, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// Writes a message to a log file with a timestamp.
        /// </summary>
        public static void WriteToLog(string message, string logFilePath = "log.txt")
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates the factorial of a given number.
        /// </summary>
        public static long Factorial(int number)
        {
            if (number < 0)
                throw new ArgumentException("Number must be non-negative");

            return number == 0 ? 1 : number * Factorial(number - 1);
        }

        /// <summary>
        /// Converts a byte size into a human-readable format (e.g., KB, MB).
        /// </summary>
        public static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
