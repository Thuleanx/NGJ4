using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Selectable))]
public class Selectable : MonoBehaviour {
	public bool Active;
	public Action OnSelected;

	public static Selectable GetSelectable(Vector2 worldPosition, LayerMask selectableMask) {
		RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 0f, selectableMask);
		if (hit) return hit.collider.gameObject.GetComponent<Selectable>();
		return null;
	}

	public void MakeSelectable(Action onSelect) {
		Active = true;
		OnSelected = onSelect;
	}

	public void DisableSelectable() {
		Active = false;
		OnSelected = null;
	}
}