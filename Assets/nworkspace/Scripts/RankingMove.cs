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

    public void RightMove(float t) { this.transform.DOLocalMove(new Vector3(rightPosX, 0.0f, 0.0f), t).SetEase(Ease.OutExpo); }
    public void LeftMove(float t) { this.transform.DOLocalMove(new Vector3(leftPosX, 0.0f, 0.0f), t).SetEase(Ease.OutExpo); }
}
