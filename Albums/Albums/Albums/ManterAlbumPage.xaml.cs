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
    public partial class ManterAlbumPage : ContentPage
    {
        public ManterAlbumPage(AlbumModel dados = null)
        {
            InitializeComponent();
            this.BindingContext = new ManterAlbumViewModel(Navigation, dados);
         
            MessagingCenter.Subscribe<ManterAlbumPage>(this, "MensagemSucesso", (sender) =>
            {
                DisplayAlert("Sucesso!", "Album salvo com sucesso", "OK");
            });

            MessagingCenter.Subscribe<ManterAlbumPage, String>(this, "ErroAlbum", (sender, a) =>
            {
                DisplayAlert("Erro", a, "OK");
            });
        }
    }
}