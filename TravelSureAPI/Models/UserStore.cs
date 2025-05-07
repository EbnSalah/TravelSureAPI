namespace TravelSureAPI.Models
{
    public static class UserStore
    {
        //Create Registered Users List
        private static List<UserAccount> _accounts = new List<UserAccount>();

        //Return All Users (accounts)
        public static List<UserAccount> GetAllUser()
        {
            return _accounts;

        }

        //Method for "Adding New User"
        public static void AddUser(UserAccount user)
        {
            _accounts.Add(user);
        }

        //Method for "Searching about User By Email"
        public static UserAccount GetUserByEmail(string email)
        {
            return _accounts.FirstOrDefault(u => u.Email == email);
        }

        //Method for "Checking if UserName Exists already"
        public static bool UserNameExists(string userName)
        {
            return _accounts.Any(u => u.UserName == userName);

        }
    }
}
