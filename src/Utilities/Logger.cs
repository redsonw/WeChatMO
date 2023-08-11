namespace YueHuan
{
    public class LogMessage(ListBox loggerListBox)
    {
        private readonly ListBox loggerListBox = loggerListBox;

        public void Add(string message)
        {
            loggerListBox.Items.Add(message);
            loggerListBox.SelectedIndex = loggerListBox.Items.Count - 1;
            loggerListBox.ClearSelected();
            loggerListBox.TopIndex = loggerListBox.Items.Count - 1;
        }
    }

}
