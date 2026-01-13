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

using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Core
{
    /// <summary>
    /// UserVerifierInterface - an interface consisting of a single verify() method, which - when implemented - should
    /// perform verification of the provided data using an external service verification should be done using
    /// an external service such as an application server or a PKI server.
    /// </summary>
    public abstract class UserVerifierInterface
    {
        /// <summary>
        /// Verifies whether the specified users are valid. Checks if each user belonged to the Context and if this is
        /// their key in `date` and return `true` or `false` otherwise.
        /// </summary>
        /// <param name="request">List of user data to verification</param>
        /// <returns>List of verification results whose items correspond to the items in the input list</returns>
        public abstract List<bool> Verify(List<VerificationRequest> request);
    }
}