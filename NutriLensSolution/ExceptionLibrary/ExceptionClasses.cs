namespace ExceptionLibrary;

/// <summary>
/// Exceção para controle/sinalização para o caso de algum item não ser encontrado em qualquer contexto aplicável
/// </summary>
[Serializable]
public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base() { }
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para controle/sinalização de erros no envio de notificações (WhatsApp/Email e quaisquer outros contextos aplicáveis).
/// </summary>
[Serializable]
public class SendNotificationsException : Exception
{
    public SendNotificationsException() : base() { }
    public SendNotificationsException(string message) : base(message) { }
    public SendNotificationsException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para controle/sinalização para o caso de algum item já ter sido registrado em qualquer contexto aplicável
/// </summary>
[Serializable]
public class AlreadyRegisteredException : Exception
{
    public AlreadyRegisteredException() : base() { }
    public AlreadyRegisteredException(string message) : base(message) { }
    public AlreadyRegisteredException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para controle/sinalização para o caso de algum item não ser encontrado em qualquer contexto aplicável
/// </summary>
[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException() : base() { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para controle/sinalização para o caso de algum item não ser encontrado em qualquer contexto aplicável
/// </summary>
[Serializable]
public class DatabaseQueryException : Exception
{
    public DatabaseQueryException() : base() { }
    public DatabaseQueryException(string message) : base(message) { }
    public DatabaseQueryException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para controle/sinalização para o caso de algum valor inválido
/// </summary>
[Serializable]
public class InvalidValueException : Exception
{
    public InvalidValueException() : base() { }
    public InvalidValueException(string message) : base(message) { }
    public InvalidValueException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para controle/sinalização em leituras de content de HttpResponseMessage
/// </summary>
[Serializable]
public class HttpContentParseException : Exception
{
    public HttpContentParseException() : base() { }
    public HttpContentParseException(string message) : base(message) { }
    public HttpContentParseException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Exceção para indicar falha na escrita/sobrescrita de um arquivo
/// </summary>
[Serializable]
public class FileWriteUpdateException : Exception
{
    public FileWriteUpdateException() : base() { }
    public FileWriteUpdateException(string message) : base(message) { }
    public FileWriteUpdateException(string message, Exception inner) : base(message, inner) { }
}