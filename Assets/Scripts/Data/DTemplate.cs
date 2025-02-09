using BroccoliBunnyStudios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

/* 
 * Data for import is at:
 * https://docs.google.com/spreadsheets/d/17e5d2aFBtPLhe7hjjMhVtuT4IQHWFQrPe12BFpmt2FQ/edit?gid=834530743#gid=834530743
 */

[CreateAssetMenu(fileName = "DTemplate", menuName = "Data/DTemplate", order = 3)]
public class DTemplate : ScriptableObject, IDataImport
{
    private static DTemplate s_loadedData;
    private static Dictionary<int, TemplateData> s_cachedDataDict;

    [field: SerializeField]
    public List<TemplateData> Data { get; private set; }

    public static DTemplate GetAllData()
    {
        if (s_loadedData == null)
        {
            // Load and cache results
            s_loadedData = Resources.Load<DTemplate>("Data/DTemplate");

            // Calculate and cache some results
            s_cachedDataDict = new();
            foreach (var data in s_loadedData.Data)
            {
#if UNITY_EDITOR
                if (s_cachedDataDict.ContainsKey(data.Id))
                {
                    Debug.LogError($"Duplicate Id {data.Id}");
                }
#endif
                s_cachedDataDict[data.Id] = data;
            }
        }

        return s_loadedData;
    }

    public static TemplateData? GetDataById(int id)
    {
        if (s_cachedDataDict == null)
        {
            GetAllData();
        }
        return s_cachedDataDict.TryGetValue(id, out var result) ? result : null;
    }

#if UNITY_EDITOR
    public static void ImportData(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        s_loadedData = GetAllData();
        if (s_loadedData == null)
        {
            return;
        }

        if (s_loadedData.Data == null)
        {
            s_loadedData.Data = new();
        }
        else
        {
            s_loadedData.Data.Clear();
        }

        // special handling for shape parameter and percentage
        var pattern = @"[{}""]";
        text = text.Replace("\r\n", "\n");      // handle window line break
        text = text.Replace(",\n", ",");
        text = Regex.Replace(text, pattern, "");

        // Split data into lines
        var lines = text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.None);
        for (var i = 0; i < lines.Length; i++)
        {
            // Comment and Header
            if (lines[i][0].Equals('#') || lines[i][0].Equals('$'))
            {
                continue;
            }

            // Empty line
            var trimLine = lines[i].Trim();
            var testList = trimLine.Split('\t');
            if (testList.Length == 1 && string.IsNullOrEmpty(testList[0]))
            {
                continue;
            }

            // Split
            var paramList = lines[i].Split('\t');
            for (var j = 0; j < paramList.Length; j++)
            {
                paramList[j] = paramList[j].Trim();
            }

            // New item
            var templateData = new TemplateData
            {
                Id = CommonUtil.ConvertToInt32(paramList[1]),
            };
            s_loadedData.Data.Add(templateData);
        }

        CommonUtil.SaveScriptableObject(s_loadedData);
    }
#endif
}

[Serializable]
public struct TemplateData
{
    [field: SerializeField]
    public int Id { get; set; }
}
