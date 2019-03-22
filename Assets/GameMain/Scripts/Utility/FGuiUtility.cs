using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity
{

    public static class FGuiUtility
    {
        private static ReferenceCollector m_RC;

        public static void InitFGuiRes()
        {
            GameEntry.Resource.LoadAsset("Assets/GameMain/UI/FGuiRes/FGuiRes.prefab", Constant.AssetPriority.UIFormAsset, new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
             {
                 GameObject fguiRes = (GameObject)asset;
                 m_RC = fguiRes.GetComponent<ReferenceCollector>();
                 Log.Debug("加载FGuiRes成功");
             },

            (assetName, status, errorMessage, userData) =>
            {
                Log.Error("加载FGuiRes失败：{0}", errorMessage);
            }));

        }

        public static Object GetFGuiResObject(string objectName)
        {
            return m_RC.Get<Object>(objectName);
        }


    }
}


