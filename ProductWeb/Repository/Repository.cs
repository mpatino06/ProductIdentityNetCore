﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductWEB.Models;
using ProductWEB.Repository.IRepository;
using ProductWEB.Utility;

namespace ProductWEB.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IHttpClientFactory HttpClientFactory { get; }
        public Repository(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<ModelStateError> CreateAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, Resource.ContentType);
            var httpclient = HttpClientFactory.CreateClient();
            HttpResponseMessage response = await httpclient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }

            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };
        }

        public async Task<ModelStateError> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url + id);
            var httpclient = HttpClientFactory.CreateClient();
            HttpResponseMessage response = await httpclient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new ModelStateError()
                {
                    Response = new Response()
                    {
                        Errors = new List<Errors>()
                    {
                      new Errors(){ ErrorMessage = "No se encontró el producto que se desea eliminar"}
                    }
                    }

                };
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }
            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };

        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var httpclient = HttpClientFactory.CreateClient();
            HttpResponseMessage response = await httpclient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var httpclient = HttpClientFactory.CreateClient();
            HttpResponseMessage response = await httpclient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            return null;
        }

        public async Task<ModelStateError> UpdateAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, Resource.ContentType);
            var httpclient = HttpClientFactory.CreateClient();
            HttpResponseMessage response = await httpclient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new ModelStateError()
                {
                    Response = new Response()
                    {
                        Errors = new List<Errors>()
                    {
                      new Errors(){ ErrorMessage = "No se encontró el producto que se desea actualizar"}
                    }
                    }

                };

            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }

            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };
        }
    }
}
