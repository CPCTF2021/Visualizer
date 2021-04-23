using System.Collections;
using RankingScript;
using UnityEngine;
using UserScripts;

namespace VisualizerSystem
{
    public class DummyDataInsertion : MonoBehaviour
    {
        [SerializeField]
        Texture texture;
        UserManager userManager;
        RankingManager rankingManager;
        void Start()
        {
            userManager = GetComponent<UserManager>();
            rankingManager = GameObject.Find("RankingPanel").GetComponent<RankingManager>();
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
            for (int i = 0; i < 30; i++)
            {
                userManager.AddUser($"name{Mathf.Pow(1.5f, i)}", i.ToString(), texture);
                rankingManager.AddUser(userManager.usersDictionary[i.ToString()]);
                yield return new WaitForSeconds(2f);
            }
        }

        IEnumerator AddPoint()
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < 100; i++)
            {
                string v = Random.Range(0, userManager.usersDictionary.Count - 1).ToString();
                var genre = (VisualizerSystem.ProblemSolvedEvent.Genre)Random.Range(0, 10);
                var score = Random.Range(100, 1000);
                userManager.AddScore(v, genre, score);
                rankingManager.Update(userManager.usersDictionary[v]);
                yield return new WaitForSeconds(Random.Range(2f, 3f));
            }
        }
    }
}
