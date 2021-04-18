using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserScripts;

namespace VisualizerSystem
{
    public class DummyDataInsertion : MonoBehaviour
    {
        [SerializeField]
        Texture texture;
        UserManager userManager;
        void Start()
        {
            userManager = GetComponent<UserManager>();
            StartCoroutine("MakeTrees");
            StartCoroutine("AddPoint");
        }

        IEnumerator Animation()
        {
            yield return MakeTrees();
            yield return new WaitForSeconds(2f);
            yield return AddPoint();
        }

        IEnumerator MakeTrees()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 150; i++)
            {
                userManager.AddUser($"name{Mathf.Pow(1.5f, i)}", i.ToString(), texture);
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator AddPoint()
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < 15000; i++)
            {
                string v = Random.Range(0, userManager.usersDictionary.Count - 1).ToString();
                userManager.AddScore(v, (VisualizerSystem.ProblemSolvedEvent.Genre)Random.Range(0, 10), Random.Range(100, 1000));
                yield return new WaitForSeconds(Random.Range(1f, 4f));
            }
        }
    }
}
