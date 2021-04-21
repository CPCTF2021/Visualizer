using UnityEngine;
using UnityEngine.UI;

namespace VisualizerSystem
{
    public class Timer : MonoBehaviour
    {
        public Text timerText;
        public const float MAX_TIME_LIMIT = 20000.0f;
        public float limitTime = MAX_TIME_LIMIT;

        private void Update()
        {
            limitTime -= Time.deltaTime;
            SetTimerText(Float2ParsedTime(limitTime));
        }

        public struct ParsedTime
        {
            public int hour;
            public int minute;
            public int second;
            public int millisecond;
        }

        public ParsedTime Float2ParsedTime(float time)
        {
            ParsedTime t;
            t.hour = (int)Mathf.Floor(time / (60 * 60));
            t.minute = (int)Mathf.Floor((time % (60 * 60)) / 60);
            t.second = (int)Mathf.Floor(time % 60);
            t.millisecond = (int)Mathf.Floor(time % 1 * 1000);

            return t;
        }

        public void SetTimerText(ParsedTime t)
        {
            timerText.text = $"{t.hour:D2}:{t.minute:D2}:{t.second:D2}.{t.millisecond:D3}";
        }
    }
}

