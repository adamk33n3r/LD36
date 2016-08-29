using System;
using System.Collections;
using LD36.ScriptableObjects;
using UnityEngine;

namespace LD36.Scripts {
    public class School : MonoBehaviour {
        public Fish Fish { get; protected set; }
        public int Count { get; protected set; }

        [SerializeField]
        private ScriptableObjects.School schoolData;

        private SpriteRenderer spriteRenderer;

        private void Awake () {
            this.spriteRenderer = GetComponent<SpriteRenderer>();
            Color orig = this.spriteRenderer.color;
            this.spriteRenderer.color = new Color(orig.r, orig.g, orig.b, 0);
            this.Fish = this.schoolData.fish;
            this.Count = this.schoolData.count;
        }

        public void TakeFish(int count) {
            this.Count -= count;
            if (this.Count <= 0) {
                Destroy(this.gameObject);
            }
        }

        public void MakeVisible() {
            if (this.spriteRenderer.color.a < 1) {
                StartCoroutine(TransitionAlpha(1));
            }
        }

        public void MakeHidden() {
            if (this.spriteRenderer.color.a > 0) {
                StartCoroutine(TransitionAlpha(0));
            }
        }

        private IEnumerator TransitionAlpha(float alpha) {
            float diff = alpha - this.spriteRenderer.color.a;
            float dir = Mathf.Sign(diff);
            Color orig = this.spriteRenderer.color;
            while (diff > 0 ? this.spriteRenderer.color.a < alpha : this.spriteRenderer.color.a > alpha) {
                this.spriteRenderer.color = new Color(orig.r, orig.g, orig.b, this.spriteRenderer.color.a + 0.01f * dir);
                yield return null;
            }
        }
    }
}