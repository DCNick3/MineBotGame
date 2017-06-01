using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MineBotGame
{
    /// <summary>
    /// Class, that represents various number of all-typed resources (all types in one class)
    /// </summary>
    public class ResourceTotality
    {
        public ResourceTotality()
        {
            resources = new int[Enum.GetValues(typeof(ResourceType)).Length];
        }

        int[] resources;

        public int this[ResourceType res]
        {
            get
            {
                return resources[(int)res];
            }
            set
            {
                resources[(int)res] = value;
            }
        }

        public int[] ToArray()
        {
            return resources.Skip(1).ToArray();
        }

        #region Operators
        public static bool operator <(ResourceTotality a, ResourceTotality b)
        {
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                if (!(a[r] < b[r]))
                    return false;
            return true;
        }

        public static bool operator >(ResourceTotality a, ResourceTotality b)
        {
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                if (!(a[r] > b[r]))
                    return false;
            return true;
        }

        public static bool operator <=(ResourceTotality a, ResourceTotality b)
        {
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                if (!(a[r] <= b[r]))
                    return false;
            return true;
        }

        public static bool operator >=(ResourceTotality a, ResourceTotality b)
        {
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                if (!(a[r] >= b[r]))
                    return false;
            return true;
        }

        public static bool operator <(ResourceTotality a, int b)
        {
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                if (!(a[r] < b))
                    return false;
            return true;
        }

        public static bool operator >(ResourceTotality a, int b)
        {
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                if (!(a[r] > b))
                    return false;
            return true;
        }

        public static ResourceTotality operator -(ResourceTotality a, ResourceTotality b)
        {
            ResourceTotality res = new ResourceTotality();
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                res[r] = a[r] - b[r];
            return res;
        }

        public static ResourceTotality operator +(ResourceTotality a, ResourceTotality b)
        {
            ResourceTotality res = new ResourceTotality();
            foreach (var r in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
                res[r] = a[r] + b[r];
            return res;
        }

        public ResourceTotality Add(ResourceStack s)
        {
            this[s.Type] += s.Count;
            return this;
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var x in ToArray())
                sb.AppendFormat("{0} ", x);
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static IEnumerable<ResourceType> EnumResources()
        {
            return Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().Where((_) => _ != ResourceType.None);
        }

        public void AddAll(int val)
        {
            foreach (var x in EnumResources())
                this[x] += val;
        }
    }
}
