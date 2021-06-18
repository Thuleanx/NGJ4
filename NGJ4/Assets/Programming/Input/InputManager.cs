using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
	public LayerMask selectable;
	[HideInInspector] public Vector2 MouseScreenPos = Vector2.zero;
	public Vector2 MouseWorldPos {
		get {
			if (Camera.main != null)
				return Camera.main.ScreenToWorldPoint(MouseScreenPos);
			return Vector2.zero;
		}
	}

	public void OnMousePosInput(InputAction.CallbackContext context) => MouseScreenPos = context.ReadValue<Vector2>();
	public void OnMouseClick(InputAction.CallbackContext context) {
		if (context.started) {
			Selectable obj = Selectable.GetSelectable(MouseWorldPos, selectable);
			if (obj != null && obj.Active) obj.OnSelected?.Invoke();
		}
	}
}