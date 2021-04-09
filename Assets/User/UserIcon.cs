using UnityEngine;
using DG.Tweening;

namespace UserScripts
{
    public class UserIcon : MonoBehaviour
    {
        public float rotateSpeed = Mathf.PI * 0.1f;
        public void SetIcon(Texture texture)
        {
            gameObject.SetActive(true);
            GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
        }

        void Update()
        {
            transform.Rotate(0f, rotateSpeed, 0f);
        }

        public void AnimationIcon(float animationTime)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => 0f, (val) => {
                rotateSpeed = (val * 30f + 1f) * Mathf.PI * 0.1f;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * (val * 2f + 1f);
            }, 1f, 0.5f * animationTime).SetEase(Ease.OutBack));
            sequence.Append(DOTween.To(() => 1f, (val) => {
                rotateSpeed = (val * 30f + 1f) * Mathf.PI * 0.1f;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * (val * 2f + 1f);
            }, 0f, 0.4f * animationTime).SetEase(Ease.InQuart));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => {
                rotateSpeed = Mathf.PI * 0.1f;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            });
        }
    }
}
