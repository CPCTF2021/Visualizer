﻿using System.Collections;
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
            StartCoroutine("Animation");
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
            for (int i = 0; i < 10; i++)
            {
                userManager.AddUser("name", i.ToString(), texture);
                yield return new WaitForSeconds(0.01f);
            }
        }

        IEnumerator AddPoint()
        {
            for (int i = 0; i < 100; i++)
            {
                userManager.AddScore(Random.Range(0, 10).ToString(), (VisualizerSystem.ProblemSolvedEvent.Genre)Random.Range(0, 10), 1000);
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            }
        }
    }
}