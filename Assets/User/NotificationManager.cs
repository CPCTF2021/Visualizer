using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserScripts
{
    class Notification
    {
        float targetPos = 0f;
        float pos = 0f;
        float targetScale = 1f;
        float scale = 1f;
        RectTransform transform;
        public Notification(RectTransform transform)
        {
            this.transform = transform;
        }

        public void Update()
        {
            pos += (targetPos - pos) * 0.5f;
            scale += (targetScale - scale) * 0.5f;
            transform.position = new Vector3(transform.position.x, pos, transform.position.z);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    public class NotificationManager : MonoBehaviour
    {
        List<Notification> transforms = new List<Notification>();

        void Start()
        {

        }
    }
}
