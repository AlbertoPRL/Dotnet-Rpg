namespace dotnet_rpg.Model;

public class ServiceResponse<T>
{
    private string _message = string.Empty;
    public T? Data { get; set; }
    public bool Succes { get; private set; } = true;
    public string Message
    {
        get
        {
            return _message;
        }
        set
        {
            _message = value;
            Succes = false;
        }
    }
}
