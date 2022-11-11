using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Albums.ViewModel;
namespace Albums
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new AlbumViewModel(Navigation);
            MessagingCenter.Subscribe<MainPage, String>(this, "ErroAlbum", (sender, a) =>
            {
                DisplayAlert("Erro", a, "OK");
            });
        }
    }
}
