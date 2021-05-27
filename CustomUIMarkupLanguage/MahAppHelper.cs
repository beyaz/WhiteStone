using System;
using System.Collections.Generic;
using System.Windows;

namespace CustomUIMarkupLanguage
{
    public static class MahAppHelper
    {
        static readonly List<ResourceDictionary> MahAppsResourceDictionaries =  new List<ResourceDictionary>
        {
            new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")},
            new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")},
            new ResourceDictionary {Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml")}
        };

        
        public static void MergeMahAppMetroStyles(ResourceDictionary resources)
        {
            foreach (var dictionary in MahAppsResourceDictionaries)
            {
                resources.MergedDictionaries.Add(dictionary);
            }
        }

    }
}