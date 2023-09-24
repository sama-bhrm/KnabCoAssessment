using Knab.Cryptocurrency.Core.Common;

namespace Knab.Cryptocurrency.Core.Entities;

public class Response<T>
{
    public StatusCode StatusCode { get; set; }
    public int ResponseCode { get; set; }
    public string? ResponseDescription { get; set; }
    public T Data { get; set; }
}