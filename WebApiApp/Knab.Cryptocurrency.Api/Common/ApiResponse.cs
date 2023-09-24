namespace Knab.Cryptocurrency.Api.Common;

public class ApiResponse<T>
{
    public int ResponseCode { get; set; }
    public string? ResponseDescription { get; set; }
    public T Data { get; set; }
}