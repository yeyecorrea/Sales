using Web.Repositories;

namespace Sales.Web.Repositories
{
    public interface IRepository
    {
        // Metodo get ue recibe como parametro la url del servicio
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        // Metodo pos que no retorna nada
        Task<HttpResponseWrapper<object>> Post<T>(string url, T model);
        // Metodo pos que nos retorna un TResponse que es el objecto(Body)
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model);


    }
}
