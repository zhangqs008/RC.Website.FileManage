//--------------------------------------------------------------------------------
// 文件描述：Object对象扩展类
// 文件作者：全体开发人员
// 创建日期：2016-06-27
//--------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace RC.FileManage
{
    /// <summary>
    ///     [扩展类]对象级别扩展类
    /// </summary>
    public static class ObjExtension
    {     
        /// <summary>
        ///     DataTable转Json文本
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dataTable)
        {
            var javaScriptSerializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var arrayList = new ArrayList();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var dictionary =
                    dataTable.Columns.Cast<DataColumn>().ToDictionary<DataColumn, string, object>(
                        dataColumn => dataColumn.ColumnName, dataColumn => dataRow[dataColumn.ColumnName].ToStr());
                //实例化一个参数集合
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
            return javaScriptSerializer.Serialize(arrayList);
        }
        /// <summary>
        ///     重写ToString()的方法，该方法首先判断object是否为空，如果对象为空则返回string.Empty，否则返回该对象的字符串表现形式
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>字符串</returns>
        public static string ToStr(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static string ToStr(this object obj, string def)
        {
            return obj == null ? def : obj.ToString();
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效数字表现形式，转换失败后默认返回0
        /// </summary>
        /// <param name="obj">需要转换的字符串</param>
        /// <returns>整数</returns>
        public static int ToInt(this object obj)
        {
            return ToInt32(obj, 0);
        }

        /// <summary>
        ///     将日期转换为字符串，默认格式：yyyy-MM-dd
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateString(this object obj, string format = "yyyy-MM-dd")
        {
            return obj.ToDateTime().ToString(format);
        }

        /// <summary>
        ///     将时间转换为字符串，默认格式：yyyy-MM-dd HH:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this object obj, string format = "yyyy-MM-dd HH:ss")
        {
            return obj.ToDateTime().ToString(format);
        }

        /// <summary>
        ///     Json格式验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSafeJSON(this string input)
        {
            var stringBuilder = new StringBuilder(input.Length);
            foreach (var c in input)
                if (char.IsControl(c) || c == 39)
                {
                    int num = c;
                    stringBuilder.Append("\\u" + num.ToString("x4"));
                }
                else
                {
                    if (c == 34 || c == 92 || c == 47)
                        stringBuilder.Append('\\');
                    stringBuilder.Append(c);
                }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效数字表现形式，转换失败后默认返回“0”
        /// </summary>
        /// <param name="obj">需要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认返回值</param>
        /// <returns>整数</returns>
        public static int ToInt(this object obj, int defaultVal)
        {
            return ToInt32(obj, defaultVal);
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效数字表现形式，转换失败后默认返回“0”
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static int ToInt32(this object str)
        {
            return ToInt32(str, 0);
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效数字表现形式，转换失败后默认返回“0”
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认返回值</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static int ToInt32(this object str, int defaultVal)
        {
            if (str == null) return defaultVal;

            var result = defaultVal;

            result = int.TryParse(str.ToString(), out defaultVal) ? defaultVal : result;

            return result;
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效长整型表现形式，转换失败后默认返回“0”
        /// </summary>
        /// <param name="str">需要转换的对象</param>
        /// <returns>长整型数字表现形式</returns>
        public static long ToInt64(this object str)
        {
            long defaultVal;
            if (str == null) return 0;
            long.TryParse(str.ToString(), out defaultVal);
            return defaultVal;
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效十进制数表现形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static decimal ToDecimal(this object str)
        {
            return ToDecimal(str, 0);
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效十进制数表现形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认返回值</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static decimal ToDecimal(this object str, decimal defaultVal)
        {
            if (str.ToStr().IsEmpty()) return defaultVal;

            decimal.TryParse(str.ToStr(), out defaultVal);

            return defaultVal;
        }

        public static double ToDouble(this object str)
        {
            return ToDouble(str, 0.0);
        }

        public static double ToDouble(this object str, double defaultVal)
        {
            if (str.ToStr().IsEmpty()) return defaultVal;

            double.TryParse(str.ToStr(), out defaultVal);

            return defaultVal;
        }

        /// <summary>
        ///     四舍五入格式化货币，默认保留两位小数，格式不对将返回0.00
        ///     格式前: 163.2545 格式结果: 163.25
        /// </summary>
        /// <param name="dDecimal">需要转换的货币数字</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static decimal ToDecimalFormat(this object dDecimal)
        {
            return Math.Round(dDecimal.ToDecimal(), 2, MidpointRounding.AwayFromZero).ToString("F2").ToDecimal();
        }

        /// <summary>
        ///     四舍五入格式化货币，指定小数位数，格式不对将返回0.小数点位数
        ///     格式前: 163.2545 格式结果: 163.25
        /// </summary>
        /// <param name="dDecimal">需要转换的货币数字</param>
        /// <param name="decimals">小数位置</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static decimal ToDecimalFormat(this string dDecimal, int decimals)
        {
            return
                Math.Round(dDecimal.ToDecimal(), decimals, MidpointRounding.AwayFromZero).ToString("F" + decimals)
                    .ToDecimal();
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效数字表现形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static long ToLong(this object str)
        {
            return ToLong(str, 0);
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效数字表现形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认返回值</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static long ToLong(this object str, long defaultVal)
        {
            if (str == null) return defaultVal;

            long.TryParse(str.ToString(), out defaultVal);

            return defaultVal;
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效日期时间表现形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static DateTime ToDateTime(this object str)
        {
            var dt = new DateTime(1900, 1, 1);

            return string.IsNullOrEmpty(str.ToStr()) ? dt : ToDateTime(str, dt);
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效日期时间表现形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认返回值</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static DateTime ToDateTime(this object str, DateTime defaultVal)
        {
            if (str == null || str.ToStr().IsEmpty()) return defaultVal;
            var result = defaultVal;
            if (!DateTime.TryParse(str.ToString(), out result)) result = defaultVal;
            return result;
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效布尔值表现形式，默认中(不区分大小写) True，1，Y 都为True类型
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static bool ToBoolean(this object str)
        {
            return ToBoolean(str, false);
        }

        /// <summary>
        ///     将此实例的字符串转换为它的有效布尔值表现形式，默认中(不区分大小写) True，1，Y 都为True类型
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="defaultVal">转换失败后的默认返回值</param>
        /// <returns>返回转换后的数字表现形式</returns>
        public static bool ToBoolean(this object str, bool defaultVal)
        {
            if (string.IsNullOrEmpty(str.ToStr()))
                return defaultVal;

            return str.ToString().ToLower() == "true" || str.ToString().ToLower() == "1" ||
                   str.ToString().ToLower() == "y" || str.ToString() == "是";
        }

        /// <summary>
        ///     将对象序列化为Xml串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml(this object obj)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    var xz = new XmlSerializer(obj.GetType());
                    xz.Serialize(sw, obj);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///     将一个对象转成JSON字符串。
        /// </summary>
        /// <param name="obj">需要转成JSON的对象。</param>
        /// <returns>返回JSON格式字符串。</returns>
        public static string ToJson(this object obj)
        {
            return JsonSerialize(obj);
        }

        /// <summary>
        ///     Json序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T t)
        {
            try
            {
                var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                return serializer.Serialize(t);
            }
            catch (Exception ex)
            {
            }

            return string.Empty;
        }


        /// <summary>
        ///     获取字段描述
        /// </summary>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumVal)
        {
            var field = enumVal.GetType().GetField(enumVal.ToString());
            if (field != null)
                foreach (DescriptionAttribute desc in field.GetCustomAttributes(typeof(DescriptionAttribute), false))
                    return desc.Description;
            return "";
        }


        public static bool IsEmpty(this object val)
        {
            if (val == null) return true;
            return string.IsNullOrEmpty(val.ToString());
        }

        public static bool IsNotEmpty(this object val)
        {
            return !IsEmpty(val);
        }
    }
}