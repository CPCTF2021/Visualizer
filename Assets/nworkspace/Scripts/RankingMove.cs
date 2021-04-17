﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RankingMove : MonoBehaviour
{
    [SerializeField]
    RectTransform thisPos;
    [SerializeField]
    float rightPosX;
    [SerializeField]
    float leftPosX;
    [SerializeField]
    float animTime;

    public void RightMove() { this.transform.DOLocalMove(new Vector3(rightPosX, 0.0f, 0.0f), animTime).SetEase(Ease.OutExpo); }
    public void LeftMove() { this.transform.DOLocalMove(new Vector3(leftPosX, 0.0f, 0.0f), animTime).SetEase(Ease.OutExpo); }
}
