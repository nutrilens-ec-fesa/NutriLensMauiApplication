using ExceptionLibrary;
using Newtonsoft.Json;
using System.Reflection;

namespace DictionaryLibrary;

public interface IDictionaryManager
{
    public string FilePath { get; set; }

    public Languages Language { get; set; }

    public List<DictionaryItem> Items { get; set; }

    public bool Initialize(string filePath, Languages language);

    public string GetItem(string key);

    public string GetItem(Enum key);

    public void AddItem(DictionaryItem dictionaryItem);

    public bool RemoveItem(string key);

    public string SaveFile();

    public string LoadFile();

    public void TranslateView(ContentPage contentPage);

    public void UpdateLanguage(Languages language);
}

/// <summary>
/// Enumerador de idiomas
/// </summary>
public enum Languages
{
    Portuguese,
    Spanish,
    English,
    All
}

public class DictionaryManager : IDictionaryManager
{
    #region Properties

    /// <summary>
    /// Caminho do arquivo json
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Linguagem configurada no dicionário
    /// </summary>
    public Languages Language { get; set; }

    /// <summary>
    /// Itens do dicionário
    /// </summary>
    public List<DictionaryItem> Items { get; set; }

    #endregion

    #region Constructors
    public DictionaryManager()
    {

    }

    public DictionaryManager(string filePath)
    {
        FilePath = filePath;
        Items = new List<DictionaryItem>();
        LoadFile();
    }

    public DictionaryManager(string filePath, Languages language)
    {
        FilePath = filePath;
        Language = language;
        Items = new List<DictionaryItem>();
        LoadFile();
    }

    public DictionaryManager(string filePath, List<DictionaryItem> items)
    {
        FilePath = filePath;
        Items = items;
    }
    #endregion

    #region Public Methods

    public bool Initialize(string filePath, Languages language)
    {
        FilePath = filePath;
        Language = language;
        Items = new List<DictionaryItem>();
        return string.IsNullOrEmpty(LoadFile());
    }

    /// <summary>
    /// Busca um item do dicionário pela chave do item a partir de uma string
    /// </summary>
    /// <param name="key">Chave do item no dicionário</param>
    /// <returns>Conteúdo do item no idioma configurado</returns>
    public string GetItem(string key)
    {
        DictionaryItem dictionaryItem = Items.FirstOrDefault(x => x.KeyString == key);

        if (dictionaryItem == null)
            return null;
        else
            return dictionaryItem.GetValue(Language);
    }

    /// <summary>
    /// Busca um item do dicionário pela chave do item a partir de um enumerador
    /// </summary>
    /// <param name="key">Enum cujo nome é a chave do item no dicionário</param>
    /// <returns>Conteúdo do item no idioma configurado</returns>
    public string GetItem(Enum key)
    {
        DictionaryItem dictionaryItem = Items.FirstOrDefault(x => x.KeyString == key.ToString());

        if (dictionaryItem == null)
            return null;
        else
            return dictionaryItem.GetValue(Language);
    }

    /// <summary>
    /// Adiciona um novo item de dicionário nos itens do dicionário
    /// </summary>
    /// <param name="dictionaryItem"></param>
    public void AddItem(DictionaryItem dictionaryItem)
    {
        Items.Add(dictionaryItem);
    }

    /// <summary>
    /// Remove um item do dicionário pela chave do item a partir de uma string
    /// </summary>
    /// <param name="key">Chave do item no dicionário</param>
    /// <returns>True caso consiga remover o item, false caso não consiga ou não esteja na lista</returns>
    public bool RemoveItem(string key)
    {
        DictionaryItem dictionaryItem = Items.FirstOrDefault(x => x.KeyString == key);

        if (dictionaryItem == null)
            return false;
        else
            return Items.Remove(dictionaryItem);
    }

    /// <summary>
    /// Salva o arquivo json do dicionário a partir de seus itens e caminho configurado
    /// </summary>
    /// <returns>Possível mensagem de erro</returns>
    public string SaveFile()
    {
        try
        {
            string jsonContent = JsonConvert.SerializeObject(Items, Formatting.Indented);
            File.WriteAllText(FilePath, jsonContent);
            return string.Empty;
        }
        catch (Exception ex)
        {
            return "Falha ao salvar arquivo json. " + ExceptionManager.ExceptionMessage(ex);
        }
    }

    /// <summary>
    /// Carrega os itens do dicionário a partir do caminho configurado
    /// </summary>
    /// <returns>Possível mensagem de erro</returns>
    public string LoadFile()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                string fileContent = File.ReadAllText(FilePath);

