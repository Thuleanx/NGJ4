using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Thuleanx.Utility;

namespace Thuleanx.Optimization {
	public class Bubble : MonoBehaviour {
		[HideInInspector]
		public BubblePool Pool;
		[HideInInspector]
		public bool inPool = false;

		void OnDisable() {
			App.Instance?.StartCoroutine(General._InvokeNextFrame(() => {
				if (!inPool) Pool?.Collects(this);
			}));
		}
	}
}