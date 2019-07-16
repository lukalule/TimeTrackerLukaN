using TimeTrackerLukaN.Domain;

namespace TimeTrackerLukaN.Models
{
    /// <summary>
    /// Represents one time tracker user.
    /// </summary>
    public class UserModel
    {
        private UserModel()
        {

        }

        /// <summary>
        /// gets or sets user Id.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// gets or sets user Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// gets or sets how much the user will be paid per hour.
        /// </summary>
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
