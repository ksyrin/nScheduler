using System.ComponentModel;
using System.Reflection;

namespace nScheduler.Common.Extensions;

public static class ReflectHelper
{
    #region 类相关

    /// <summary>
    /// 获取对象的DisplayName
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetTableColumns<TItem>(this TItem obj) where TItem : class, new()
    {
        Dictionary<string, string> result = new();
        var properties = obj.GetType().GetProperties();
        foreach (var property in properties)
        {
            // 获取显示名称
            var displayName = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayName != null)
            {
                result.Add(property.Name, displayName.DisplayName);
            }
        }

        return result;
    }

    #endregion 类相关

    #region 枚举

    /// <summary>
    /// 获取枚举的值和描述
    /// </summary>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static Dictionary<int, string> GetEnumDescriptions(this Enum @enum)
    {
        Dictionary<int, string> result = new();
        var fields = @enum.GetType().GetFields();
        foreach (var field in fields)
        {
            if (field.FieldType.IsEnum)
            {
                var descripion = field.GetCustomAttribute<DescriptionAttribute>();
                if (descripion != null)
                {
                    int value = (int)@enum.GetType().InvokeMember(field.Name,
                        BindingFlags.GetField, null, null, null)!;
                    result.Add(value, descripion.Description);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 获取枚举的值和描述
    /// </summary>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetEnumDescriptionString(this Enum @enum)
    {
        Dictionary<string, string> result = new();
        var fields = @enum.GetType().GetFields();
        foreach (var field in fields)
        {
            if (field.FieldType.IsEnum)
            {
                var descripion = field.GetCustomAttribute<DescriptionAttribute>();
                if (descripion != null)
                {
                    result.Add(field.Name, descripion.Description);
                }
            }
        }
        return result;
    }

    public static int GetEnumValue(this Enum @enum)
    {
        return (int)@enum.GetType().InvokeMember(@enum.ToString(), BindingFlags.GetField, null, null, null)!;
    }

    public static string GetEnumDescription(this Enum @enum)
    {
        string value = @enum.ToString();
        FieldInfo field = @enum.GetType().GetField(value);
        if (field == null) return string.Empty;

        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        if (attr == null) return value;
        return attr.Description;
    }

    #endregion 枚举
}