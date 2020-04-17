using System.Collections.Generic;
using System.Linq;

namespace ConFin.Common.Domain
{
    public class Notification
    {
        private readonly List<string> _notifications;
        public bool Any => _notifications.Any();
        public IEnumerable<string> Get => _notifications;

        public Notification()
        {
            _notifications = new List<string>();
        }

        public void Add(params string[] notification)
        {
            _notifications.AddRange(notification.Where(x => !string.IsNullOrEmpty(x?.Trim())));
        }

        public T AddWithReturn<T>(params string[] notification)
        {
            Add(notification);
            return default(T);
        }
    }
}
