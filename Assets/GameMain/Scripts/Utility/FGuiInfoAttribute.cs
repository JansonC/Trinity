using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trinity
{
    public class FGuiInfoAttribute : Attribute
    {
        public string PackageName;

        public FGuiInfoAttribute(string packageName)
        {
            PackageName = packageName;
        }


    }

}
