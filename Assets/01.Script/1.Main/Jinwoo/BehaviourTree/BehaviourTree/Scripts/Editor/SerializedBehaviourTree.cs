using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTreeEditorDev {

    // 동작에 대한 속성을 찾기 위해 직렬화된 개체를 래핑하는 도우미 클래스입니다.
    // SerializedObjects 및 SerializedProperty 인터페이스를 통해 동작 트리를 수정하는 것이 가장 좋습니다.
    // UI를 동기화 상태로 유지하고 실행 취소/다시 실행
    // 시간이 지남에 따라 발전할 다양한 기능의 뒤죽박죽 믹스입니다. 어떤 식으로든 완전하지는 않습니다.
    public class SerializedBehaviourTree {

        // 비헤이비어 트리에 변경 사항을 쓰기 위한 래퍼 직렬화 객체
        readonly public SerializedObject serializedObject;
        readonly public BehaviourTree tree;

        // 속성 이름. 이들은 행동 트리의 변수 이름에 해당합니다.
        const string sPropRootNode = "rootNode";
        const string sPropNodes = "nodes";
        const string sPropBlackboard = "blackboard";
        const string sPropGuid = "guid";
        const string sPropChild = "child";
        const string sPropChildren = "children";
        const string sPropPosition = "position";
        const string sViewTransformPosition = "viewPosition";
        const string sViewTransformScale = "viewScale";

        public SerializedProperty RootNode {
            get {
                return serializedObject.FindProperty(sPropRootNode);
            }
        }

        public SerializedProperty Nodes {
            get {
                return serializedObject.FindProperty(sPropNodes);
            }
        }

        public SerializedProperty Blackboard {
            get {
                return serializedObject.FindProperty(sPropBlackboard);
            }
        }

        // Start is called before the first frame update
        public SerializedBehaviourTree(BehaviourTree tree)
        {
            serializedObject = new SerializedObject(tree);
            this.tree = tree;
        }

        public void Save() {
            serializedObject.ApplyModifiedProperties();
        }

        public SerializedProperty FindNode(SerializedProperty array, Node node) {
            for(int i = 0; i < array.arraySize; ++i) {
                var current = array.GetArrayElementAtIndex(i);
                if (current.FindPropertyRelative(sPropGuid).stringValue == node.guid) {
                    return current;
                }
            }
            return null;
        }

        public void SetViewTransform(Vector3 position, Vector3 scale) {
            serializedObject.FindProperty(sViewTransformPosition).vector3Value = position;
            serializedObject.FindProperty(sViewTransformScale).vector3Value = scale;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public void SetNodePosition(Node node, Vector2 position) {
            var nodeProp = FindNode(Nodes, node);
            nodeProp.FindPropertyRelative(sPropPosition).vector2Value = position;
            serializedObject.ApplyModifiedProperties();
        }

        public void DeleteNode(SerializedProperty array, Node node) {
            for (int i = 0; i < array.arraySize; ++i) {
                var current = array.GetArrayElementAtIndex(i);
                if (current.FindPropertyRelative(sPropGuid).stringValue == node.guid) {
                    array.DeleteArrayElementAtIndex(i);
                    return;
                }
            }
        }

        public Node CreateNodeInstance(System.Type type) {
            Node node = System.Activator.CreateInstance(type) as Node;
            node.guid = GUID.Generate().ToString();
            return node;
        }

        SerializedProperty AppendArrayElement(SerializedProperty arrayProperty) {
            arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
            return arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
        }

        public Node CreateNode(System.Type type, Vector2 position) {

            Node node = CreateNodeInstance(type);
            node.position = position;

            SerializedProperty newNode = AppendArrayElement(Nodes);
            newNode.managedReferenceValue = node;

            serializedObject.ApplyModifiedProperties();

            return node;
        }

        public void SetRootNode(RootNode node) {
            RootNode.managedReferenceValue = node;
            serializedObject.ApplyModifiedProperties();
        }

        public void DeleteNode(Node node) {

            SerializedProperty nodesProperty = Nodes;

            for(int i = 0; i < nodesProperty.arraySize; ++i) {
                var prop = nodesProperty.GetArrayElementAtIndex(i);
                var guid = prop.FindPropertyRelative(sPropGuid).stringValue;
                DeleteNode(Nodes, node);
                serializedObject.ApplyModifiedProperties();
            }
        }

        public void AddChild(Node parent, Node child) {
            
            var parentProperty = FindNode(Nodes, parent);

            // RootNode, Decorator node
            var childProperty = parentProperty.FindPropertyRelative(sPropChild);
            if (childProperty != null) {
                childProperty.managedReferenceValue = child;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            // Composite nodes
            var childrenProperty = parentProperty.FindPropertyRelative(sPropChildren);
            if (childrenProperty != null) {
                SerializedProperty newChild = AppendArrayElement(childrenProperty);
                newChild.managedReferenceValue = child;
                serializedObject.ApplyModifiedProperties();
                return;
            }
        }

        public void RemoveChild(Node parent, Node child) {
            var parentProperty = FindNode(Nodes, parent);

            // RootNode, Decorator node
            var childProperty = parentProperty.FindPropertyRelative(sPropChild);
            if (childProperty != null) {
                childProperty.managedReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            // Composite nodes
            var childrenProperty = parentProperty.FindPropertyRelative(sPropChildren);
            if (childrenProperty != null) {
                DeleteNode(childrenProperty, child);
                serializedObject.ApplyModifiedProperties();
                return;
            }
        }
    }
}
