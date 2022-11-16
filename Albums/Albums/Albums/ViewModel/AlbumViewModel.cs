using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Albums.Model;
using Albums.Service;
using Xamarin.Forms;

namespace Albums.ViewModel
{
    public class AlbumViewModel : BaseViewModel
    {
        private BancoSQLiteService _service { get; set; }
        private INavigation _navigation { get; set; }
        public ICommand CarregarAlbumsCommand { get; set; }
        public ICommand EditarAlbumCommand { get; set; }
        public ICommand AdicionarAlbumCommand { get; set; }
        public ICommand DeletarAlbumCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand AdicionarFotoCommand { get; set; }
        private ObservableCollection<AlbumModel> items;
        public ObservableCollection<AlbumModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }
        bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public AlbumViewModel(INavigation navigation)
        {
            _service = new BancoSQLiteService();
            _navigation = navigation;
            AdicionarAlbumCommand = new Command(async () => await AdicionarAlbum());
            CarregarAlbumsCommand = new Command(async () => await CarregarAlbums());
            EditarAlbumCommand = new Command<AlbumModel>(async (a) => await EditarAlbum(a));
            RefreshCommand = new Command(async () => await RefreshItemsAsync());
            DeletarAlbumCommand = new Command<AlbumModel>(async (a) => await DeletarAlbum(a));
            AdicionarFotoCommand = new Command<AlbumModel>(async (a) => await AdicionarFoto(a));
            Items = new ObservableCollection<AlbumModel>();
            var t = Task.Run(() => this.CarregarAlbums());
            t.Wait();
        }
        private async Task AdicionarFoto(AlbumModel album)
        {
            try
            {
                await _navigation.PushAsync(new FotoPage(album), true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task AdicionarAlbum()
        {
            try
            {
                await _navigation.PushAsync(new ManterAlbumPage(), true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task EditarAlbum(AlbumModel a)
        {
            try
            {
                await _navigation.PushAsync(new ManterAlbumPage(a), true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task CarregarAlbums()
        {
            try
            {
                Items.Clear();
                IsLoading = true;
                // await Task.Delay(1000);                
                var dados = _service.RecuperarAlbums();
                foreach (var d in dados)
                {
                    Items.Add(d);
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<MainPage, String>(new MainPage(), "ErroAlbum", ex.Message);
                IsLoading = false;
            }
        }
        private async Task RefreshItemsAsync()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            await CarregarAlbums();
            IsRefreshing = false;
        }
        private async Task DeletarAlbum(AlbumModel a)
        {
            try
            {
                IsLoading = true;
                Items.Clear();
                await Task.Delay(TimeSpan.FromSeconds(1));
                _service.DeletarAlbum(a);
                await this.CarregarAlbums();
                IsLoading = false;
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<MainPage, String>(new MainPage(), "ErroAnotacao", ex.Message);
                IsLoading = false;
            }
        }
    }
}
