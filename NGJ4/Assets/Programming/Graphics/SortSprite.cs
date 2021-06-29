
using UnityEngine;
using UnityEngine.Rendering;

public class SortSprite : MonoBehaviour {
	void Update() {
		float pos = transform.position.y;
		int order = (int)(-pos * 64f);
		SortingGroup group = GetComponent<SortingGroup>();
		if (group)
			group.sortingOrder = order;
		else foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
				renderer.sortingOrder = order;
	}
}
