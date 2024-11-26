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

using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Core.Internal
{
    internal class Executor
    {
        private static readonly Mapper mapper = new Mapper(TypeRegistry.Types);

        private readonly INativeExecutor nativeExecutor;

        public Executor(INativeExecutor nativeExecutor)
        {
            this.nativeExecutor = nativeExecutor;
        }

        public void ExecuteVoid(IntPtr ptr, int method, List<object> args)
        {
            ExecuteOpt<object>(ptr, method, args);
        }

        public T Execute<T>(IntPtr ptr, int method, List<object> args)
        {
            return ExecuteOpt<T>(ptr, method, args) ?? throw new EndpointException("Unexpected error: Result is null");
        }

        public T ExecuteOpt<T>(IntPtr ptr, int method, List<object> args)
        {
            var dynamicValueArgs = mapper.MapToDynamicValue(args);
            nativeExecutor.Exec(ptr, (int)method, dynamicValueArgs, out var dynamicValueResult);
            ExecResult<T> nullableResult = mapper.ParseFromDynamicValue<ExecResult<T>>(dynamicValueResult);
            PsonNative.pson_free_value(dynamicValueArgs);
            PsonNative.pson_free_value(dynamicValueResult);
            if (nullableResult is null)
            {
                throw new EndpointException("Unexpected error: ExecResult<T> is null");
            }
            ExecResult<T> result = nullableResult;
            if (!(result.Error is null))
            {
                throw new EndpointNativeException(result.Error);
            }
            return result.Result;
        }
    }
}