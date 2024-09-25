using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

/// <summary>
/// Utility class for creating new scripts.
/// </summary>
public static class ScriptTemplateUtils {
    private const string TEMPLATE_PATH = "Packages/com.stimpae.utilities/Editor/Templates/";

    private static void CreateScript(string templatePath, string fileName) {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, fileName);
    }

    [MenuItem("Assets/Create/Scripts/Monobehaviour", false, -999)]
    private static void CreateMonoBehaviour() => CreateScript($"{TEMPLATE_PATH}MonoBehaviourTemplate.cs.txt", "NewMonoBehaviour.cs");

    [MenuItem("Assets/Create/Scripts/Scriptable Object", false, -999)]
    private static void CreateScriptableObject() => CreateScript($"{TEMPLATE_PATH}ScriptableObjectTemplate.cs.txt", "NewScriptableObject.cs");

    [MenuItem("Assets/Create/Scripts/Unity/Editor/Editor Window", false, 9)]
    private static void CreateEditorWindow() => CreateScript($"{TEMPLATE_PATH}EditorWindowTemplate.cs.txt", "NewEditorWindow.cs");
    
    [MenuItem("Assets/Create/Scripts/Unity/Editor/Editor Window (UITK)", false, 10)]
    private static void CreateEditorToolkitWindow() => CreateScript($"{TEMPLATE_PATH}EditorWindowTemplate(UITK).cs.txt", "NewEditorWindow.cs");

    [MenuItem("Assets/Create/Scripts/Unity/Editor/Editor", false, 10)]
    private static void CreateEditor() => CreateScript($"{TEMPLATE_PATH}EditorTemplate.cs.txt", "NewEditor.cs");
    
    [MenuItem("Assets/Create/Scripts/Unity/Editor/Editor (UITK)", false, 10)]
    private static void CreateEditorToolkit() => CreateScript($"{TEMPLATE_PATH}EditorTemplate(UITK).cs.txt", "NewEditor.cs");

    [MenuItem("Assets/Create/Scripts/Text/Json File", false, 10)]
    private static void CreateJson() => CreateScript($"{TEMPLATE_PATH}JsonTemplate.json.txt", "NewJson.json");
    
    [MenuItem("Assets/Create/Scripts/Text/Xml File", false, 10)]
    private static void CreateXml() => CreateScript($"{TEMPLATE_PATH}XmlTemplate.xml.txt", "NewXml.xml");
    
    [MenuItem("Assets/Create/Scripts/Text/Markdown File", false, 10)]
    private static void CreateMarkdown() => CreateScript($"{TEMPLATE_PATH}MarkdownTemplate.md.txt", "NewMarkdown.md");
    
    [MenuItem("Assets/Create/Scripts/Text/Text File", false, 10)]
    private static void CreateText() => CreateScript($"{TEMPLATE_PATH}TextTemplate.txt.txt", "NewText.txt");
    
    [MenuItem("Assets/Create/Scripts/C#/C# Class", false, 10)]
    private static void CreateClass() => CreateScript($"{TEMPLATE_PATH}NativeClassTemplate.cs.txt", "NewClass.cs");
    
    [MenuItem("Assets/Create/Scripts/C#/C# Struct", false, 10)]
    private static void CreateStruct() => CreateScript($"{TEMPLATE_PATH}NativeStructTemplate.cs.txt", "NewStruct.cs");
    
    [MenuItem("Assets/Create/Scripts/C#/C# Interface", false, 10)]
    private static void CreateInterface() => CreateScript($"{TEMPLATE_PATH}NativeInterfaceTemplate.cs.txt", "NewInterface.cs");
    
    [MenuItem("Assets/Create/Scripts/C#/C# Enum", false, 10)]
    private static void CreateEnum() => CreateScript($"{TEMPLATE_PATH}NativeEnumTemplate.cs.txt", "NewEnum.cs");
    
    [MenuItem("Assets/Create/Scripts/Unity/Editor/Property Attribute", false, 10)]
    private static void CreatePropertyAttribute() => CreateScript($"{TEMPLATE_PATH}PropertyAttributeTemplate.cs.txt", "NewPropertyAttribute.cs");
    
    [MenuItem("Assets/Create/Scripts/Unity/Editor/Property Drawer", false, 10)]
    private static void CreatePropertyDrawer() => CreateScript($"{TEMPLATE_PATH}PropertyDrawerTemplate.cs.txt", "NewPropertyDrawer.cs");
    
    [MenuItem("Assets/Create/Scripts/Unity/Editor/Property Drawer (UITK)", false, 10)]
    private static void CreatePropertyDrawerToolkit() => CreateScript($"{TEMPLATE_PATH}PropertyDrawerTemplate(UITK).cs.txt", "NewPropertyDrawer.cs");
    
    
    
    
    
    
}