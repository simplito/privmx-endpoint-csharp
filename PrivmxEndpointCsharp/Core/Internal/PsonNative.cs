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
using System.Runtime.InteropServices;

namespace PrivMX.Endpoint.Core.Internal
{
    internal class PsonNative
    {
        public enum Type {
            PSON_NULL,
            PSON_BOOL,
            PSON_INT32,
            PSON_INT64,
            PSON_FLOAT32,
            PSON_FLOAT64,
            PSON_STRING,
            PSON_BINARY,
            PSON_ARRAY,
            PSON_OBJECT,
            PSON_INVALID = 255
        }

        [DllImport("libPson")]
        public static extern Type pson_value_type(IntPtr value);

        [DllImport("libPson")]
        public static extern int pson_is_null(IntPtr value);

        [DllImport("libPson")]
        public static extern int pson_get_bool(IntPtr value, out bool result);

        [DllImport("libPson")]
        public static extern int pson_get_int32(IntPtr value, out int result);

        [DllImport("libPson")]
        public static extern int pson_get_int64(IntPtr value, out long result);

        [DllImport("libPson")]
        public static extern int pson_get_float32(IntPtr value, out float result);

        [DllImport("libPson")]
        public static extern int pson_get_float64(IntPtr value, out double result);

        [DllImport("libPson")]
        public static extern IntPtr pson_get_cstring(IntPtr value);

        [DllImport("libPson")]
        public static extern int pson_inspect_binary(IntPtr value, out IntPtr data, out int size);

        [DllImport("libPson")]
        public static extern int pson_get_array_size(IntPtr array, out int size);

        [DllImport("libPson")]
        public static extern IntPtr pson_get_array_value(IntPtr array, int offset);

        [DllImport("libPson")]
        public static extern int pson_open_object_iterator(IntPtr value, out IntPtr iterator);

        [DllImport("libPson")]
        public static extern int pson_object_iterator_next(IntPtr iterator, out IntPtr key, out IntPtr val);

        [DllImport("libPson")]
        public static extern void pson_close_object_iterator(IntPtr iterator);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_null();

        [DllImport("libPson")]
        public static extern IntPtr pson_new_bool(bool val);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_int32(int val);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_int64(long val);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_float32(float val);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_float64(double val);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_string(string val);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_binary(IntPtr data, int size);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_array();

        [DllImport("libPson")]
        public static extern int pson_add_array_value(IntPtr array, IntPtr value);

        [DllImport("libPson")]
        public static extern IntPtr pson_new_object();

        [DllImport("libPson")]
        public static extern int pson_set_object_value(IntPtr obj, string key, IntPtr value);

        [DllImport("libPson")]
        public static extern void pson_free_value(IntPtr value);
    }
}
