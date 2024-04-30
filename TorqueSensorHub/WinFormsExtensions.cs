using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorqueSensorHub
{
    static public class WinFormsExtensions
    {
        public static void ForAllControls(this Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }
    }
}
