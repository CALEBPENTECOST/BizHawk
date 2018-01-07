using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPENTECOST.BizHawk.DeepLearning
{
    public sealed class Backend
    {
        /// <summary>
        /// Static instance of this class. Created once, and used for all NN needs.
        /// </summary>
        private static readonly Lazy<Backend> sInstance = new Lazy<Backend>(() => new Backend(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Internal object used for locking. Nearly all functions will lock to ensure thread synchronization
        /// </summary>
        private object locker = new object();

        private static UserView mUserView = new UserView();
        private static Thread mViewThread = new Thread(new ThreadStart(viewThreadContext));

        /// <summary>
        /// Gets the singleton instance of the class. This is how you will normally access this class.
        /// </summary>
        /// <returns></returns>
        public static Backend GetInstance()
        {
            // Get the singleton instance
            return sInstance.Value;
        }
        
        /// <summary>
        /// Constructor. Kept private for singleton class.
        /// </summary>
        private Backend()
        {
            
        }

        /// <summary>
        /// This thread should only be started once, and allows for a new UI thread for the neural net view
        /// </summary>
        private static void viewThreadContext()
        {
            Application.Run(mUserView);
        }

        /// <summary>
        /// Requests the view for the neural net engine. This form should not be closable, so all we have to do is start it up the first time
        /// </summary>
        public void RequestView()
        {
            lock(locker)
            {
                // Have we started off a view thread yet?
                if (mViewThread.IsAlive == false)
                {
                    // We need to start the thread and all
                    mViewThread.Start();
                }
                else
                {
                    // The form already exists -- nothing left to do.
                }
            }
        }

        private void GetCurrentGameImage()
        {
            lock(locker)
            {
                using (var bb = GlobalWin.DisplayManager.RenderOffscreen(_currentVideoProvider, Global.Config.Screenshot_CaptureOSD))
                {
                    using (var img = bb.ToSysdrawingBitmap())
                    {
                        Clipboard.SetImage(img);
                    }
                }
            }
        }
    }
}
