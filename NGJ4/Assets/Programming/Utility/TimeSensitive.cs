using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Thuleanx.Utility {
	[Serializable]
	public class TimeSensitive<T> {
		float timeLastCheck;
		[SerializeField] public T val;

		[HideInInspector]
		public UnityEvent<float> OnUpdate;
		[HideInInspector]
		public UnityEvent<T,T> OnChange;

		public TimeSensitive(T initValue) {
			val = initValue;
			timeLastCheck = 0;
			OnUpdate = new UnityEvent<float>();
			OnChange = new UnityEvent<T, T>();
		}

		void Update() {
			OnUpdate?.Invoke(Time.time - timeLastCheck);
			timeLastCheck = 0;
		}

		public T Value {
			get {
				Update();
				return val;
			}
			set {
				OnChange?.Invoke(val, value);
				val = value;
				timeLastCheck = Time.time;
			}
		}
	}
}
