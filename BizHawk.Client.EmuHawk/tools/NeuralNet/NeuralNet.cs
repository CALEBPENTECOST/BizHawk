using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk
{
    public partial class NeuralNet : ToolFormBase, IToolForm
    {
        // ////////
        // Required Services
        // ////////
        [RequiredService]
        public IMemoryDomains MemoryDomains { get; set; }

        [RequiredService]
        public IEmulator Emu { get; set; }

        public NeuralNet()
        {
            InitializeComponent();
        }

        public bool UpdateBefore => false;

        public bool AskSaveChanges()
        {
            return true;
        }

        public void FastUpdate()
        {
            // Update the GUI with the info present in the NN model
            //throw new NotImplementedException();
        }

        public void NewUpdate(ToolFormUpdateType type)
        {
            // Do nothing
        }

        public void Restart()
        {
            // To restart this tool is to re-load everything?
            //throw new NotImplementedException();
        }

        public void UpdateValues()
        {
            // This is a full, slow update.
            //throw new NotImplementedException();
        }
    }
}
