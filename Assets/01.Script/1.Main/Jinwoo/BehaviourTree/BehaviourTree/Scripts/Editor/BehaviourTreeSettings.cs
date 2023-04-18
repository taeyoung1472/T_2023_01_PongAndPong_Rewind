using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

class BehaviourTreeSettings : ScriptableObject {
    public VisualTreeAsset behaviourTreeXml;
    public StyleSheet behaviourTreeStyle;
    public VisualTreeAsset nodeXml;
    public TextAsset scriptTemplateActionNode;
    public TextAsset scriptTemplateCompositeNode;
    public TextAsset scriptTemplateDecoratorNode;
    public string newNodeBasePath = "Assets/";

    static BehaviourTreeSettings FindSettings(){
        var guids = AssetDatabase.FindAssets("t:BehaviourTreeSettings");
        if (guids.Length > 1) {
            Debug.LogWarning($"Found multiple settings files, using the first.");
        }

        switch (guids.Length) {
            case 0:
                return null;
            default:
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<BehaviourTreeSettings>(path);
        }
    }

    internal static BehaviourTreeSettings GetOrCreateSettings() {
        var settings = FindSettings();
        if (settings == null) {
            settings = ScriptableObject.CreateInstance<BehaviourTreeSettings>();
            AssetDatabase.CreateAsset(settings, "Assets");
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    internal static SerializedObject GetSerializedSettings() {
        return new SerializedObject(GetOrCreateSettings());
    }
}

// ����� �����ӿ�ũ�� UIElements�� ����Ͽ� SettingsProvider�� ���
static class MyCustomSettingsUIElementsRegister {
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider() {
        // ù ��° �Ű������� ���� â�� ���
        // �� ��° �Ű������� �� ������ ����. ������Ʈ ������ ���� â���� ��Ÿ��
        var provider = new SettingsProvider("Project/MyCustomUIElementsSettings", SettingsScope.Project) {
            label = "BehaviourTree",
            // activateHandler�� ����ڰ� ���� â���� ���� �׸��� Ŭ���� �� ȣ��
            activateHandler = (searchContext, rootElement) => {
                var settings = BehaviourTreeSettings.GetSerializedSettings();

                // rootElement�� VisualElement. �ڽ��� �߰��ϸ� OnGUI �����
                // SettingsProvider�� UIElements ����� �����ӿ�ũ�� ����ϱ� ������ ȣ����� ����(������ �ռҸ��� �� �𸣰���)
                var title = new Label() {
                    text = "Behaviour Tree Settings"
                };
                title.AddToClassList("title");
                rootElement.Add(title);

                var properties = new VisualElement() {
                    style =
                    {
                        flexDirection = FlexDirection.Column
                    }
                };
                properties.AddToClassList("property-list");
                rootElement.Add(properties);

                properties.Add(new InspectorElement(settings));

                rootElement.Bind(settings);
            },
        };

        return provider;
    }
}
