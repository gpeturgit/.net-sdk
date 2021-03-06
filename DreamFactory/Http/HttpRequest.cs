﻿namespace DreamFactory.Http
{
    using System;

    /// <summary>
    /// Represents generic HTTP request.
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">URL.</param>
        /// <param name="headers">Headers collection.</param>
        public HttpRequest(HttpMethod method, string url, IHttpHeaders headers)
        {
            HttpUtils.CheckUrlString(url);

            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            Method = method;
            Url = url;
            Headers = headers.Exclude(HttpHeaders.ContentTypeHeader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">URL.</param>
        /// <param name="headers">Headers collection.</param>
        /// <param name="body">Body content.</param>
        public HttpRequest(HttpMethod method, string url, IHttpHeaders headers, string body)
        {
            HttpUtils.CheckUrlString(url);

            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            Body = body;
            Method = method;
            Url = url;
            Headers = headers;
        }

        /// <inheritdoc />
        public HttpMethod Method { get; private set; }

        /// <inheritdoc />
        public string Url { get; private set; }

        /// <inheritdoc />
        public string Body { get; private set; }

        /// <inheritdoc />
        public IHttpHeaders Headers { get; private set; }

        /// <inheritdoc />
        public void SetTunnelingWith(HttpMethod method)
        {
            string httpMethod = HttpUtils.GetHttpMethodName(Method);
            Headers = Headers.Include(HttpHeaders.TunnelingHeader, httpMethod);
            Method = method;
        }
    }
}
