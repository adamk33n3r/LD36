using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD36.ScriptableObjects;
using UnityEngine;

namespace LD36.Scripts {
    public class Net : MonoBehaviour {
        public bool Lowered { get; protected set; }
        public bool Lowering { get; protected set; }

        public bool Raised {
            get { return !this.Lowered; }
            set { this.Lowered = !value; }
        }
        public bool Raising { get; protected set; }

        public List<Fish> Fish { get; protected set; }

        private Action<Net> cbOnLowered;
        private Action<Net> cbOnRaised;

        private float upSpeed;
        private float downSpeed;
        private ScriptableObjects.Net netData;

        private void Awake() {
            this.Fish = new List<Fish>();
        }

        private void Start () {
//            this.downSpeed = 0.001f + Mathf.Pow(0.0000000000001f, 1f / this.netData.holeSize) / 10;
            this.downSpeed = 0.001f + Mathf.Log(this.netData.holeSize, 10);
            this.upSpeed = this.downSpeed;
            Debug.Log("Down Speed: " + this.downSpeed);
            Debug.Log("Up Speed: " + this.upSpeed);
        }

        private void Update () {
        }

        public void Raise() {
            if (this.Raising) {
                return;
            }
            Debug.Log("Net: Raising");
            StartCoroutine(RaiseNet());
        }

        public void Lower(float toDepth) {
            this.Fish.Clear();
            if (this.Lowering) {
                return;
            }
            Debug.Log("Net: Lowering");
            StartCoroutine(LowerNet(toDepth));
        }

        public void RegisterOnLoweredCallback(Action<Net> cb) {
            this.cbOnLowered += cb;
        }

        public void RegisterOnRaisedCallback(Action<Net> cb) {
            this.cbOnRaised += cb;
        }

        private IEnumerator RaiseNet() {
            this.Raising = true;
            while (this.transform.position.y < 0) {
                this.transform.Translate(0, Time.deltaTime * this.upSpeed, 0);
                yield return null;
            }
            this.Raising = false;
            this.Raised = true;
            if (this.cbOnRaised != null) {
                this.cbOnRaised(this);
            }
        }

        private IEnumerator LowerNet(float depth) {
            this.Lowering = true;
            while (this.transform.position.y > -depth) {
                this.transform.Translate(0, -this.downSpeed * Time.deltaTime, 0);
                yield return null;
            }
            this.Lowering = false;
            this.Lowered = true;
            if (this.cbOnLowered != null) {
                this.cbOnLowered(this);
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (this.Lowering) {
                return;
            }
            School school = col.gameObject.GetComponent<School>();
            CatchFish(school);
            school.MakeVisible();
        }

        private void CatchFish(School school) {
            Debug.Log(string.Format("Net hole size: {0}, Fish size: {1}", this.netData.holeSize, school.Fish.size));
            float perc = Mathf.Clamp((float)school.Fish.size / this.netData.holeSize, 0, 1);

            Debug.Log("Percent: " + perc);
            int count = Mathf.FloorToInt(school.Count * perc);
            Debug.Log(string.Format("Going to try to put in {0} fish", count));

            // Get weight of current fish in net
            float currentWeight = CalcWeight();
            Debug.Log(string.Format("Current weight in net is {0}", currentWeight));
            float weightLeft = this.netData.weightCapacity - currentWeight;
            Debug.Log(string.Format("Weight left is {0}", weightLeft));
            float fishWeight = school.Fish.weight;
            Debug.Log(string.Format("Weight of species is {0}", fishWeight));
            int countFit = Mathf.FloorToInt(weightLeft / fishWeight);
            Debug.Log(string.Format("{0} fish can fit", countFit));
            int fishToPutIn = Mathf.Clamp(count, 0, countFit);
            Debug.Log(string.Format("Putting in {0} fish", fishToPutIn));

            if (fishToPutIn == 0) {
                return;
            }

            Debug.Log(string.Format("Adding {0} fish", fishToPutIn));
            for (int i = 0; i < fishToPutIn; i++) {
                this.Fish.Add(school.Fish);
            }
            school.TakeFish(fishToPutIn);
        }

        private float CalcWeight() {
            return this.Fish.Sum(fish => fish.weight);
        }

        public void SetNetType(ScriptableObjects.Net netData) {
            this.netData = netData;
        }
    }
}