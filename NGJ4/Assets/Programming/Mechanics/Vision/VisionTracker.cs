using UnityEngine;
using System.Collections.Generic;
using Thuleanx.Math;

[RequireComponent(typeof(SpriteRenderer))]
public class VisionTracker : MonoBehaviour {
	[SerializeField] float visionFadeoff = .5f;
	[SerializeField] float alphaPosition = 8f, alphaRange = 8f;
	Material mat;

	void Awake() {
		mat = GetComponent<SpriteRenderer>().material;
	}

	List<PlayableUnit> units = new List<PlayableUnit>();

	public void AddUnit(PlayableUnit unit) => units.Add(unit);
	public void ClearUnits() => units.Clear();

	void Start() {
		for (int i = 0; i < 4; i++) {
			mat.SetVector("_VisionPosition" + i, Vector2.zero);
			mat.SetVector("_VisionFadeRange" + i, Vector2.zero);
		}
	}

	void Update() {
		for (int i = 0; i < 4; i++) {

			Vector2 originalPos = mat.GetVector("_VisionPosition" + i);
			Vector2 originalRange = mat.GetVector("_VisionFadeRange" + i);

			Vector2 wantPos, wantRange;

			if (i < units.Count) {
				wantPos = units[i].WorldPosition;
				wantRange = units[i].IsDead() ? new Vector2(0, visionFadeoff) : 
					new Vector2(units[i].vision - visionFadeoff/2, units[i].vision + visionFadeoff/2);
			} else {
				wantPos = Vector2.zero;
				wantRange = Vector2.zero;
			}

			Vector2 nxtPos = Calc.Damp(originalPos, wantPos, alphaPosition, Time.deltaTime);
			Vector2 nxtRange = Calc.Damp(
				originalRange,
				wantRange,
				alphaRange,
				Time.deltaTime
			);

			mat.SetVector("_VisionPosition" + i, nxtPos);
			mat.SetVector("_VisionFadeRange" + i, nxtRange);
		}
	}
}