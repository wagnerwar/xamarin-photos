using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Albums.Model;
using Albums.Service;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Albums.ViewModel
{
    public class FotosViewModel : BaseViewModel
    {
        private BancoSQLiteService _service { get; set; }
        private INavigation _navigation { get; set; }
        public ICommand AdicionarFotoCommand { get; set; }
        public ICommand ExcluirFotoCommand { get; set; }
        AlbumModel _album;
        public AlbumModel Album
        {
            get { return _album; }
            set
            {
                _album = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<FotoModel> items;
        public ObservableCollection<FotoModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }
        public FotosViewModel(INavigation navigation, AlbumModel album)
        {
            _navigation = navigation;
            _service = new BancoSQLiteService();
            Album = album;
            Items = new ObservableCollection<FotoModel>();
            AdicionarFotoCommand = new Command(async () => await AdicionarFoto());
            ExcluirFotoCommand = new Command<FotoModel>(async (f) => await ExcluirFoto(f));
            var t = Task.Run(() => this.CarregarFotos());
            t.Wait();
        }
        private async Task AdicionarFoto()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "SucessoFoto", "Sucesso ao subir foto");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "ErroFoto", fnsEx.Message);
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "ErroFoto", pEx.Message);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "ErroFoto", ex.Message);
            }
        }
        private async Task ExcluirFoto(FotoModel foto)
        {
            try
            {
                await _service.ExcluirFoto(foto);
                await this.CarregarFotos();
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "SucessoFoto", "Sucesso ao excluir foto");
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "ErroFoto", ex.Message);
            }
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                //PhotoPath = null;
                return;
            }
            // recuperar binário e salvar no banco
            FotoModel arquivo = new FotoModel();
            arquivo.Nome = "foto";
            arquivo.Arquivo = File.ReadAllBytes(photo.FullPath);            
             _service.InserirFoto(arquivo, Album);
            await this.CarregarFotos();
        }
        private async Task CarregarFotos()
        {
            try
            {
                Items.Clear();
                IsLoading = true;
                var dados = await _service.RecuperarFotos(Album);
                foreach (var d in dados)
                {
                    Items.Add(d);
                }
                IsLoading = false;
            }            
            catch (Exception ex)
            {
                MessagingCenter.Send<FotoPage, String>(new FotoPage(Album), "ErroFoto", ex.Message);
            }
        }
    }
}
