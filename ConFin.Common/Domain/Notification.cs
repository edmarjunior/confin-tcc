using System;
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
            _notifications = _errors = new List<string>();
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

        // * * * * * * * * * * * * * * * O L D * * * * * * * * * * * * * *
        private readonly List<string> _errors;

        [Obsolete("Use \".Any\"")]
        public bool HasErrors => _errors.Any();

        [Obsolete("Use \".Get\"")]
        public IEnumerable<string> GetErrors()
        {
            return _errors;
        }

        [Obsolete("Use \".Add()\"")]
        public void AddError(string error)
        {
            _errors.Add(error);
        }

        [Obsolete("Use \".Add()\"")]
        public void AddErrors(IEnumerable<string> errors)
        {
            _errors.AddRange(errors);
        }
    }
}
