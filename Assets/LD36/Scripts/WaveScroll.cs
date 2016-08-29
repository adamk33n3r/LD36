using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class WaveScroll : MonoBehaviour {
    public float depth;
    private Material mat;
    private bool up;
    private Vector3 initial;

    private void Awake() {
        this.mat = GetComponent<MeshRenderer>().material;
    }

    private void Start() {
        this.initial = this.transform.position;
        StartCoroutine(WaveDown(this.initial, new Vector3(0, this.initial.y - this.depth, this.initial.z), 1));
    }

    private void Update() {
        float x = this.mat.mainTextureOffset.x - Time.deltaTime * 0.1f;
        if (x < -1) {
            x = 0;
        }
        this.mat.mainTextureOffset = new Vector2(x, 0);
    }

    private IEnumerator WaveUp(Vector3 start, Vector3 end, float seconds) {
        float t = 0f;
        while (t <= 1f) {
            t += Time.deltaTime / seconds;
            this.transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        StartCoroutine(WaveDown(this.transform.position, new Vector3(0, this.initial.y - this.depth, this.initial.z), 1));
    }

    private IEnumerator WaveDown(Vector3 start, Vector3 end, float seconds) {
        float t = 0f;
        while (t <= 1f) {
            t += Time.deltaTime / seconds;
            this.transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        StartCoroutine(WaveUp(this.transform.position, this.initial, 1));
    }
}
