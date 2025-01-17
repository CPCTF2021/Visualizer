﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static VisualizerSystem.ProblemSolvedEvent;
using static TreeScripts.TreeGenerator;

namespace UserScripts
{
    class Notification
    {
        public float targetPos = 0f;
        float pos = 0f;
        public float targetScale = 1f;
        float scale = 1f;
        public RectTransform transform;
        float time;
        public Notification(RectTransform transform)
        {
            this.transform = transform;
            time = Time.time;
        }

        public bool Update()
        {
            pos += (targetPos - pos) * 0.3f;
            scale += (targetScale - scale) * 0.3f;
            transform.position = new Vector3(transform.position.x, pos, transform.position.z);
            transform.localScale = new Vector3(scale, scale, 1f);

            if(Time.time - time >= 2f) return false;
            return true;
        }
    }
    public class NotificationManager : MonoBehaviour
    {
        List<Notification> transforms = new List<Notification>();
        [SerializeField]
        GameObject notification;

        void Start()
        {

        }

        public void Add(string name, Genre genre , float point)
        {
            Text text = Instantiate(notification, transform).GetComponent<Text>();
            text.text = $"{name.ToUpper()}   +{point}pts".Replace (' ', '\u00A0');
            text.color = GENRE_TO_COLOR[(int)genre];
            transforms.Add(new Notification(text.rectTransform));
            for(int i=1;i<transforms.Count;i++)
            {
                Notification not = transforms[transforms.Count - i - 1];
                not.targetPos = i * Screen.height * 0.07f + 0.05f;
                not.targetScale = i == 0 ? 1f : 0.8f;
                not.Update();
            }
        }

        void Update()
        {
            bool flag = false;
            for(int i=0;i<transforms.Count;i++)
            {
                flag = flag || !transforms[i].Update();
            }
            if(flag){
                Destroy(transforms[0].transform.gameObject);
                transforms.RemoveAt(0);
            }
        }
    }
}
