using ExceptionLibrary;
using Newtonsoft.Json;

namespace AppConfigLibrary;

public interface IAppConfigurationManager
{
    /// <summary>
    /// Cria ou atualiza o item de configuração a partir da chave e valor informados
    /// </summary>
    /// <param name="key">Chave do item de configuração</param>
    /// <param name="item">Valor a ser armazenado para o item</param>
    public void SetItem(string key, object item);

    /// <summary>
    /// Retorna o valor do item armazenado a partir da chave do item
    /// </summary>
    /// <typeparam name="T">Tipo do item que será retornado</typeparam>
    /// <param name="key">Chave do item de configuração</param>
    /// <returns>Retorna o item armazenado</returns>
    public T? GetItem<T>(string key);

    /// <summary>
    /// Deleta o item de configuração a partir da chave informada
    /// </summary>
    /// <param name="key">Chave do item de configuração</param>
    public void DeleteItem(string key);

    /// <summary>
    /// Retorna o valor do item armazenado a partir da chave do item
    /// </summary>
    /// <param name="key">Enumerador cuja string é chave do item de configuração</param>
    /// <param name="item">Valor a ser armazenado para o item</param>
    public void SetItem(Enum key, object item);

    /// <summary>
    /// Cria ou atualiza o item de configuração a partir da chave e valor informados
    /// </summary>
    /// <typeparam name="T">Tipo do item que será retornado</typeparam>
    /// <param name="key">Enumerador cuja string é chave do item de configuração</param>
    /// <returns>Retorna o item armazenado</returns>
    public T? GetItem<T>(Enum key);

    /// <summary>
    /// Deleta o item de configuração a partir da chave informada
    /// </summary>
    /// <param name="key">Enumerador cuja string é chave do item de configuração</param>
    public void DeleteItem(Enum key);
}

public class AppConfigurationManager : IAppConfigurationManager
{
    public string FilePath { get; set; }
    Dictionary<string, object?> ConfigurationItems { get; set; }

    public AppConfigurationManager(string filePath)
    {
        FilePath = filePath;
        ConfigurationItems = new();
        LoadConfigurationFile();
    }

    public void SetItem(string key, object item)
    {
        if (!ConfigurationItems.TryAdd(key, item))
            ConfigurationItems[key] = item;

        SaveChanges();
    }

    public T? GetItem<T>(string key)
    {
        if (ConfigurationItems.TryGetValue(key, out object? item))
        {
            if (item == null)
                return default;
            else
                return (T?)Convert.ChangeType(item, typeof(T));
        }
        else
            throw new NotFoundException($"Não existe um item com a chave '{key}' informada.");
    }

    public void DeleteItem(string key)
    {
        if (ConfigurationItems.Remove(key))
            SaveChanges();
    }

    private void SaveChanges()
    {
        try
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(ConfigurationItems));
        }
        catch (Exception ex)
        {
            throw new FileWriteUpdateException("Falha na escrita/atualização do arquivo.", ex);
        }
    }

    private void LoadConfigurationFile()
    {
        try
        {
            string fileContent = File.ReadAllText(FilePath);

            if (!string.IsNullOrEmpty(fileContent))
                ConfigurationItems = JsonConvert.DeserializeObject<Dictionary<string, object?>>(fileContent);
        }
        catch (FileNotFoundException) { }
        catch (Exception ex)
        {
            throw new FileWriteUpdateException("Falha na leitura do arquivo.", ex);
        }
    }

    public void SetItem(Enum key, object item)
    {
        SetItem(key.ToString(), item);
    }

    public T? GetItem<T>(Enum key)
    {
        return GetItem<T>(key.ToString());
    }

    public void DeleteItem(Enum key)
    {
        DeleteItem(key.ToString());
    }
}
