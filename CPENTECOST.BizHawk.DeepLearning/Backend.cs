using BizHawk.Bizware.BizwareGL;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPENTECOST.BizHawk.DeepLearning
{
    public class Backend
    {
        /// <summary>
        /// Static instance of this class. Created once, and used for all NN needs.
        /// </summary>
        private static readonly Lazy<Backend> sInstance = new Lazy<Backend>(() => new Backend(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Internal object used for locking. Nearly all functions will lock to ensure thread synchronization
        /// </summary>
        private object locker = new object();

        private UserView mUserView = null;
        private Thread mViewThread = null;
        private Form mParentForm = null;
        private IVideoProvider mGameVideo = null;

        public int mCurrentFrameNumber = 0;

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
            mUserView = new UserView(this);
            mViewThread = new Thread(new ThreadStart(viewThreadContext));
        }

        /// <summary>
        /// This thread should only be started once, and allows for a new UI thread for the neural net view
        /// </summary>
        private void viewThreadContext()
        {
            Application.Run(mUserView);
        }

        /// <summary>
        /// Requests the view for the neural net engine. This form should not be closable, so all we have to do is start it up the first time
        /// </summary>
        public void RequestView(Form parentForm)
        {
            lock(locker)
            {
                // Have we started off a view thread yet?
                if (mViewThread.IsAlive == false)
                {
                    // We need to start the thread and all
                    mParentForm = parentForm;
                    parentForm.FormClosing += ParentForm_FormClosing;
                    mViewThread.Start();                    
                }
                else
                {
                    // The form already exists -- nothing left to do.
                }
            }
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When the parent form is closing, we will also want to close the user view
            mUserView.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                mUserView.Close();
            });        
        }

        public BitmapBuffer capuredImage = null;

        public void GetCurrentGameImage()
        {
            lock (locker)
            {
                capuredImage = new BitmapBuffer(mGameVideo.BufferWidth, mGameVideo.BufferHeight, mGameVideo.GetVideoBuffer());
            }
        }

        public void SetVideoProvider(IVideoProvider _currentVideoProvider)
        {
            mGameVideo = _currentVideoProvider;
        }

        private List<RamWatch> list_ramWatches = new List<RamWatch>();

        public uint GetWatchVal(string name)
        {
            // Check each watch
            foreach(var watch in list_ramWatches)
            {
                // If the name matches, return the value
                if (watch.Name == name)
                {
                    return watch.Value;
                }
            }

            // Didn't find that name. Return 0
            return 0;
        }

        private class RamWatch
        {
            public string Name;
            public long Address;
            public uint Value;
            public string Notes;

            public RamWatch(string n, long a, string m = "No Notes")
            {
                Name = n;
                Address = a;
                Notes = n;
                Value = 0;
            }
        }

        public void ClearRamWatches()
        {
            lock (locker)
            {
                list_ramWatches.Clear();
            }
        }

        public void AddRamWatch(string name, long address, string notes = "No Notes")
        {
            // We want to add a new RAM watch to the list
            // Make sure nothing else has the same name or address
            lock (locker)
            {
                foreach (var watch in list_ramWatches)
                {
                    if (watch.Name == name || watch.Address == address)
                    {
                        // Duplicate! Do not add
                        MessageBox.Show("Duplicate value not added");
                        return;
                    }
                }

                // Add to the list!
                list_ramWatches.Add(new RamWatch(name, address, notes));
            }
        }

        private void ReadAllRamWatches()
        {
            lock (locker)
            {
                // Lets go into each ram watch and update the value
                for (int index = 0; index < list_ramWatches.Count; index++)
                {
                    list_ramWatches[index].Value = ReadMainMemoryUint(list_ramWatches[index].Address);
                }
            }
        }

        public void NewFrameReady(int frameNumber)
        {
            lock (locker)
            {
                // Get frame info and save it in the model
                mCurrentFrameNumber = frameNumber;

                // Acquire an image (we will always want the image)
                GetCurrentGameImage();

                // Also fill out our dictionary of read values
                ReadAllRamWatches();

                // Finally, let the GUI know its time to update its view of the model
                ForceViewUpdate();
            }
        }

        private void ForceViewUpdate()
        {
            // Tell the other GUI to update, but don't wait for it to finish
            mUserView.BeginInvoke((MethodInvoker)delegate
            {
                // Update the form on the forms thread
                mUserView.ForceViewUpdate();
            });
        }

        [RequiredService]
        public IMemoryDomains MemoryDomains
        {
            get;
            set;
        }

        private uint ReadMainMemoryUint(long addr = 0x1644D0)
        {
            var main = MemoryDomains.MainMemory;
            var value = main.PeekUint(addr, true);

            return value;
        }
    }
}
