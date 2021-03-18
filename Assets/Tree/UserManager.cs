using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
  public List<User> users;
  public bool[] usedTree;
  public Dictionary<string, User> usersDictionary;
  [SerializeField]
  CameraAnimator cameraAnimator;

  IEnumerator LoadUsers()
  {
    Texture tex = Resources.Load("icon") as Texture;
    for (int i = 0; i < this.users.Count; i++)
    {
      AddUser("UserName", i.ToString(), tex);
      AddPoint(i.ToString(), UnityEngine.Random.Range(0, 10), 5000);
    }

    for (int i = 0; i < 1000; i++)
    {
      AddPoint((i % this.users.Count).ToString(), UnityEngine.Random.Range(0, 10), 1000);
      yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
    }
  }

  public void SetTrees(List<User> users)
  {
    this.users = users;
    this.usedTree = new bool[users.Count];
    for (int i = 0; i < users.Count; i++)
    {
      this.usedTree[i] = false;
    }
    this.usersDictionary = new Dictionary<string, User>();

    // テストコード
    StartCoroutine(LoadUsers());
  }

  public void AddUser(string name, string id, Texture icon, int[] pointsNum)
  {
    int count = users.Count;
    int index = UnityEngine.Random.Range(0, count);

    bool fullyUsed = true;

    for (int i = 0; i < count; i++)
    {
      if (!usedTree[(index + i) % count])
      {
        index = (index + i) % count;
        usedTree[index] = true;
        fullyUsed = false;
        break;
      }
    }

    if (fullyUsed) throw new IndexOutOfRangeException();

    Points points = new Points();
    for (int i = 0; i < 10; i++)
    {
      points.Add(i, pointsNum[i]);
    }

    users[index].SetUser(name, id, icon, points);
    usersDictionary.Add(id, users[index]);
  }

  public void AddUser(string name, string id, Texture icon)
  {
    this.AddUser(name, id, icon, new int[10]);
  }

  public void AddPoint(string id, int genre, int point)
  {
    User user;
    if (!usersDictionary.TryGetValue(id, out user)) throw new MissingFieldException();
    user.AddPoint(genre, point);
    cameraAnimator.SetTarget(user.GetPosition());

  }
}
