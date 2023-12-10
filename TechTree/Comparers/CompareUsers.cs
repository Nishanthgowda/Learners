using System.Diagnostics.CodeAnalysis;
using TechTree.Areas.Admin.Models;

namespace TechTree.Comparers
{
    public class CompareUsers : IEqualityComparer<UserViewModel>
    {
        public bool Equals(UserViewModel x, UserViewModel y)
        {
            if(y == null) { return false; }

            if(x.Id == y.Id) { return true; }

            return false;
        }

        public int GetHashCode([DisallowNull] UserViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
