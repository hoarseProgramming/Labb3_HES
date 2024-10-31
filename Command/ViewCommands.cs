using System.Windows.Input;

namespace Labb3_HES.Command
{
    public class ViewCommands
    {

        public static RoutedUICommand OpenPackOptionsCommand
                            = new RoutedUICommand("Open Pack Options", "OpenPackOptionsCommand", typeof(ViewCommands));

        public static RoutedUICommand CreateNewPackCommand
                            = new RoutedUICommand("Create New pack", "CreateNewPackCommand", typeof(ViewCommands));

    }
}
