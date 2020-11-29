namespace UtilityBox.App.Server.Models
{
    public interface IWindowsApp
    {
        /**
         * Name of the program to find it through Powershell
         */
        string Name { get; }
        
        /**
         * The actual name (that will be displayed)
         */
        string DisplayName { get; }
        
        /**
         * Description of what the program is / where it comes from
         */
        string Description { get; }
    }
}