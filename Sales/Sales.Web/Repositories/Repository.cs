using Sales.Web.Repositories;
using System.Text;
using System.Text.Json;
using Web.Repositories;

namespace Sales.WEB.Repositories
{
    public class Repository : IRepository
    {
        // Creamos la propiedad pripada del HttpClient
        private readonly HttpClient _httpClient;

        // Propiedad _jsonDefaultOptions para volver universal el json por si las propiedades llegan en mayusculas o minusculas
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        // Inyectamos el  HttpClient en el constructor(inyeccion de dependencias) que se inyecta en el Program.cs
        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Metodo Get generico 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">URL del servicio</param>
        /// <returns></returns>
        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            // Obtenemos la peticion del servicio
            var responseHttp = await _httpClient.GetAsync(url);
            // Validamos que todo funcione bien
            if (responseHttp.IsSuccessStatusCode)
            {
                // 
                var response = await UnserializeAnswer<T>(responseHttp, _jsonDefaultOptions);
                // Si todo sale bien retornamos la respuesta, con los parametros, response = la respúesta, false = que no hubo error, responseHttp = codigo del http
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }

            // Si todo sale mal retornamos el error, con los parametros, default = por que no encontro nada, true = fue un error, responseHttp = codigo del error
            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            var mesageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(mesageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }


        /// <summary>
        /// Metodo que controla las respuestas recibidas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpResponse">La respuesta</param>
        /// <param name="jsonSerializerOptions">El Json </param>
        /// <returns></returns>
        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions)!;
        }
    }
}
