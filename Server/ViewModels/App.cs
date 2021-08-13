using System;
using DotNetify;

namespace UtilityBox.App.Server.ViewModels
{
    public class App : BaseVM
    {
        public Action<bool> Exit => b =>
        {
            if (b) // just validation
                Environment.Exit(1);
        };
    }
}