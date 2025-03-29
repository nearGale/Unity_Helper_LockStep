using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace Utils.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        
        /// <summary>
        /// 首字母小写
        /// </summary>
        public static string FirstLower(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }
            return value.Length > 1 ? value.First().ToString().ToLower() + value[1..] : value.First().ToString().ToLower();
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        public static string FirstUpper(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }
            return value.Length > 1 ? value.First().ToString().ToUpper() + value[1..] : value.First().ToString().ToUpper();
        }

        public static IPEndPoint ToIPEndPoint(this string ipAddr)
        {
            var addressParts = ipAddr.Split(':');
            var ipAddressStr = addressParts[0];
            var port = int.Parse(addressParts[1]);
            var ipAddress = IPAddress.Parse(ipAddressStr);
            return new(ipAddress, port);
        }
        
        /// <summary>
        /// 迭代器转字符串
        /// </summary>
        public static string GetString<T>(this IEnumerable<T> list, string separator = null)
        {
            separator ??= ", ";
            var sb = new StringBuilder();
            sb.AppendJoin(separator, list);
            return sb.ToString();
        }

        #region Converter
        public static sbyte ConvertToSByte(this string value, sbyte defaultValue = default)
        {
            var ok = sbyte.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static short ConvertToShort(this string value, short defaultValue = default)
        {
            var ok = short.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static int ConvertToInt(this string value, int defaultValue = default)
        {
            var ok = int.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static long ConvertToLong(this string value, long defaultValue = default)
        {
            var ok = long.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static byte ConvertToByte(this string value, byte defaultValue = default)
        {
            var ok = byte.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static ushort ConvertToUShort(this string value, ushort defaultValue = default)
        {
            var ok = ushort.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static uint ConvertToUInt(this string value, uint defaultValue = default)
        {
            var ok = uint.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static ulong ConvertToULong(this string value, ulong defaultValue = default)
        {
            var ok = ulong.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static float ConvertToFloat(this string value, float defaultValue = default)
        {
            var ok = float.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static double ConvertToDouble(this string value, double defaultValue = default)
        {
            var ok = double.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        public static bool ConvertToBool(this string value, bool defaultValue = default)
        {
            var ok = bool.TryParse(value, out var result);
            return ok ? result : defaultValue;
        }

        /// <summary>
        /// 替换 \n 转义符
        /// </summary>
        public static string ConvertToString(this string value, bool defaultValue = default)
        {
            return value.Replace(@"\n", "\n");
        }

        public static sbyte[] ConvertToSByteAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new sbyte[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToSByte()).ToArray();
        }

        public static sbyte[][] ConvertToSByteAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new sbyte[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToSByte()).ToArray()).ToArray();
        }

        public static short[] ConvertToShortAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new short[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToShort()).ToArray();
        }

        public static short[][] ConvertToShortAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new short[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToShort()).ToArray()).ToArray();
        }

        public static int[] ConvertToIntAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new int[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToInt()).ToArray();
        }

        public static int[][] ConvertToIntAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new int[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToInt()).ToArray()).ToArray();
        }

        public static T ConvertToEnum<T>(this string value) where T : Enum
        {
            if (value.IsNullOrEmpty())
            {
                return default(T);
            }

            var succes = Enum.TryParse(typeof(T), value, out var res);
            if (succes)
            {
                return (T)res;
            }

            return default(T);
        }

        public static T[] ConvertToEnumAry<T>(this string value, char separator = default) where T : Enum
        {
            if (value.IsNullOrEmpty())
            {
                return new T[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToEnum<T>()).ToArray();
        }

        public static T[][] ConvertToEnumAryAry<T>(this string value, char arySeparator, char separator = default)
            where T : Enum
        {
            if (value.IsNullOrEmpty())
            {
                return new T[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToEnum<T>()).ToArray())
                .ToArray();
        }

        public static long[] ConvertToLongAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new long[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToLong()).ToArray();
        }

        public static long[][] ConvertToLongAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new long[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToLong()).ToArray()).ToArray();
        }

        public static byte[] ConvertToByteAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new byte[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToByte()).ToArray();
        }

        public static byte[][] ConvertToByteAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new byte[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToByte()).ToArray()).ToArray();
        }

        public static ushort[] ConvertToUShortAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new ushort[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToUShort()).ToArray();
        }

        public static ushort[][] ConvertToUShortAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new ushort[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToUShort()).ToArray()).ToArray();
        }

        public static uint[] ConvertToUIntAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new uint[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToUInt()).ToArray();
        }

        public static uint[][] ConvertToUIntAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new uint[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToUInt()).ToArray()).ToArray();
        }

        public static ulong[] ConvertToULongAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new ulong[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToULong()).ToArray();
        }

        public static ulong[][] ConvertToULongAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new ulong[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToULong()).ToArray()).ToArray();
        }

        public static float[] ConvertToFloatAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new float[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToFloat()).ToArray();
        }

        public static float[][] ConvertToFloatAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new float[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToFloat()).ToArray()).ToArray();
        }

        public static double[] ConvertToDoubleAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new double[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToDouble()).ToArray();
        }

        public static double[][] ConvertToDoubleAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new double[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToDouble()).ToArray()).ToArray();
        }

        public static bool[] ConvertToBoolAry(this string value, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new bool[] { };
            }

            var separatorStr = value.Split(separator);
            return separatorStr.Select(item => item.ConvertToBool()).ToArray();
        }

        public static bool[][] ConvertToBoolAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new bool[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr
                .Select(itemAry => itemAry.Split(separator).Select(item => item.ConvertToBool()).ToArray()).ToArray();
        }

        public static string[] ConvertToStringAry(this string value, char separator = default)
        {
            return value.IsNullOrEmpty() ? new string[] { } : value.Split(separator).ToArray();
        }

        public static string[][] ConvertToStringAryAry(this string value, char arySeparator, char separator = default)
        {
            if (value.IsNullOrEmpty())
            {
                return new string[][] { };
            }

            var separatorStr = value.Split(arySeparator);
            return separatorStr.Select(itemAry => itemAry.Split(separator).ToArray()).ToArray();
        }

        public static string ConvertToCSVGrid(this sbyte value) => value.ToString();
        public static string ConvertToCSVGrid(this short value) => value.ToString();
        public static string ConvertToCSVGrid(this int value) => value.ToString();
        public static string ConvertToCSVGrid(this long value) => value.ToString();
        public static string ConvertToCSVGrid(this byte value) => value.ToString();
        public static string ConvertToCSVGrid(this ushort value) => value.ToString();
        public static string ConvertToCSVGrid(this uint value) => value.ToString();
        public static string ConvertToCSVGrid(this ulong value) => value.ToString();
        public static string ConvertToCSVGrid(this float value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ConvertToCSVGrid(this double value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ConvertToCSVGrid(this bool value) => value.ToString();

        public static string ConvertToCSVGrid(this string value) => value;

        public static string ConvertToCSVGrid<T>(this T value) where T : Enum => value.ToString();

        public static string ConvertToCSVGrid(this string[] value, char separator = default)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < value.Length - 1; i++)
            {
                builder.Append(value[i]);
                builder.Append(separator);
            }

            if (value.Length > 0)
            {
                builder.Append(value[^1]);
            }

            return builder.ToString();
        }

        public static string ConvertToCSVGrid(this sbyte[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this short[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this int[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this long[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this byte[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this ushort[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this uint[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this ulong[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this float[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this double[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this bool[] value, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid<T>(this T[] value, char separator = default) where T : Enum
        {
            return value.Select(v => v.ConvertToCSVGrid()).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this sbyte[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this short[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this int[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this long[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this byte[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this ushort[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this uint[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this ulong[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this float[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this double[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid(this bool[][] value, char arysSeparator, char separator = default)
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }

        public static string ConvertToCSVGrid<T>(this T[][] value, char arysSeparator, char separator = default)
            where T : Enum
        {
            return value.Select(v => v.ConvertToCSVGrid(arysSeparator)).ToArray().ConvertToCSVGrid(separator);
        }
        #endregion
    }
}