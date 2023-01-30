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
    internal partial class HeaderCollectionTestData
    {
        public static IEnumerable HeaderTupleParams
        {
            get
            {
                yield return new TestCaseData(
                    new[] { ("header1", "value1"), ("header2", "value2"), ("header3", "value3"), ("header4", "value4") }
                );

                yield return new TestCaseData(
                    new[] { ("header-name", "value1"), ("header-name", "value2"), ("header-name", "value3"), ("header-name", "value4") }
                );
            }
        }

        public static IEnumerable HeaderPairParams
        {
            get
            {
                yield return new TestCaseData(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "header1", new [] { "value1", "value2", "value3" } },
                        { "header2", new [] { "value1", "value2" } },
                        { "header3", new [] { "value1" } },
                        { "header-name", new [] { "value1" } },
                        { "wrong-name-@#$%^)(*&^%", new [] { "value1" } },
                    },
                    new[]
                    {
                        ( "header1", "value1" ),
                        ( "header2", "value2" ),
                        ( "header3", "value1" ),
                        ( "header1", "value3" ),
                        ( "header-name", "value1" ),
                    },
                    new[]
                    {
                        ( "header4", false ),
                        ( "header5", false ),
                        ( "wrong-name-@#$%^()*&^%", true ),
                    }
                );
            }
        }
    }
}
