//
// PrivMX Endpoint C#
// Copyright © 2024 Simplito sp. z o.o.
//
// This file is part of the PrivMX Platform (https://privmx.dev).
// This software is Licensed under the MIT License.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

namespace PrivMX.Endpoint.Event.Models.EventDataSchema
{
    /// <summary>
    /// Versions of event data schema
    /// </summary>
    public enum Version : long
    {
        UNKNOWN = 0,
        VERSION_1 = 1,
        VERSION_5 = 5
    }
}