using backend_template.Model;

namespace backend_template;

public static class UserRepository
{
    private static List<User> users = new List<User>()
        {
            new User { Id = 1, Username = "user1", Password = "password1" },
            new User { Id = 2, Username = "user2", Password = "password2" }
        };

    public static void AddUser(User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);
    }

    public static User? GetByUsername(string? username)
    {
        return users.FirstOrDefault(u => u.Username == username);
    }

    public static User? GetByUserId(int? id)
    {
        return users.FirstOrDefault(u => u.Id == id);
    }
}

