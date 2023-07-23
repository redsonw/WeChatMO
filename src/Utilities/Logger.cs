using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace YueHuan
{
    public class LogMessage
    {
        private readonly ListBox loggerListBox;

        public LogMessage(ListBox loggerListBox)
        {
            this.loggerListBox = loggerListBox;
        }

        public void Add(string message)
        {
            loggerListBox.Items.Add(message);
            loggerListBox.SelectedIndex = loggerListBox.Items.Count - 1;
            loggerListBox.ClearSelected();
            loggerListBox.TopIndex = loggerListBox.Items.Count - 1;
        }
    }

}
