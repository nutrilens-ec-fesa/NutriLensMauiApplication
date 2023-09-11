namespace ExceptionLibrary;

/// <summary>
/// Classe de controle de Exceções
/// </summary>
public static class ExceptionManager
{
    /// <summary>
    /// Obtém a mensagem de exceção e seu inner (se houver)
    /// </summary>
    /// <param name="ex">Exceção</param>
    /// <returns>Mensagem de exceção</returns>
    public static string ExceptionMessage(Exception ex)
    {
        if (ex == null) // proteção contra exception nula
            return "null";
        else
        {
            string msg = $"{ex.Message}"; // inicializa a mensagem com a mensagem da exception

            if (ex.InnerException != null) // verifica se existe uma innerException
                msg += $" - in: {ex.InnerException.Message}"; // obtém a mensagem da innerException

            return msg += $" - {ex.GetType()}"; // adiciona o tipo da exception (é útil saber o tipo para fazer proteções específicas no código onde estorou a exceção)
        }
    }
}