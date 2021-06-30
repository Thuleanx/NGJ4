using UnityEngine;
using Thuleanx.Optimization;
using Thuleanx;

public class RockObject : MonoBehaviour {
	[SerializeField] BubblePool CollectEffect;
	[SerializeField, FMODUnity.EventRef] string collectSound;

	public void OnCollect() {
		CollectEffect?.Borrow(transform.position, Quaternion.identity);
		gameObject.SetActive(false);
		App.Instance._AudioManager.PlayOneShot(collectSound);
	}
}