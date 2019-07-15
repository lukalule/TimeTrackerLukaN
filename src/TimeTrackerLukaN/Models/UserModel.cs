using TimeTrackerLukaN.Domain;

namespace TimeTrackerLukaN.Models
{
    public class UserModel
    {
        private UserModel()
        {

        }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal HourRate { get; set; }

        public static UserModel FromUser(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                HourRate = user.HourRate
            };
        }
    }
}
