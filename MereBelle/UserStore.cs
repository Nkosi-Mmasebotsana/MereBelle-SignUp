using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MereBelle
{
    public static class UserStore
    {
        private static readonly string FilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");

        private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

        public static List<User> LoadAll()
        {
            if (!File.Exists(FilePath)) return new List<User>();
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private static void SaveAll(List<User> users) =>
            File.WriteAllText(FilePath, JsonSerializer.Serialize(users, JsonOptions));

        public static bool EmailExists(string email) =>
            LoadAll().Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public static void AddUser(User user)
        {
            var users = LoadAll();
            users.Add(user);
            SaveAll(users);
        }

        public static User? FindByEmail(string email) =>
            LoadAll().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public static bool ConfirmUser(string email, string code)
        {
            var users = LoadAll();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null || user.ConfirmationCode != code) return false;

            user.IsConfirmed = true;
            user.ConfirmationCode = ""; // burn the code after use
            SaveAll(users);
            return true;
        }
    }
}