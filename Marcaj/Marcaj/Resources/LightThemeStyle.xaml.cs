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
    public partial class LightThemeStyle : ResourceDictionary
    {
        public static LightThemeStyle SharedInstance { get; } = new LightThemeStyle();
        public LightThemeStyle()
        {
            InitializeComponent();
        }
    }
}