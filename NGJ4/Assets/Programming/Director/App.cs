using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;	
using Thuleanx.Optimization;
using FMOD_Thuleanx;

namespace Thuleanx {
	public class App : MonoBehaviour {
		public static bool IsEditor = false;

		public static App Instance;
		public static LocalApp LocalInstance;

		public InputManager _InputManager;
		public NarrativeGenerator _NarrativeGenerator;
		public GameMaster _GameMaster;
		public AudioManager _AudioManager;

		[HideInInspector]
		public List<BubblePool> activePools = new List<BubblePool>();

		void Awake() {
			Instance = this;
			SceneManager.sceneLoaded += OnNewScene;
			App.LocalInstance = GameObject.FindObjectOfType<LocalApp>();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Bootstrap() {
			var app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
			if (app == null) throw new ApplicationException();
			UnityEngine.Object.DontDestroyOnLoad(app);
		}

		public void OnNewScene(Scene scene, LoadSceneMode mode) {
			App.LocalInstance = GameObject.FindObjectOfType<LocalApp>();			
		}
	}
}