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
    public class ManterAlbumViewModel : BaseViewModel
    {
        public ICommand LimparCamposCommand { get; set; }
        public ICommand EnviarAlbumCommand { get; set; }
        public ICommand VoltarTelaInicialCommand { get; set; }
        private BancoSQLiteService _service { get; set; }
        private INavigation _navigation { get; set; }
        private String _nome;
        public String Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                OnPropertyChanged();
            }
        }
        private Int32 _id;
        public Int32 Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public ManterAlbumViewModel(INavigation navigation, AlbumModel dados = null)
        {
            _service = new BancoSQLiteService();
            _navigation = navigation;
            LimparCamposCommand = new Command(async () => await LimparCampos());
            EnviarAlbumCommand = new Command(async () => await EnviarAlbum());
            VoltarTelaInicialCommand = new Command(async () => await VoltarTelaInicial());
            if (dados != null)
            {
                Id = dados.Id;
                Nome = dados.Nome;
            }
        }
        private async Task LimparCampos()
        {
            try
            {
                Nome = String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ValidarCampos()
        {
            try
            {
                String erro = String.Empty;
                if (String.IsNullOrEmpty(Nome))
                {
                    erro = "Nome precisa ser digitado";
                }
                if (!String.IsNullOrEmpty(erro))
                {
                    MessagingCenter.Send<ManterAlbumPage, String>(new ManterAlbumPage(), "ErroAlbum", erro);
                    throw new Exception(erro);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task EnviarAlbum()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(TimeSpan.FromSeconds(1));
                await ValidarCampos();
                AlbumModel dadosAlbum = new AlbumModel();
                dadosAlbum.Nome = Nome;
                dadosAlbum.Id = Id;
                // Gravação
                if (dadosAlbum.Id > 0)
                {
                    _service.EditarAlbum(dadosAlbum);
                }
                else
                {
                    _service.InserirAlbum(dadosAlbum);
                    await LimparCampos();
                }
                IsLoading = false;
                MessagingCenter.Send<ManterAlbumPage>(new ManterAlbumPage(), "MensagemSucesso");
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<ManterAlbumPage, String>(new ManterAlbumPage(), "ErroAlbum", ex.Message);
                throw ex;
            }
        }
        private async Task VoltarTelaInicial()
        {
            try
            {
                await _navigation.PopToRootAsync(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
