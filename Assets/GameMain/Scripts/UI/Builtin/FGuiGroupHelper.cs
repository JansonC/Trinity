using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity
{
    /// <summary>
    /// FGui界面组辅助器
    /// </summary>
    public class FGuiGroupHelper : UIGroupHelperBase
    {
        public const int DepthFactor = 10000;
        private int m_Depth = 0;

        public override void SetDepth(int depth)
        {
            m_Depth = depth;
        }
    }


}
