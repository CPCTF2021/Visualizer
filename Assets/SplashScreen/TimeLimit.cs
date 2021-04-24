using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeLimit : MonoBehaviour
{
    // Start is called before the first frame update
    public void MoveScene() {
        SceneManager.LoadScene("Production");
    }
}
