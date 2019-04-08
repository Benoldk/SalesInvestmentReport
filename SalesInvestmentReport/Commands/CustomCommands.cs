using System.Windows.Input;

namespace SalesInvestmentReport.Commands
{
    /// <summary>
    /// Contains custom command for WPF view to use
    /// </summary>
    public static class CustomCommands
    {
        /// <summary>
        /// Custom command to bind to CTRL + L keys and load the report
        /// </summary>
        public static readonly RoutedUICommand LoadReport = new RoutedUICommand
        (
            "Load Report",
            "loadReportMenuItem",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, ModifierKeys.Control)
            }
        );
        
        /// <summary>
        /// Custom command to bind to CTRL + G keys and generate the report
        /// </summary>
        public static readonly RoutedUICommand GenerateReport = new RoutedUICommand
        (
            "Generage Report",
            "generateReportMenuItem",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.G, ModifierKeys.Control)
            }
        );
        
        /// <summary>
        /// Custom command to bind to CTRL + S keys and save the report
        /// </summary>
        public static readonly RoutedUICommand SaveReport = new RoutedUICommand
        (
            "Save Report",
            "saveReportMenuItem",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
        );

        /// <summary>
        /// Custom command to bind to ALT + F4 keys and exit out of the application
        /// </summary>
        public static readonly RoutedUICommand ExitReport = new RoutedUICommand
        (
            "Exit",
            "exitAppMenuItem",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );
    }
}
