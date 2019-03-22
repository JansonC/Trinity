using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using UnityGameFramework.Runtime;

namespace Trinity
{
    [FGuiInfo("Test")]
    public class TestUIForm : FGuiForm
    {

        protected override void OnInit(object userData)
        {
            GButton btnTest = UI.GetChild("TestBtn").asButton;
            btnTest.onClick.Set(() => {
                Log.Debug("点击了测试按钮");
            });
        }
    }
}

