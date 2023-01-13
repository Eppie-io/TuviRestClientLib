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
    public class Client
    {
        private HttpClient _httpClient;
        private Uri _baseUri;

        public Client(HttpClient httpClient, Uri baseUri)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _httpClient = httpClient;
            _baseUri = baseUri;
        }

        public Task SendAsync(Message msg)
        {
            return SendAsync(msg, CancellationToken.None);
        }

        public async Task<HttpStatusCode> SendAsync(Message msg, CancellationToken cancellationToken)
        {
            if(msg is null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

//            FormUrlEncodedContent

            using (var request = await msg.CreateRequestAsync(_baseUri, cancellationToken).ConfigureAwait(false))
            {
                using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    await msg.CreateResponseAsync(response, cancellationToken).ConfigureAwait(false);

                    return response.StatusCode;
                }
            }
        }
    }
}
