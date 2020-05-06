using System;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    internal class CamelCasePropertyNameContractResolver : IContractResolver
    {
        public JsonContract ResolveContract(Type type)
        {
            throw new NotImplementedException();
        }
    }
}