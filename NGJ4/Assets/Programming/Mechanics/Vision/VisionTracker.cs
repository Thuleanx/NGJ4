using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class VisionTracker : MonoBehaviour {
	[SerializeField] float visionFadeoff = .5f;
	Material mat;

	void Awake() {
		mat = GetComponent<SpriteRenderer>().material;
	}

	List<PlayableUnit> units = new List<PlayableUnit>();

	public void AddUnit(PlayableUnit unit) => units.Add(unit);
	public void ClearUnits() => units.Clear();

	void Update() {
		for (int i = 0; i < 4; i++) {
			if (i < units.Count) {
				mat.SetVector("_VisionPosition" + i, units[i].WorldPosition);
				mat.SetVector("_VisionFadeRange" + i, new Vector2(
					units[i].vision - visionFadeoff/2, units[i].vision + visionFadeoff/2));
			} else {
				mat.SetVector("_VisionFadeRange" + i, Vector2.zero);
			}
		}
	}
}