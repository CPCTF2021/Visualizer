using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingMove : MonoBehaviour
{
    [SerializeField]
    float speed;

    bool isRightMove = false;
    bool isLeftMove = false;

    [SerializeField]
    RectTransform thisPos;
    [SerializeField]
    float rightPosX;
    [SerializeField]
    float leftPosX;

    void RightMove()
    {
        isLeftMove = false;
        Vector2 tmpPos = thisPos.anchoredPosition;
        tmpPos.x += Time.deltaTime * speed;
        if (tmpPos.x >= rightPosX)
        {
            isRightMove = false;
        }
        thisPos.anchoredPosition = tmpPos;
    }

    void LeftMove()
    {
        isRightMove = false;
        Vector2 tmpPos = thisPos.anchoredPosition;
        tmpPos.x -= Time.deltaTime * speed;
        if (tmpPos.x <= leftPosX)
        {
            isLeftMove = false;
        }
        thisPos.anchoredPosition = tmpPos;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.D))
        {
            isRightMove = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            isLeftMove = true;
        }
        */

        if (isRightMove)
        {
            RightMove();
        }

        if (isLeftMove)
        {
            LeftMove();
        }
    }
}
