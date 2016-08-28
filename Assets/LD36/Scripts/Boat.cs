using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD36.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LD36.Scripts {
    public class Boat : MonoBehaviour {
        private float unloadTimeMedian = 2f;
        private float unloadTimeDelta = 1f;
        private float catchTimeMedian = 3f;
        private float catchTimeDelta = 2f;
        private float netDepth = 3f;
        private float maxWeight = 500f;

        private Net net;
        private List<Species> fish;

        private void Awake() {
            this.fish = new List<Species>();
        }

        private void Start() {
            Dictionary<Species, TextDisplay> displayDict = new Dictionary<Species, TextDisplay>();
            foreach (Species species in Enum.GetValues(typeof(Species))) {
                displayDict.Add(species, GameObject.Find(species.ToString()).GetComponent<TextDisplay>());
            }

            this.net = GetComponentInChildren<Net>();

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
                foreach (KeyValuePair<Species, int> keyValuePair in net.Fish) {
                    Debug.Log(string.Format("Caught {0} {1}", keyValuePair.Value, keyValuePair.Key));
                    displayDict[keyValuePair.Key].UpdateText(keyValuePair.Value, true);
                    for (int i = 0; i < keyValuePair.Value; i++) {
                        this.fish.Add(keyValuePair.Key);
                    }
                }

                Debug.Log(string.Format("calced: {0}, max: {1}", CalcWeight(), this.maxWeight));
                if (CalcWeight() >= this.maxWeight) {
                    Debug.Log("BOAT FULL!");
                    StartCoroutine(CoroutineThenAction(MoveOffscreen(), () => {
                        Debug.Log("off screen now");
                        Destroy(this.gameObject);
                    }));
                    return;
                }
                
                // Wait while unloading fish then send back down
                float waitTime = this.catchTimeMedian + Random.Range(-this.catchTimeDelta, this.catchTimeDelta);
                StartCoroutine(WaitThenAction(waitTime, () => {
                    Debug.Log("Sending back down");
                    net.Lower(this.netDepth);
                }));
            });

            // Wait for a time then start fishing!
            StartCoroutine(WaitThenAction(1, () => {
                this.net.Lower(this.netDepth);
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
            while (this.transform.position.x > -GameManager.WORLD_WIDTH / 2f) {
                this.transform.Translate(-0.01f, 0, 0);
                yield return null;
            }
        }

        private float CalcWeight() {
            return this.fish.Sum(species => Fish.GetWeightOfSpecies(species));
        }
    }
}