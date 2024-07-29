using StarBucks_Backend.Services;
using StarBucks_Backend.Models;
using System.Data;

namespace StarBucks_Backend.Implementations
{
    public class AuthImplementation: IAuthService
    {
        private readonly string _usersFilePath = "D:\\project 1\\.vs\\StarBucks Project\\users.txt";

        public void SignUp(User user)
        {
            var users = LoadUsers().ToList();

            // Check if the user already exists
            if (users.Any(u => u.UserName == user.UserName))
            {
                throw new System.Exception("User already exists.");
            }

            // Create a new user and append to the CSV file
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role
            };

            using (var writer = new StreamWriter(_usersFilePath, true))
            {
                writer.WriteLine($"{newUser.FirstName},{newUser.LastName},{newUser.UserName},{newUser.Password},{newUser.Role}");
            }
        }

        // Login method to authenticate users
        public bool Login(string userName, string password, out string role)
        {
            role = null;
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (user != null)
            {
                role = user.Role;
                return true;
            }
            return false;
        }

        // Helper method to load users from the CSV file
        private IEnumerable<User> LoadUsers()
        {
            var users = new List<User>();
            using (var reader = new StreamReader(_usersFilePath))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var values = line?.Split(',');

                    if (values?.Length != 5)
                    {
                        continue;
                    }

                    users.Add(new User
                    {
                        FirstName = values[0],
                        LastName = values[1],
                        UserName = values[2],
                        Password = values[3],
                        Role = values[4]
                    });
                }
            }
            return users;
        }
    }
}