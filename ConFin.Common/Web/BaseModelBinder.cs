using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ConFin.Common.Web
{
    public static class BaseModelBinder
    {
        public static void Init()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder<DateTime>());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder<DateTime?>());
            ModelBinders.Binders.Add(typeof(byte), new NumericModelBinder<byte>());
            ModelBinders.Binders.Add(typeof(byte?), new NumericModelBinder<byte?>());
            ModelBinders.Binders.Add(typeof(short), new NumericModelBinder<short>());
            ModelBinders.Binders.Add(typeof(short?), new NumericModelBinder<short?>());
            ModelBinders.Binders.Add(typeof(int), new NumericModelBinder<int>());
            ModelBinders.Binders.Add(typeof(int?), new NumericModelBinder<int?>());
            ModelBinders.Binders.Add(typeof(long), new NumericModelBinder<long>());
            ModelBinders.Binders.Add(typeof(long?), new NumericModelBinder<long?>());
            ModelBinders.Binders.Add(typeof(decimal), new NumericModelBinder<decimal>());
            ModelBinders.Binders.Add(typeof(decimal?), new NumericModelBinder<decimal?>());
        }
    }

    public class DateTimeModelBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (string.IsNullOrEmpty(value?.AttemptedValue))
                return null;
            var date = value.ConvertTo(typeof(T), CultureInfo.CurrentCulture);
            return date == null || date.ToString() == string.Empty ? null : date;
        }
    }

    public class NumericModelBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (string.IsNullOrEmpty(value?.AttemptedValue))
                return null;

            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(decimal?))
            {
                var numberD = Regex.Replace(value.AttemptedValue.Replace('.', ','), @"[\-]|[^\d](?!\d+$)", string.Empty);
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value.AttemptedValue.StartsWith("-")
                    ? (decimal.Parse(numberD) * -1).ToString()
                    : numberD);
            }

            var number = Regex.Replace(value.AttemptedValue, "[^0-9]", string.Empty);
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value.AttemptedValue.StartsWith("-")
                ? (long.Parse(number) * -1).ToString()
                : number);
        }
    }
}
