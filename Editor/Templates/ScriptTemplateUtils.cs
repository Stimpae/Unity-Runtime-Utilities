using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

/// <summary>
/// Utility class for creating new scripts.
/// </summary>
public static class ScriptTemplateUtils {
    private static void CreateScript(string templatePath, string fileName) {
         ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, fileName);
     }
     
     [MenuItem("Assets/Create/Script/Monobehaviour", false, 999)]
     private static void CreateMonoBehaviour() {
            CreateScript("Packages/com.stimpae.tgutilities/Editor/Templates/MonoBehaviourTemplate.cs.txt", "NewMonoBehaviour.cs");
     }
     
     
}
