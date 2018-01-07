using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPENTECOST.BizHawk.DeepLearning
{
    public sealed class Backend
    {
        /// <summary>
        /// Static instance of this class. Created once, and used for all NN needs.
        /// </summary>
        private static readonly Lazy<Backend> sInstance = new Lazy<Backend>(() => new Backend());
        



        public static Backend GetInstance()
        {
            // Get the singleton instance
            return sInstance.Value;
        }

        private bool mInitialized { get; set; }

        /// <summary>
        /// Constructor. Kept private for singleton class.
        /// </summary>
        private Backend()
        {
            
        }

        private bool FormRequested()
        {

        }
    }
}
