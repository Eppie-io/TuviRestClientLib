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

namespace Tuvi.RestClient
{
    public abstract class CommonQueryMessage<TResponse, TRequest> : CommonMessage<TResponse, TRequest>
            where TResponse : Response
            where TRequest : Request
    {
        protected abstract QueryParameters CreateQuery();

        internal override Uri BuildUri(Uri baseUri, Uri endpoint)
        {
            var query = CreateQuery();
            if (query is null)
            {
                throw new InvalidOperationException("URI query is not defined.");
            }

            return base.BuildUri(baseUri, new Uri($"{endpoint}?{query}", UriKind.Relative));
        }
    }

    public abstract class QueryMessage<TResponse, TRequest> : CommonQueryMessage<TResponse, TRequest>
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

    public abstract class QueryMessage<TResponse> : QueryMessage<TResponse, EmptyRequest>
        where TResponse : Response, new()
    { }
}
