////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2022 Eppie(https://eppie.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tuvi.RestClient
{
    public abstract class Message
    {
        public abstract Uri Endpoint { get; }
        public abstract HttpMethod Method { get; }
        public HttpStatusCode HttpStatus { get; internal set; }
        internal abstract Task<HttpRequestMessage> CreateRequestAsync(Uri baseUri, CancellationToken cancellationToken);
        internal abstract Task CreateResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken);
    }

    public abstract class Message<TRequest, TResponse> : Message
            where TRequest : Request, new()
            where TResponse : Response, new ()
    {

        public TRequest Request { get; protected set; }
        public TResponse Response { get; protected set; }

        protected Message()
        {
            Request = new TRequest();
            Response = new TResponse();
        }

        internal override async Task<HttpRequestMessage> CreateRequestAsync(Uri baseUri, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(Method, BuildUri(baseUri, Endpoint, Request?.QueryString));

            if (Request != null)
            {
                Request.Headers?.UpdateHeaders(request);

                // ToDo should be 'Dispose' or use 'using'
                var content = await Request.GetContentAsync(cancellationToken).ConfigureAwait(false);
                if (content != null)
                {
                    request.Content = content;
                }
            }

            return request;
        }

        internal override async Task CreateResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            HttpStatus = response.StatusCode;

             response.EnsureSuccessStatusCode();

            if(Response != null)
            {
                await Response.ContentAsync(response.Content, cancellationToken).ConfigureAwait(false);
            }
        }

        internal static Uri BuildUri(Uri baseUri, Uri relativeUri, string query)
        {
            Uri uri = (baseUri is null) ? relativeUri : new Uri(baseUri, relativeUri);

            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = query;

            return uriBuilder.Uri;
        }
    }
}
