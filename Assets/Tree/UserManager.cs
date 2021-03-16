using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public List<User> users;
    public bool[] usedTree;
    public Dictionary<string, User> usersDictionary;

    IEnumerator LoadUsers() {
        Texture tex = Resources.Load("icon") as Texture;
        for(int i=0;i<this.users.Count;i++) {
            AddUser("UserName", i.ToString(), tex);
        }

        for(int i=0;i<1000;i++) {
            yield return new WaitForSeconds(0.3f);
            AddPoint((i % this.users.Count).ToString(), UnityEngine.Random.Range(0, 10), 1000);
        }
    }

    public void SetTrees(List<User> users) {
        this.users = users;
        this.usedTree = new bool[users.Count];
        for(int i=0;i<users.Count;i++) {
            this.usedTree[i] = false;
        }
        this.usersDictionary = new Dictionary<string, User>();

        // テストコード
        StartCoroutine(LoadUsers());
    }

    public void AddUser(string name, string id, Texture icon, int[] pointsNum) {
        int count = users.Count;
        int index = UnityEngine.Random.Range(0, count);
        
        bool fullyUsed = true;

        for(int i=0;i<count - 1;i++) {
            if(!usedTree[(index + i) % count]) {
                usedTree[(index + i) % count] = true;
                fullyUsed = false;
                break;
            }
        }

        if(fullyUsed) throw new IndexOutOfRangeException();


        Points points = new Points();
        for(int i=0;i<10;i++) {
            points.Add(i, pointsNum[i]);
        }
        
        users[index].SetUser(name, id, icon, points);
        usersDictionary.Add(id, users[index]);
    }

    public void AddUser(string name, string id, Texture icon) {
        this.AddUser(name, id, icon, new int[10]);
    }

    public void AddPoint(string id, int genre, int point) {
        User user;
        if(!usersDictionary.TryGetValue(id, out user)) throw new MissingFieldException();
        user.AddPoint(genre, point);
    }
}
