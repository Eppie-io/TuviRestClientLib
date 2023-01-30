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

using NUnit.Framework;
using System.Collections;

namespace Tuvi.RestClient.Test.Data
{
    internal partial class QueryParametersTestData
    {
        public static IEnumerable QueryUnicodeParams
        {
            get
            {
                yield return new TestCaseData(new[] { ("param", "üúûùūēāīїšўє") }, "param=%c3%bc%c3%ba%c3%bb%c3%b9%c5%ab%c4%93%c4%81%c4%ab%d1%97%c5%a1%d1%9e%d1%94");
                yield return new TestCaseData(new[] { ("param", "獨角獸是種傳說生物形象通常為頭上長有独角的白馬") }, "param=%e7%8d%a8%e8%a7%92%e7%8d%b8%e6%98%af%e7%a8%ae%e5%82%b3%e8%aa%aa%e7%94%9f%e7%89%a9%e5%bd%a2%e8%b1%a1%e9%80%9a%e5%b8%b8%e7%82%ba%e9%a0%ad%e4%b8%8a%e9%95%b7%e6%9c%89%e7%8b%ac%e8%a7%92%e7%9a%84%e7%99%bd%e9%a6%ac");
            }
        }
    }
}
