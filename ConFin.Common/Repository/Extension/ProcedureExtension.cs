using System;
using System.Data;
using System.Linq;

namespace ConFin.Common.Repository.Extension
{
    public static class ProcedureExtension
    {
        public static T ReadAttr<T>(this IDataReader r, string attrName)
        {
            try
            {
                if (r[attrName] == DBNull.Value || string.IsNullOrEmpty(r[attrName].ToString()))
                    return default(T);

                var tipoT = typeof(T);
                var tipoR = r[attrName].GetType();

                return (T)(tipoR == tipoT || (tipoT.GetGenericArguments().Any() && tipoR == tipoT.GenericTypeArguments[0])
                    ? r[attrName]
                    : Convert.ChangeType(r[attrName], tipoT));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
