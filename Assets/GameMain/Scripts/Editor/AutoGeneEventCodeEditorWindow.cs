﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
namespace Trinity.Editor
{
    /// <summary>
    /// 事件参数数据
    /// </summary>
    [Serializable]
    public class EventArgsData
    {
        public string Type;
        public string Name;
        public EventArgType TypeEnum;
        public EventArgsData()
        {

        }

        public EventArgsData(string type,string name)
        {
            Type = type;
            Name = name;
        }

       
    }

    public enum EventArgType
    {
        Object,
        Int,
        Float,
        Bool,
        Char,
        String,

        UnityObject,
        GameObject,
        Transfrom,
        Vector2,
        Vector3,
        Quaternion,

        Other,
    }

    public class AutoGeneEventCodeEditorWindow : EditorWindow
    {
        [MenuItem("Trinity/自动生成事件参数类的代码")]
        public static void OpenAutoGeneWindow()
        {
            
            AutoGeneEventCodeEditorWindow window = GetWindow<AutoGeneEventCodeEditorWindow>(true, "自动生成事件参数类代码");
            window.minSize = new Vector2(460f, 400f);
        }

        [SerializeField]
        private List<EventArgsData> m_EventArgsDatas = new List<EventArgsData>();
    
        
        /// <summary>
        /// 是否是热更新层事件
        /// </summary>
        private bool isHotfixEvent = false;
        /// <summary>
        /// 事件参数类名
        /// </summary>
        private string className;
        //事件代码生成后的路径
        private const string m_EventCodePath = "Assets/GameMain/Scripts/Event";
        private const string m_HotfixEventCodePath = "Assets/GameMain/Scripts/Hotfix/Event";
        private void OnEnable()
        {
            m_EventArgsDatas.Clear();
            className = "EventArgs";
        }

        private void OnGUI()
        {
            //EditorGUILayout.BeginScrollView(Vector2.zero);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("事件参数类名：", GUILayout.Width(140f));
            className = EditorGUILayout.TextField(className);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("是否为热更新事件：", GUILayout.Width(140f));
            isHotfixEvent = EditorGUILayout.Toggle(isHotfixEvent);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("自动生成的代码路径：", GUILayout.Width(140f));
            EditorGUILayout.LabelField(isHotfixEvent ? m_HotfixEventCodePath : m_EventCodePath);
            EditorGUILayout.EndHorizontal();

            //绘制事件参数相关按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加事件参数", GUILayout.Width(140f)))
            {
                m_EventArgsDatas.Add(new EventArgsData(null,null));
            }
            if (GUILayout.Button("删除所有事件参数", GUILayout.Width(140f)))
            {
                m_EventArgsDatas.Clear();
            }
            if (GUILayout.Button("删除空事件参数", GUILayout.Width(140f)))
            {
                for (int i = m_EventArgsDatas.Count -1; i >= 0; i--)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    if (string.IsNullOrWhiteSpace(data.Name))
                    {
                        m_EventArgsDatas.RemoveAt(i);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            //绘制事件参数数据
            for (int i = 0; i < m_EventArgsDatas.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EventArgsData data = m_EventArgsDatas[i];
                EditorGUILayout.LabelField("参数类型：",GUILayout.Width(70f));
                data.TypeEnum = (EventArgType)EditorGUILayout.EnumPopup(data.TypeEnum, GUILayout.Width(100f));
                switch (data.TypeEnum)
                {
                    case EventArgType.Object:
                    case EventArgType.Int:
                    case EventArgType.Float:
                    case EventArgType.Bool:
                    case EventArgType.Char:
                    case EventArgType.String:
                        data.Type = data.TypeEnum.ToString().ToLower();
                        break;

                    case EventArgType.UnityObject:
                        data.Type = "UnityEngine.Object";
                        break;

                    case EventArgType.Other:
                        data.Type = EditorGUILayout.TextField(data.Type, GUILayout.Width(140f));
                        break;

                    default:
                        data.Type = data.TypeEnum.ToString();
                        break;
                }
                EditorGUILayout.LabelField("参数字段名：", GUILayout.Width(70f));
                data.Name = EditorGUILayout.TextField(data.Name, GUILayout.Width(140f));
                EditorGUILayout.EndHorizontal();
            }

            //生成事件参数类代码
            if (GUILayout.Button("生成事件参数类代码", GUILayout.Width(210f)))
            {
                AutoGeneEventCode();
                AssetDatabase.Refresh();
            }

            //EditorGUILayout.EndScrollView();
        }

        private void AutoGeneEventCode()
        {
            //根据是否为热更新层事件来决定一些参数
            string codepath = isHotfixEvent ? m_HotfixEventCodePath : m_EventCodePath;
            string nameSpace = isHotfixEvent ? "Trinity.Hotfix" : "Trinity";

            using (StreamWriter sw = new StreamWriter($"{codepath}/{className}.cs"))
            {
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("using GameFramework.Event;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic class {className} : GameEventArgs");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //事件编号
                sw.WriteLine($"\t\tpublic static readonly int EventId = typeof({className}).GetHashCode();");
                sw.WriteLine("");
                sw.WriteLine("\t\tpublic override int Id");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tget");
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\treturn EventId;");
                sw.WriteLine("\t\t\t}");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //事件参数
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    data.Name = data.Name[0].ToString().ToUpper() + data.Name.Substring(1);
                    sw.WriteLine($"\t\tpublic {data.Type} {data.Name}");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tget;");
                    sw.WriteLine("\t\t\tprivate set;");
                    sw.WriteLine("\t\t}");
                    sw.WriteLine("");
                }

                //清空参数数据方法
                sw.WriteLine($"\t\tpublic override void Clear()");
                sw.WriteLine("\t\t{");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.WriteLine($"\t\t\t{data.Name} = default({data.Type});");
                }
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //填充参数数据方法
                sw.Write($"\t\tpublic {className} Fill(");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.Write($"{data.Type} {data.Name.ToLower()}");
                    if (i!=m_EventArgsDatas.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.WriteLine(")");
                sw.WriteLine("\t\t{");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.WriteLine($"\t\t\t{data.Name} = {data.Name.ToLower()};");
                }
                sw.WriteLine("\t\t\treturn this;");
                sw.WriteLine("\t\t}");



                sw.WriteLine("\t}");
                sw.WriteLine("}");

            }
        }
    }

   

}
