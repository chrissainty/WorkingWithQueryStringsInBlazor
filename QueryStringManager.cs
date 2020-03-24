using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;

namespace BlazorQueryString
{
    public class QueryStringManager
    {
        private readonly NavigationManager _navigationManager;

        public event EventHandler<QueryStringChangedEventArgs> QueryStringChanged;

        public QueryStringManager(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public bool TryGetQueryString<T>(string key, out T value)
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);

            return TryGetQueryString<T>(QueryHelpers.ParseQuery(uri.Query), key, out value);
        }

        public bool TryGetQueryString<T>(Dictionary<string, StringValues> kvps, string key, out T value)
        {
            if (kvps.TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;
                    return true;
                }

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();
                    return true;
                }

                if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    value = (T)(object)valueAsDecimal;
                    return true;
                }
            }

            value = default;
            return false;
        }

        [JSInvokable]
        public void RaiseQueryStringChanged()
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
            var querystring = QueryHelpers.ParseNullableQuery(uri.Query);

            QueryStringChanged.Invoke(this, new QueryStringChangedEventArgs(querystring));
        }
    }

    public class QueryStringChangedEventArgs : EventArgs
    {
        public QueryStringChangedEventArgs(Dictionary<string, StringValues> kvps)
        {
            QueryStringKeyValuePairs = kvps;
        }

        public Dictionary<string, StringValues> QueryStringKeyValuePairs { get; set; }
    }
}
