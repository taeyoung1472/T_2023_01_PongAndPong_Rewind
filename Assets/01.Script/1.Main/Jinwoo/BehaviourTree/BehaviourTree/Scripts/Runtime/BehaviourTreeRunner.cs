using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeEditorDev {
    public class BehaviourTreeRunner : MonoBehaviour {

        public BehaviourTree tree;

        // ���� ��ü ���� �ý����� �����ϴ� ���丮�� �����̳� ��ü
        Context context;

        void Start() {
            context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(context);
        }

        void Update() {
            if (tree) {
                tree.Update();
            }
        }

        Context CreateBehaviourTreeContext() {
            return Context.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}