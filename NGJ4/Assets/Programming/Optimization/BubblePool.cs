using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Thuleanx.Optimization {
	[CreateAssetMenu(fileName = "Pool", menuName = "~/Optimization/BubblePool", order = 0)]
	public class BubblePool : ScriptableObject {
		public GameObject prefab;
		[SerializeField] int expansionConstant = 10;

		[HideInInspector]
		public Queue<Bubble> bubblePool = new Queue<Bubble>();
		[HideInInspector]
		public Dictionary<Scene, List<Bubble>> BorrowToScene = new Dictionary<Scene, List<Bubble>>();


		public GameObject Borrow() {
			return Borrow(Vector2.zero, Quaternion.identity, SceneManager.GetActiveScene());
		}

		public GameObject Borrow(Scene bubbleScene) {
			return Borrow(Vector2.zero, Quaternion.identity, bubbleScene);
		}

		public GameObject Borrow(Vector3 position, Quaternion rotation) {
			return Borrow(position, rotation, SceneManager.GetActiveScene());
		}

		public GameObject Borrow(Vector3 position, Quaternion rotation, Scene bubbleScene) {
			if (bubblePool.Count==0) Expand(expansionConstant);
			Bubble bubble = bubblePool.Dequeue();
			bubble.gameObject.transform.position = position;
			bubble.gameObject.transform.rotation = rotation;
			bubble.gameObject.SetActive(true);
			bubble.inPool = false;
			if (!BorrowToScene.ContainsKey(bubbleScene))
				BorrowToScene[bubbleScene] = new List<Bubble>();
			if (!App.Instance.activePools.Contains(this))
				App.Instance.activePools.Add(this);

			BorrowToScene[bubbleScene].Add(bubble);
			return bubble.gameObject;
		}

		void Expand(int count) {
			while (count-->0) {
				GameObject obj = Instantiate(prefab, Vector2.zero, Quaternion.identity);
				obj.SetActive(false);
				DontDestroyOnLoad(obj);
				Bubble bubble = obj.AddComponent<Bubble>();
				bubble.Pool = this;
				bubble.inPool = true;
				bubblePool.Enqueue(bubble);
			}
		}

		public void Wipe() {
			while (bubblePool.Count > 0) {
				Bubble bubble = bubblePool.Dequeue();
				Destroy(bubble.gameObject);
			}
		}

		public void CollectsAll(Scene scene) {
			if (BorrowToScene.ContainsKey(scene))
				foreach (Bubble bubble in BorrowToScene[scene])
					if (bubble.gameObject.activeSelf)
						Collects(bubble);
			BorrowToScene.Clear();
		}

		public void Collects(Bubble bubble) {
			// Sus code
			if (bubble.gameObject.activeSelf) {
				bubble.transform.SetParent(null);
				bubble.gameObject.SetActive(false);
				bubblePool.Enqueue(bubble);
				DontDestroyOnLoad(bubble.gameObject);
				bubble.inPool = true;
			}
		}

		
	}
}