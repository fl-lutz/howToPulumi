using System.Collections.Generic;
using Pulumi;

namespace chap4.helpers;

public static class Tags
{
    /// <summary>
    ///     This Method creates a InputMap from a List of KeyValue Pairs
    /// </summary>
    /// <param name="tagList">A List of KeyValue Pairs</param>
    /// <returns>A InputMap of strings</returns>
    public static InputMap<string> CreateTags(List<KeyValuePair<string, string>> tagList)
    {
        var tagInputMap = new InputMap<string>();
        foreach (var tag in tagList)
        {
            tagInputMap.Add(tag.Key, tag.Value);
        }

        return tagInputMap;
    }
}
