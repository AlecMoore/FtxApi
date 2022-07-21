using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeanReversion
{
    // A simple light-weight cache, used for storing data in the memory of each running instance of the Azure Function.
    // If an instance gets idle (for 5 minutes or whatever the latest time period is) or if the Function App scales out to an extra instance then the cache is re-populated.
    // To use, create a static readonly instance of this class in the Azure Function class, in the constructor pass a function which populates the object to cache.
    // Then simply reference the Data object.  It will be populated on the first call and re-used on future calls whilst the same instance remains alive.
    public class FunctionInstanceCache<T>
    {
        public FunctionInstanceCache(Func<T> populate)
        {
            Populate = populate;
            IsInit = false;
        }

        public Func<T> Populate { get; set; }

        public bool IsInit { get; set; }

        private T data;

        public T Data
        {
            get
            {
                if (IsInit == false)
                {
                    Init();
                };
                return data;
            }

        }

        public void Init()
        {
            data = Populate();
            IsInit = true;
        }
    }
}
