using MunicipalServiceApp.Domain.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Domain
{
    public static class Categories
    {
        private static readonly SimpleSet<string> _set = new();

        static Categories()
        {
            _set.Add("Roads");
            _set.Add("Sanitation");
            _set.Add("Utilities");
            _set.Add("Parks & Recreation");
            _set.Add("Public Safety");
        }

        public static IEnumerable<string> All()
        {
            foreach (var c in _set.Items()) yield return c;
        }
    }
}
