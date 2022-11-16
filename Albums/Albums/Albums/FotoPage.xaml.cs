using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Albums.ViewModel;
using Xamarin.Forms.Xaml;
using Albums.Model;
namespace Albums
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FotoPage : ContentPage
    {
        public FotoPage(AlbumModel album)
        {
            InitializeComponent();
            this.BindingContext = new FotosViewModel(Navigation, album);
            MessagingCenter.Subscribe<FotoPage, String>(this, "ErroFoto", (sender, a) =>
            {
                DisplayAlert("Erro", a, "OK");
            });
            MessagingCenter.Subscribe<FotoPage, String>(this, "SucessoFoto", (sender, a) =>
            {
                DisplayAlert("Sucesso", a, "OK");
            });
        }
    }
}