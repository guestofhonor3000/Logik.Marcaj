using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DarkThemeStyle : ResourceDictionary
    {
        public static DarkThemeStyle SharedInstance { get; } = new DarkThemeStyle();
        public DarkThemeStyle()
        {
            InitializeComponent();
        }
    }
}