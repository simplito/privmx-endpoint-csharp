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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PrivMX.Endpoint.Core.Internal
{
    internal class Mapper
    {
        readonly Dictionary<string, Type> registeredTypes;

        public Mapper(Dictionary<string, Type> registeredTypes)
        {
            this.registeredTypes = registeredTypes;
        }

        public IntPtr MapToDynamicValue(object? obj)
        {
            if (obj is null)
            {
                return PsonNative.pson_new_null();
            }
            if (obj is bool boolVal)
            {
                return PsonNative.pson_new_bool(boolVal);
            }
            if (obj is int intVal)
            {
                return PsonNative.pson_new_int32(intVal);
            }
            if (obj is long longVal)
            {
                return PsonNative.pson_new_int64(longVal);
            }
            if (obj is float floatVal)
            {
                return PsonNative.pson_new_float32(floatVal);
            }
            if (obj is double doubleVal)
            {
                return PsonNative.pson_new_float64(doubleVal);
            }
            if (obj is string stringVal)
            {
                return PsonNative.pson_new_string(stringVal);
            }
            if (obj is byte[] bytesVal)
            {
                IntPtr bufPtr = Marshal.AllocHGlobal(bytesVal.Length);
                Marshal.Copy(bytesVal, 0, bufPtr, bytesVal.Length);
                IntPtr psonBytes = PsonNative.pson_new_binary(bufPtr, bytesVal.Length);
                Marshal.FreeHGlobal(bufPtr);
                return psonBytes;
            }
            if (obj is IList list)
            {
                IntPtr psonArray = PsonNative.pson_new_array();
                foreach (object value in list) {
                    IntPtr psonValue = MapToDynamicValue(value);
                    PsonNative.pson_add_array_value(psonArray, psonValue);
                    PsonNative.pson_free_value(psonValue);
                }
                return psonArray;

            }
            var properties = obj.GetType().GetProperties();
            IntPtr psonObj = PsonNative.pson_new_object();
            foreach (var property in properties)
            {
                IntPtr psonValue = MapToDynamicValue(property.GetValue(obj));
                PsonNative.pson_set_object_value(psonObj, Name2camelCase(property.Name), psonValue);
                PsonNative.pson_free_value(psonValue);
            }
            return psonObj;
        }

        public T? ParseFromDynamicValue<T>(IntPtr value) where T : class
        {
            return (T?)ParseFromDynamicValue(value, typeof(T));
        }

        private object? ParseFromDynamicValue(IntPtr value, Type type)
        {
            PsonNative.Type psonType = PsonNative.pson_value_type(value);
            switch (psonType)
            {
                case PsonNative.Type.PSON_NULL:
                    return null;
                case PsonNative.Type.PSON_BOOL:
                    {
                        PsonNative.pson_get_bool(value, out bool val);
                        return val;
                    }
                case PsonNative.Type.PSON_INT32:
                    {
                        PsonNative.pson_get_int32(value, out int val);
                        return val;
                    }
                case PsonNative.Type.PSON_INT64:
                    {
                        PsonNative.pson_get_int64(value, out long val);
                        return val;
                    }
                case PsonNative.Type.PSON_FLOAT32:
                    {
                        PsonNative.pson_get_float32(value, out float val);
                        return val;
                    }
                case PsonNative.Type.PSON_FLOAT64:
                    {
                        PsonNative.pson_get_float64(value, out double val);
                        return val;
                    }
                case PsonNative.Type.PSON_STRING:
                    {
                        IntPtr val = PsonNative.pson_get_cstring(value);
                        return Marshal.PtrToStringUTF8(val);
                    }
                case PsonNative.Type.PSON_BINARY:
                    {
                        PsonNative.pson_inspect_binary(value, out IntPtr val, out int size);
                        byte[] res = new byte[size];
                        Marshal.Copy(val, res, 0, size);
                        return res;
                    }
                case PsonNative.Type.PSON_ARRAY:
                    {
                        PsonNative.pson_get_array_size(value, out int size);
                        object? list = Activator.CreateInstance(type);
                        var method = type.GetMethod("Add");
                        for (int i = 0; i < size; ++i) {
                            IntPtr element = PsonNative.pson_get_array_value(value, i);
                            method?.Invoke(list, new object?[]{ParseFromDynamicValue(element, type.GetGenericArguments()[0])});
                        }
                        return list;
                    }
                case PsonNative.Type.PSON_OBJECT:
                    {
                        Type objType = TryResolveRegisteredType(value) ?? type;
                        object? obj = Activator.CreateInstance(objType);
                        if (obj is null)
                        {
                            return null;
                        }
                        if (PsonNative.pson_open_object_iterator(value, out IntPtr it) != 0) {
                            while (PsonNative.pson_object_iterator_next(it, out IntPtr key, out IntPtr val) != 0) {
                                string? keyStr = Marshal.PtrToStringUTF8(key);
                                if (keyStr is null)
                                {
                                    continue;
                                }
                                var property = objType.GetProperty(Name2PascalCase(keyStr));
                                if (property is null)
                                {
                                    continue;
                                }
                                property.SetValue(obj, ParseFromDynamicValue(val, property.PropertyType));
                            }
                        }
                        PsonNative.pson_close_object_iterator(it);
                        return obj;
                    }
                case PsonNative.Type.PSON_INVALID:
                default:
                    return null;
            }
        }

        public static void FreeDynamicValue(IntPtr value)
        {
            PsonNative.pson_free_value(value);
        }

        private Type? TryResolveRegisteredType(IntPtr value)
        {
            if (PsonNative.pson_open_object_iterator(value, out IntPtr it) != 0)
            {
                while (PsonNative.pson_object_iterator_next(it, out IntPtr key, out IntPtr psonValue) != 0)
                {
                    string? keyStr = Marshal.PtrToStringUTF8(key);
                    if (string.Equals(keyStr, "__type")) {
                        string? typeStr = ParseFromDynamicValue<string>(psonValue);
                        if (!(typeStr is null) && registeredTypes.TryGetValue(typeStr, out Type? type))
                        {
                            PsonNative.pson_close_object_iterator(it);
                            return type;
                        }
                        break;
                    }
                }
            }
            PsonNative.pson_close_object_iterator(it);
            return null;
        }

        private static string Name2PascalCase(string name)
        {
            return string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1).ToString());
        }

        private static string Name2camelCase(string name)
        {
            return string.Concat(name[0].ToString().ToLower(), name.AsSpan(1).ToString());
        }
    }
}
