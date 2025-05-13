using TRACE.Context;
using TRACE.Models;

namespace TRACE.Helpers
{
    public class EventLogger
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public EventLogger(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
        }

        public async Task LogEventAsync(string action, string source, string category)
        {
            var currentUserName = _currentUserHelper.Email;
            var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

            if (user == null)
            {
                // Handle case when user is not found
                throw new Exception("Current user not found.");
            }

            var eventLog = new EventLog
            {
                EventDatetime = DateTime.Now,
                UserId = user.Username,
                Event = action,
                Source = source,
                Category = category
            };

            _context.EventLogs.Add(eventLog);
            await _context.SaveChangesAsync();
        }
    }
}
