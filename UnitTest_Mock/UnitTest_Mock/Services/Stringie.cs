using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest_Mock.Services
{
  
    public static class RemoveExtensions
    {
        public static RemoveString Remove(this string source)
        {
            return new RemoveString(source);
        }
    }

    public class RemoveString
    {
        private readonly string _source;

        internal RemoveString(string source)
        {
            _source = source;
        }

        public static implicit operator string(RemoveString removeString)
        {
            return removeString.ToString();
        }

        public override string ToString()
        {
            return _source != null ? string.Empty : null;
        }
    }
}
