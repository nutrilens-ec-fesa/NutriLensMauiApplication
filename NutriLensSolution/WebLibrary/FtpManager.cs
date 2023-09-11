using ExceptionLibrary;
using FluentFTP;
using FluentFTP.Exceptions;

namespace WebLibrary;

public interface IFtpManager
{
    public List<FtpListItem> GetFtpPathContent(string path);
    public bool CreateDirectory(string path, string directoryName);
    public bool UploadFile(string remotePath, string localPath);
}

public class FtpManager : IFtpManager
{
    private readonly string _host;
    private readonly string _user;
    private readonly string _password;

    public FtpManager(string host, string user, string password)
    {
        _host = host;
        _user = user;
        _password = password;
    }

    /// <summary>
    /// Retorna uma lista dos diretórios/arquivos presentes no caminho especificado
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="FtpException"></exception>
    public List<FtpListItem> GetFtpPathContent(string path)
    {
        List<FtpListItem> ftpItemsList = new();

        using (FtpClient client = new(_host, _user, _password))
        {
            try
            {
                ftpItemsList = client
                    .GetListing(path)
                    .ToList();
            }
            catch (FtpException ex)
            {
                throw new FtpException("Houve algum erro na obtenção ou leitura dos diretórios FTP.", ex);
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }

        return ftpItemsList;
    }

    /// <summary>
    /// Cria um novo diretório (pasta) no caminho especificado
    /// </summary>
    /// <param name="path">Caminho a ser criado o diretório (pasta)</param>
    /// <param name="directoryName">Nome do diretório (pasta)</param>
    /// <returns></returns>
    /// <exception cref="FtpException"></exception>
    public bool CreateDirectory(string path, string directoryName)
    {
        List<string> pathContentFileNames;
        string existingDirectory = string.Empty;

        try
        {
            // Retorna todos os nomes de arquivos presentes no caminho em questão
            pathContentFileNames = GetFtpPathContent(path)
                .Select(x => x.Name)
                .ToList();

            // Busca se já existe um diretório com o nome especificado
            existingDirectory = pathContentFileNames
                .Where(x => x == directoryName)
                .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new FtpException(ExceptionManager.ExceptionMessage(ex));
        }

        // Se já existe o diretório especificado retorna true
        if (existingDirectory != null)
            return true;

        // Caso não exista, procede para sua criação
        using FtpClient client = new(_host, _user, _password);

        try
        {
            return client.CreateDirectory(Path.Combine(path, directoryName));
        }
        catch (Exception ex)
        {
            throw new FtpException("Houve um problema na criação do diretório no FTP.", ex);
        }
        finally
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
        }
    }

    /// <summary>
    /// Realiza upload do arquivo no caminho especificado
    /// </summary>
    /// <param name="remotePath">Caminho de destino do arquivo</param>
    /// <param name="localPath">Caminho de origem do arquivo</param>
    /// <returns></returns>
    /// <exception cref="FtpException"></exception>
    public bool UploadFile(string remotePath, string localPath)
    {
        using FtpClient client = new(_host, _user, _password);

        try
        {
            using FileStream fileStream = File.OpenRead(localPath);

            FtpStatus ftpResult = client.UploadStream(fileStream, Path.Combine(remotePath, localPath.Split("\\").Last()), FtpRemoteExists.Overwrite);

            if (ftpResult == FtpStatus.Success)
                return true;
            else
                throw new FtpException($"Falha: {ftpResult}");
        }
        catch (Exception ex)
        {
            throw new FtpException($"Falha no envio do arquivo '{localPath}'", ex);
        }
    }
}
