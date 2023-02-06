////////////////////////////////////////////////////////////////////////////////
//
//   Copyright 2023 Eppie(https://eppie.io)
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

    public abstract class CommonMessage<TResponse, TRequest> : Message
            where TResponse : Response
            where TRequest : Request
    {
        public TRequest Request { get; private set; }
        public TResponse Response { get; private set; }

        protected abstract TRequest CreateRequest();
        protected abstract TResponse CreateResponse();

        internal override async Task<HttpRequestMessage> CreateRequestAsync(Uri baseUri, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(Method, BuildUri(baseUri, Endpoint));

            Request = CreateRequest();
            if (Request != null)
            {
                Request.Headers?.UpdateHeaders(request);
                request.Content = await Request.CreateContentAsync(cancellationToken).ConfigureAwait(false);
            }

            return request;
        }

        internal override async Task CreateResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            HttpStatus = response.StatusCode;

            Response = CreateResponse();
            if (Response != null)
            {
                Response.Headers = new HeaderCollection(response.Headers);
                await Response.ReadContentAsync(response.Content, cancellationToken).ConfigureAwait(false);
            }

            response.EnsureSuccessStatusCode();
        }

        internal virtual Uri BuildUri(Uri baseUri, Uri endpoint)
        {
            return (baseUri is null) ? endpoint : new Uri(baseUri, endpoint);
        }
    }

    public abstract class Message<TResponse, TRequest> : CommonMessage<TResponse, TRequest>
            where TResponse : Response, new()
            where TRequest : Request, new()
    {
        protected sealed override TRequest CreateRequest()
        {
            return new TRequest();
        }

        protected sealed override TResponse CreateResponse()
        {
            return new TResponse();
        }
    }

    public abstract class Message<TResponse> : Message<TResponse, EmptyRequest>
        where TResponse : Response, new()
    { }
}