                if (!string.IsNullOrEmpty(fileContent))
                {
                    Items = JsonConvert.DeserializeObject<List<DictionaryItem>>(fileContent);
                    Items = Items.Where(x => x.HasKey).ToList();
                    //break;
                }
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            return "Falha ao carregar arquivo json. " + ExceptionManager.ExceptionMessage(ex);
        }
    }

    /// <summary>
    /// A partir do envio de uma content page, traduz todos os controles
    /// cujos identificadores correspondam à chaves de dicionário
    /// </summary>
    /// <param name="contentPage">Objeto ContentPage</param>
    public void TranslateView(ContentPage contentPage)
    {
        IList<IView> viewToTranslate = new List<IView>();

        FindTranslatableRecursive(contentPage.Content, viewToTranslate);

        foreach (IView child in viewToTranslate)
        {
            string dictionaryText;

            if (child is Label label)
            {
                dictionaryText = GetItem(label.StyleId);

                if (!string.IsNullOrEmpty(dictionaryText))
                    label.Text = dictionaryText;
            }
            else if (child is Button button)
            {
                dictionaryText = GetItem(button.StyleId);

                if (!string.IsNullOrEmpty(dictionaryText))
                    button.Text = dictionaryText;
            }
            else if (child is SearchBar searchBar)
            {
                dictionaryText = GetItem(searchBar.StyleId);

                if (!string.IsNullOrEmpty(dictionaryText))
                    searchBar.Placeholder = dictionaryText;
            }
            else if (child is ContentView contentView)
                TranslateContentView(contentView);
            else if (child is StackBase stackNase)
                TranslateStack(stackNase);
        }
    }

    /// <summary>
    /// Encontra e guarda todos os itens dos tipos Label, Button e ContentView
    /// </summary>
    /// <param name="view"></param>
    /// <param name="views"></param>
    private void FindTranslatableRecursive(IView view, IList<IView> views)
    {
        if (view is Label label)
            views.Add(label);
        else if (view is Button button)
            views.Add(button);
        else if (view is SearchBar searchBar)
            views.Add(searchBar);
        else if (view is ContentView contentView)
            views.Add(contentView);
        else if (view is ScrollView scrollView)
            FindTranslatableRecursive(scrollView.Content, views);
        else if (view is Microsoft.Maui.Controls.Grid grid)
        {
            foreach (IView child in grid.Children)
            {
                FindTranslatableRecursive(child, views);
            }
        }
        else if (view is Microsoft.Maui.Controls.Layout layout)
        {
            foreach (IView child in layout.Children)
            {
                FindTranslatableRecursive(child, views);
            }
        }
    }

    /// <summary>
    /// Realiza tradução de ContentView (TemplateView) no padrão de Xid
    /// </summary>
    /// <param name="contentView"></param>
    private void TranslateContentView(ContentView contentView)
    {
        List<PropertyInfo> xidProperties = contentView
                .GetType()
                .GetProperties()
                .Where(x => x.Name.EndsWith("Xid"))
                .ToList();

        foreach (PropertyInfo xidProp in xidProperties)
        {
            string xid = xidProp.GetValue(contentView).ToString();

            if (!string.IsNullOrEmpty(xid))
            {
                string dictionaryText = GetItem(xid);

                if (!string.IsNullOrEmpty(dictionaryText))
                    contentView.GetType().GetProperty(xidProp.Name.Replace("Xid", string.Empty)).SetValue(contentView, dictionaryText);
            }
        }
    }

    /// <summary>
    /// Atualiza a linguagem configurada no dicionário
    /// </summary>
    /// <param name="language"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void UpdateLanguage(Languages language)
    {
        Language = language;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Método recursivo que traduz os itens dentro de stacks
    /// </summary>
    /// <param name="stackBase"></param>
    private void TranslateStack(StackBase stackBase)
    {
        foreach (IView child in stackBase.Children)
        {
            string dictionaryText;

            if (child is Label label)
            {
                dictionaryText = GetItem(label.StyleId);

                if (!string.IsNullOrEmpty(dictionaryText))
                    label.Text = dictionaryText;
            }
            else if (child is Button button)
            {
                dictionaryText = GetItem(button.StyleId);

                if (!string.IsNullOrEmpty(dictionaryText))
                    button.Text = dictionaryText;
            }
            else if (child is SearchBar searchBar)
            {
                dictionaryText = GetItem(searchBar.StyleId);

                if (!string.IsNullOrEmpty(dictionaryText))
                    searchBar.Placeholder = dictionaryText;
            }
            else if (child is StackBase)
            {
                TranslateStack(child as StackBase);
            }
        }
    }

    #endregion
}
