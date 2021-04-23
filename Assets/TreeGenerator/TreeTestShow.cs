using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeScripts {
    public class TreeTestShow : MonoBehaviour
    {
        public GameObject tree;

        void Start()
        {
            StartCoroutine("Loop");
        }
        IEnumerator Loop()
        {
            float pos = 0f;
            while(true)
            {
                ControlTree controlTree = Instantiate(tree, new Vector3(pos, 0f, 0f), Quaternion.identity).GetComponent<ControlTree>();
                TreeGenerator treeGenerator = GetComponent<TreeGenerator>();
                treeGenerator.MakeTree();
                treeGenerator.SetMesh(controlTree);
                //controlTree.SetProgress(1f);
                yield return new WaitForSeconds(1f);
                pos += 2f;
            }
        }
    }
}
