using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsWebApp.Models
{
    public class RegistrationExceptions
    {
        public IReadOnlyDictionary<Type, (int, string)> Exceptions => new Dictionary<Type, (int, string)>
        { 
            [typeof(InvalidOperationException)] = (601, "Empty login or password!"),
            [typeof(ArgumentOutOfRangeException)] = (602, "Wrong format of password!"),
            [typeof(JsonException)] = (603, "Wrong format of password or login!"),
            [typeof(NotImplementedException)] = (604, "registration error!"),
            [typeof(Exception)] = (605, "Inner server error!")
        };

    }
}
