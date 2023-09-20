using ExceptionLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace AppDataLibrary;

public interface IAppDataManager
{
    /// <summary>
    /// Cria ou atualiza o item a partir da chave e valor informados
    /// </summary>
    /// <param name="key">Chave do item</param>
    /// <param name="item">Valor a ser armazenado para o item</param>
    public void SetItem(string key, object item);

    /// <summary>
    /// Retorna o valor do item armazenado a partir da chave do item
    /// </summary>
    /// <typeparam name="T">Tipo do item que será retornado</typeparam>
    /// <param name="key">Chave do item</param>
    /// <returns>Retorna o item armazenado</returns>
    public T? GetItem<T>(string key);

    /// <summary>
    /// Deleta o item a partir da chave informada
    /// </summary>
    /// <param name="key">Chave do item</param>
    public void DeleteItem(string key);

    /// <summary>
    /// Retorna o valor do item armazenado a partir da chave do item
    /// </summary>
    /// <param name="key">Enumerador cuja string é chave do item</param>
    /// <param name="item">Valor a ser armazenado para o item</param>
    public void SetItem(Enum key, object item);

    /// <summary>
    /// Cria ou atualiza o item a partir da chave e valor informados
    /// </summary>
    /// <typeparam name="T">Tipo do item que será retornado</typeparam>
    /// <param name="key">Enumerador cuja string é chave do item</param>
    /// <returns>Retorna o item armazenado</returns>
    public T? GetItem<T>(Enum key);

    /// <summary>
    /// Deleta o item a partir da chave informada
    /// </summary>
    /// <param name="key">Enumerador cuja string é chave do item</param>
    public void DeleteItem(Enum key);
}

public class AppDataManager : IAppDataManager
{
    public string FilePath { get; set; }
    Dictionary<string, object?> AppDataItems { get; set; }

    public AppDataManager(string filePath)
    {
        FilePath = filePath;
        AppDataItems = new();
        LoadAppDataFile();
    }

    public void SetItem(string key, object item)
    {
        if (!AppDataItems.TryAdd(key, item))
            AppDataItems[key] = item;

        SaveChanges();
    }

    public T? GetItem<T>(string key)
    {
        if (AppDataItems.TryGetValue(key, out object? item))
        {
            if (item == null)
                return default;
            else
            {
                try
                {
                    if(item is JArray jArray)
                    {
                        string json = JsonConvert.SerializeObject(jArray);
                        return JsonConvert.DeserializeObject<T>(json);
                    }
                    else
                        return (T?)Convert.ChangeType(item, typeof(T)); 
                }
                catch(Exception ex)
                {
                    return default;
                }
            }
        }
        else
            throw new NotFoundException($"Não existe um item com a chave '{key}' informada.");
    }

    public void DeleteItem(string key)
    {
        if (AppDataItems.Remove(key))
            SaveChanges();
    }

    private void SaveChanges()
    {
        try
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(AppDataItems));
        }
        catch (Exception ex)
        {
            throw new FileWriteUpdateException("Falha na escrita/atualização do arquivo.", ex);
        }
    }

    private void LoadAppDataFile()
    {
        try
        {
            string fileContent = File.ReadAllText(FilePath);

            if (!string.IsNullOrEmpty(fileContent))
                AppDataItems = JsonConvert.DeserializeObject<Dictionary<string, object?>>(fileContent);
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
