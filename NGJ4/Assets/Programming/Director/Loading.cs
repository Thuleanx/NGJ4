using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

namespace Thuleanx.Master {
	public class Loading : MonoBehaviour
	{
		public int SceneToLoad = 1;
		void Update()
		{
			try {
				if (FMODUnity.RuntimeManager.HasBanksLoaded)
				{
					Debug.Log("Master Bank Loaded");
					SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
					App.Instance._GameMaster.StartGame();
				} else {
					Debug.Log("Master Bank Not Yet Loaded " + FMODUnity.RuntimeManager.AnyBankLoading());
				}
			} catch (Exception err) {
				Debug.Log(err);
			}

		}
	}
}
