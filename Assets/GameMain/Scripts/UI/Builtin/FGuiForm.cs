using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using FairyGUI;
namespace Trinity
{
    public abstract class FGuiForm : UIFormLogic
    {
       

        protected UIPanel UIPanel
        {
            get;
            private set;
        }

        protected GComponent UI
        {
            get;
            private set;
        }

        /// <summary>
        /// 原始深度
        /// </summary>
        public int OriginalDepth
        {
            get;
            private set;
        }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get
            {
                return UIPanel.sortingOrder;
            }
        }

        public const int DepthFactor = 100;

        private static UIContentScaler s_Scaler;

        private void Awake()
        {
            

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

            UIPanel = GetComponent<UIPanel>();
            UI = UIPanel.ui;

            if (s_Scaler == null)
            {
                s_Scaler = GameEntry.UI.GetComponent<UIContentScaler>();
                GRoot.inst.SetContentScaleFactor(s_Scaler.designResolutionX, s_Scaler.designResolutionY);
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            OriginalDepth = UIPanel.sortingOrder;
        }



        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            UIPanel.sortingOrder += deltaDepth;

        }

     
    }
}

