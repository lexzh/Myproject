namespace Bussiness.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance;

        static Settings()
        {
            old_acctor_mc();
        }

        private static void old_acctor_mc()
        {
            defaultInstance = (Settings) SettingsBase.Synchronized(new Settings());
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }
    }
}

