using Marcaj.Models.DbModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marcaj.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifiersModalPage : ContentPage
    {
        OrderTransactionsModel item;
        MenuItemsModel itemMenuModel;
        public ModifiersModalPage(OrderTransactionsModel orderItem, MenuItemsModel menuItem)
        {
            InitializeComponent();
            item = orderItem;
            itemMenuModel = menuItem;
            multiShow();
        }

        private async void multiShow()
        {
            Grid grid = new Grid();

            grid.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
            };

            Label DefMod = new Label
            {
                Margin = new Thickness(0),
            };
            DefMod.SetDynamicResource(StyleProperty, "mainBtnLabel");
            //DefMod.Text = itemMenuModel.DefaultModifierType;

            Label ModBuild = new Label
            {
                //Text = itemMenuModel.ModBuilderTemplateID.ToString(),
                Margin = new Thickness(0),
            };
            ModBuild.SetDynamicResource(StyleProperty, "mainBtnLabel");

            grid.Children.Add(DefMod, 0, 0);
            grid.Children.Add(DefMod, 0, 1);
            multiGrid.Children.Add(grid, 0, 1);
        }

        private async void closeModal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}