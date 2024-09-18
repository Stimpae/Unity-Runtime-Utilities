using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

/// <summary>
/// Utility class for creating new scripts.
/// </summary>
public static class ScriptTemplateUtils {
     
     /// <summary>
     /// 
     /// </summary>
     /// <param name="templatePath"></param>
     /// <param name="fileName"></param>
     private static void CreateScript(string templatePath, string fileName) {
         ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, fileName);
     }
     
     [MenuItem("Assets/Create/Script/Monobehaviour", false, 10)]
        private static void CreateMonoBehaviour() {
            CreateScript("Assets/TG Utilities/Editor/Templates/MonoBehaviourTemplate.cs.txt", "NewMonoBehaviour.cs");
        }
     
     
}
