using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using Data;
using System.ComponentModel;
using System.Reflection;


public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    #region Functions
    [MenuItem("Clear/RemoveSaveData _F3")]
    public static void RemoveSaveData()
    {
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("SaveFile Deleted");
        }
        else
        {
            Debug.Log("No SaveFile Detected");
        }
    }

    #endregion

    [MenuItem("Tools/ParseExcel _F4")]  // 추가 단축키: Control + K
    public static void ParseExcelDataToJson()
    {
        ParseExcelDataToJson<SkillDataLoader, SkillData>("Skill");
        ParseExcelDataToJson<HeroDataLoader, HeroData>("Hero");
        ParseExcelDataToJson<MonsterDataLoader, MonsterData>("Monster");
        //ParseExcelDataToJson<ProjectileDataLoader, ProjectileData>("Projectile");
        ParseExcelDataToJson<StageDataLoader, StageData>("Stage");
        

        Debug.Log("Complete DataTransformer");
    }

    #region Helpers
    private static void ParseExcelDataToJson<Loader, LoaderData>(string filename) where Loader : new() where LoaderData : new()
    {
        Loader loader = new Loader();
        FieldInfo field = loader.GetType().GetFields()[0];
        field.SetValue(loader, ParseExcelDataToList<LoaderData>(filename));

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/Resources/Data/JsonData/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static List<LoaderData> ParseExcelDataToList<LoaderData>(string filename) where LoaderData : new()
    {
        List<LoaderData> loaderDatas = new List<LoaderData>();

        string[] lines = File.ReadAllText($"{Application.dataPath}/Resources/Data/ExcelData/{filename}Data.csv").Split("\n");

        for (int l = 1; l < lines.Length; l++)
        {
            string[] row = lines[l].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            LoaderData loaderData = new LoaderData();
            var fields = GetFieldsInBase(typeof(LoaderData));

            for (int f = 0; f < fields.Count; f++)
            {
                FieldInfo field = loaderData.GetType().GetField(fields[f].Name);
                Type type = field.FieldType;

                if (type.IsGenericType)
                {
                    object value = ConvertList(row[f], type);
                    field.SetValue(loaderData, value);
                }
                else
                {
                    object value = ConvertValue(row[f], field.FieldType);
                    field.SetValue(loaderData, value);
                }
            }

            loaderDatas.Add(loaderData);
        }

        return loaderDatas;
    }

    private static object ConvertValue(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);
    }

    private static object ConvertList(string value, Type type)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        // Reflection
        Type valueType = type.GetGenericArguments()[0];
        Type genericListType = typeof(List<>).MakeGenericType(valueType);
        var genericList = Activator.CreateInstance(genericListType) as IList;

        // Parse Excel
        var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();

        foreach (var item in list)
            genericList.Add(item);

        return genericList;
    }

    private static IList ParseCsvDataToList(string csvData, Type itemType)
    {
        var listType = typeof(List<>).MakeGenericType(new Type[] { itemType });
        var list = Activator.CreateInstance(listType) as IList;

        if (string.IsNullOrEmpty(csvData)) return list;

        var items = csvData.Split('\n');
        foreach (var item in items)
        {
            var obj = Activator.CreateInstance(itemType);
            var props = item.Split(',');

            var fields = itemType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fields.Length && i < props.Length; i++)
            {
                var field = fields[i];
                object value = Convert.ChangeType(props[i], field.FieldType);
                field.SetValue(obj, value);
            }

            list.Add(obj);
        }

        return list;
    }

    public static List<FieldInfo> GetFieldsInBase(Type type, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    {
        List<FieldInfo> fields = new List<FieldInfo>();
        HashSet<string> fieldNames = new HashSet<string>(); // 중복방지
        Stack<Type> stack = new Stack<Type>();

        while (type != typeof(object))
        {
            stack.Push(type);
            type = type.BaseType;
        }

        while (stack.Count > 0)
        {
            Type currentType = stack.Pop();

            foreach (var field in currentType.GetFields(bindingFlags))
            {
                if (fieldNames.Add(field.Name))
                {
                    fields.Add(field);
                }
            }
        }

        return fields;
    }

    #endregion

#endif

}