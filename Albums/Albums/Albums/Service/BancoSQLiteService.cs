using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;
using Albums.Model;
namespace Albums.Service
{
    public class BancoSQLiteService
    {
        private String _banco = "album.db";
        public string DatabasePath
        {
            get
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder();
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                String caminho = Path.Combine(basePath, _banco);
                connectionStringBuilder.DataSource = caminho;
                return connectionStringBuilder.ConnectionString;
            }
        }
        public void CriarBanco()
        {
            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    var command = conexao.CreateCommand();
                    command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS album (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        nome varchar(100) NOT NULL
                    )
                ";
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        public void InserirAlbum(AlbumModel album)
        {
            try
            {
                this.CriarBanco();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    using (var transaction = conexao.BeginTransaction())
                    {
                        try
                        {

                            var command = conexao.CreateCommand();
                            command.CommandText =
                            String.Format(@"
                            insert into album (nome ) values('{0}')
                            ", album.Nome);
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        public void EditarAlbum(AlbumModel album)
        {
            try
            {
                this.CriarBanco();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    using (var transaction = conexao.BeginTransaction())
                    {
                        try
                        {

                            var command = conexao.CreateCommand();
                            command.CommandText =
                            String.Format(@"
                            update album set nome = '{0}' where id = '{1}'
                            ", album.Nome, album.Id);
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        public void DeletarAlbum(AlbumModel album)
        {
            try
            {
                this.CriarBanco();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    using (var transaction = conexao.BeginTransaction())
                    {
                        try
                        {

                            var command = conexao.CreateCommand();
                            command.CommandText =
                            String.Format(@"
                            delete from album where id = '{1}'
                            ", album.Id);
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        public List<AlbumModel> RecuperarAlbums()
        {
            
            List<AlbumModel> lista = new List<AlbumModel>();
            using (var conexao = new SqliteConnection(DatabasePath))
            {
                conexao.Open();
                try
                {
                    var command = conexao.CreateCommand();
                    command.CommandText =
                    String.Format(@"
                        select id, nome from album
                    ");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AlbumModel item = new AlbumModel();
                            item.Id = reader.GetInt32(0);
                            item.Nome = reader.GetString(1);
                            lista.Add(item);
                        }
                    }
                    return lista;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
    }
}
