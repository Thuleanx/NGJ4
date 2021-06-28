using UnityEngine;
using System.Collections.Generic;

public class UnitAction : ScriptableObject {
	public string ActionName;
	public Sprite icon;

	public UnitAction(string name) {
		ActionName = name;
	}

	public virtual List<Cell> GetPossibleTargets(PlayableUnit punit) {
		return new List<Cell>();
	}
	public virtual void PerformAction(PlayableUnit punit, Cell other) {}
}