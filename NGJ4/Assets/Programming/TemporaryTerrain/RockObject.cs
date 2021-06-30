using UnityEngine;
using Thuleanx.Optimization;
using Thuleanx;

public class RockObject : MonoBehaviour {
	[SerializeField] BubblePool CollectEffect;

	public void OnCollect() {
		CollectEffect?.Borrow(transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}
}