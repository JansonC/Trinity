using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using FairyGUI;
namespace Trinity
{
    public abstract class FGuiForm : UIFormLogic
    {
        protected GComponent UI
        {
            get;
            private set;
        }

        private static UIContentScaler s_Scaler;

        private void Awake()
        {
            if (s_Scaler == null)
            {
                s_Scaler = GameEntry.UI.GetComponent<UIContentScaler>();
            }

            FGuiInfoAttribute fguiInfo;

            object[] attrs = GetType().GetCustomAttributes(false);
            if (attrs.Length == 0)
            {
                Log.Error("没有为{0}类型的FGuiForm添加FGuiInfo特性", GetType());
                return;
            }
            else
            {
                fguiInfo = attrs[0] as FGuiInfoAttribute;
            }

            UIPackage.AddPackage(fguiInfo.PackageName, (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
            {
                destroyMethod = DestroyMethod.Unload;
                return FGuiUtility.GetFGuiResObject(name);
            });

            UIPackage.AddPackage(fguiInfo.PackageName, (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
            {
                destroyMethod = DestroyMethod.Unload;
                return FGuiUtility.GetFGuiResObject(name);
            });

            UI = GetComponent<UIPanel>().ui;
            GRoot.inst.SetContentScaleFactor(1080, 1920);
        }

    }
}

