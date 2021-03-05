using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public List<User> trees;
    public bool[] usedTree;
    public Dictionary<string, User> users;

    public void SetTrees(List<User> trees) {
        this.trees = trees;
        this.usedTree = new bool[trees.Count];
        for(int i=0;i<trees.Count;i++) {
            this.usedTree[i] = false;
        }
        this.users = new Dictionary<string, User>();
    }

    public void AddUser(string name, string id, Sprite icon, int[] pointsNum) {
        int count = trees.Count;
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
        
        trees[index].SetUser(name, id, icon, points);
        users.Add(id, trees[index]);
    }

    public void AddUser(string name, string id, Sprite icon) {
        this.AddUser(name, id, icon, new int[10]);
    }

    public void AddPoint(string id, int genre, int point) {
        User user;
        if(!users.TryGetValue(id, out user)) throw new MissingFieldException();
        user.AddPoint(genre, point);
    }
}
