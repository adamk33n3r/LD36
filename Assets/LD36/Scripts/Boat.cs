using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD36.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LD36.Scripts {
    public class Boat : MonoBehaviour {
        // TODO: rebalance these
        private float unloadTimeMedian = 2f;
        private float unloadTimeDelta = 1f;
        private float catchTimeMedian = 3f;
        private float catchTimeDelta = 2f;
        private bool emptying;

        private Net net;
        private ScriptableObjects.Boat boatData;
        private List<Fish> fish;
        private Dictionary<Species, TextDisplay> displayDict;
        private TextDisplay weightDisplay;

        private Action cbOnLeaveScreen;

        private void Awake() {
            this.net = GetComponentInChildren<Net>();
            this.fish = new List<Fish>();
            this.displayDict = new Dictionary<Species, TextDisplay>();
            this.weightDisplay = this.transform.Find("Canvas/Container/Left/Weight").GetComponent<TextDisplay>();
            this.transform.Find("Canvas/Container/Left/Move").GetComponent<Button>().onClick.AddListener(() => {
                LevelManager.Instance.MoveBoat(this.gameObject);
            });
            this.transform.Find("Canvas/Container/Left/Empty").GetComponent<Button>().onClick.AddListener(Empty);
        }

        private void Start() {
            foreach (Species species in Enum.GetValues(typeof(Species))) {
                GameObject go = this.transform.Find("Canvas/Container/FishCounts/"+species).gameObject;
                TextDisplay disp = go.GetComponent<TextDisplay>();
                disp.prefix = species.ToString();
                disp.UpdateText(0);
                this.displayDict.Add(species, disp);
            }
            // Net Lowered
            this.net.RegisterOnLoweredCallback((net) => {
                Debug.Log("Net lowered");
                float waitTime = this.catchTimeMedian + Random.Range(-this.catchTimeDelta, this.catchTimeDelta);

                // Wait for fish to "gather" and then pull back up
                StartCoroutine(WaitThenAction(waitTime, () => {
                    Debug.Log("Pulling up");
                    net.Raise();
                }));
            });

            // Net Raised
            this.net.RegisterOnRaisedCallback((net) => {
                Debug.Log("Net raised");
                Debug.Log("Unloading fish");
                // TODO: Have different "sized" fish
                Dictionary<Species, int> catchAmount = new Dictionary<Species, int>();
                foreach (Fish fish in net.Fish) {
                    this.fish.Add(fish);
                    if (!catchAmount.ContainsKey(fish.species)) {
                        catchAmount[fish.species] = 0;
                    }
                    catchAmount[fish.species]++;
                }
                foreach (KeyValuePair<Species, int> caught in catchAmount) {
                    Debug.Log(string.Format("Caught {0} {1}", caught.Value, caught.Key));
                    this.displayDict[caught.Key].UpdateText(caught.Value, true);
                }

                int weight = CalcWeight();
                this.weightDisplay.UpdateText(weight);

                Debug.Log(string.Format("calced: {0}, max: {1}", weight, this.boatData.maxWeight));
                if (weight >= this.boatData.maxWeight) {
                    Debug.Log("BOAT FULL!");
                    Empty();
                }

                if (this.emptying) {
                    return;
                }
                
                // Wait while unloading fish then send back down
                float waitTime = this.unloadTimeMedian + Random.Range(-this.unloadTimeDelta, this.unloadTimeDelta);
                StartCoroutine(WaitThenAction(waitTime, () => {
                    if (this.emptying) {
                        return;
                    }
                    Debug.Log("Sending back down");
                    net.Lower(this.boatData.netDepth);
                }));
            });

            // Wait for a time then start fishing!
            StartFishing();
        }

        public void StartFishing() {
            Debug.Log("Starting fishing");
            StartCoroutine(WaitThenAction(1, () => {
                this.net.Lower(this.boatData.netDepth);
            }));
        }

        private IEnumerator WaitThenAction(float seconds, Action action) {
            yield return new WaitForSeconds(seconds);
            action();
        }

        private IEnumerator CoroutineThenAction(IEnumerator coroutine, Action action) {
            while (coroutine.MoveNext()) {
                yield return null;
            }
            action();
        }

        private IEnumerator MoveOffscreen() {
            while (this.transform.position.x > -LevelManager.WORLD_WIDTH / 2f - 2) {
                this.transform.Translate(-Time.deltaTime, 0, 0);
                yield return null;
            }
        }

        private int CalcWeight() {
            return this.fish.Sum(fish => fish.weight);
        }

        public void Empty() {
            this.emptying = true;
            StartCoroutine(CoroutineThenAction(MoveOffscreen(), () => {
                LevelManager.Instance.AddFish(this.fish);
                Destroy(this.gameObject);
                if (this.cbOnLeaveScreen != null) {
                    this.cbOnLeaveScreen();
                }
            }));
        }

        public void SetBoatType(ScriptableObjects.Boat boatData) {
            this.boatData = boatData;
        }

        public void SetNetType(ScriptableObjects.Net netData) {
            this.net.SetNetType(netData);
        }

        public void RegisterOnLeaveCallback(Action callback) {
            this.cbOnLeaveScreen += callback;
        }
    }
}