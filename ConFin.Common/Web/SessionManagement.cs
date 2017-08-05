using System.Web;

namespace ConFin.Common.Web
{
    public static class SessionManagement
    {
        public static T Get<T>(string sessionName)
        {
            return (T)HttpContext.Current.Session[sessionName];
        }

        public static T GetWithDefault<T>(string sessionName)
        {
            var value = HttpContext.Current.Session[sessionName];
            return value == null ? default(T) : (T)value;
        }

        public static void Update(string sessionName, object value)
        {
            HttpContext.Current.Session[sessionName] = value;
            if (value == null)
                Remove(sessionName);
        }

        public static void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }

        public static void Remove(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }

        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
