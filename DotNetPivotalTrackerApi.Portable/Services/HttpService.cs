﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DotNetPivotalTrackerApi.Portable.Exceptions;

namespace DotNetPivotalTrackerApi.Portable.Services
{ 
    public class HttpService
    {
        private string _apiToken;
        public string ApiToken => _apiToken;

        private readonly string _baseUrl = "https://www.pivotaltracker.com/services/v5/";
        private readonly HttpClient HttpClient = new HttpClient();

        private StringContent CreateStringContent(string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public void SetupHttpClient(string apiToken)
        {
            _apiToken = apiToken;
            HttpClient.BaseAddress = new Uri(_baseUrl);
            HttpClient.DefaultRequestHeaders.Add("X-TrackerToken", _apiToken);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        /// <summary>
        /// Calls a GET request on the specified path.
        /// </summary>
        /// <param name="path">URL of the path to call as a string.</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            CheckTokenExists();
            return await HttpClient.GetAsync(path);        
        }

        /// <summary>
        /// Calls a POST request on the specified path, posting the model data as JSON.
        /// </summary>
        /// <typeparam name="T">Type to return deserialised JSON as.</typeparam>
        /// <param name="path">URL of the path to call as a string.</param>
        /// <param name="data">Model data to be serialised as JSON.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync<T>(string path, T data)
        {
            CheckTokenExists();
            var content = CreateStringContent(JsonService.SerializeObjectToJson(data));
            return await HttpClient.PostAsync(path, content);
        }

        /// <summary>
        /// Calls a POST request on the specified path, posting the model data as JSON.
        /// </summary>
        /// <typeparam name="T">Type to return deserialised JSON as.</typeparam>
        /// <param name="path">URL of the path to call as a string.</param>
        /// <param name="data">Model data to be serialised as JSON.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync<T>(string path, T data)
        {
            CheckTokenExists();
            var content = CreateStringContent(JsonService.SerializeObjectToJson(data));
            return await HttpClient.PutAsync(path, content);
        }

        /// <summary>
        /// Calls a POST request on the specified path, passing data as HttpContent.
        /// </summary>
        /// <typeparam name="T">Type to return deserialised JSON as.</typeparam>
        /// <param name="path">URL of the path to call as a string.</param>
        /// <param name="data">Data to be sent in the POST request as HttpContent</param>
        /// <param name="serialiseToJson">Whether or not to serialise as JSON. Calls <see cref="PostAsync{T}(string, T)"/> if true. Default: false.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostContentAsync<T>(string path, T data, bool serialiseToJson = false) where T : HttpContent
        {
            CheckTokenExists();
            if (serialiseToJson)
                return await PostAsync<T>(path, data);

            return await HttpClient.PostAsync(path, data);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string path)
        {
            CheckTokenExists();
            return await HttpClient.DeleteAsync(path);
        }

        private void CheckTokenExists()
        {
            if (string.IsNullOrEmpty(_apiToken))
                throw new PivotalAuthorisationException("Api Token has not been set. Please set it using HttpService.SetupHttpClient(string apiToken).");
        }
    }
}
