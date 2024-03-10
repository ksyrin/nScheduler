using Newtonsoft.Json;
using System.Text;

namespace nScheduler.Common.Extensions;

public static class StringExtension
{
    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Encode(this string str)
    {
        byte[] base64 = Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(base64);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Decode(this string str)
    {
        byte[] value = Convert.FromBase64String(str);
        return Encoding.UTF8.GetString(value);
    }

    /// <summary>
    /// 转换为Guid
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static Guid ToGuid(this string str)
    {
        return Guid.TryParse(str, out var result) ?
            result :
            throw new InvalidDataException("无法转换为Guid");
    }

    /// <summary>
    /// 将Guid转换为统一的字符串
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string ToStringN(this Guid guid)
    {
        return guid.ToString("N");
    }

    /// <summary>
    /// 转换成Json字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    /// <summary>
    /// 转换成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T ToObject<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str) ?? throw new Exception("JSON转换失败");
    }
}