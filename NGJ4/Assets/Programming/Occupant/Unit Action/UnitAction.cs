using UnityEngine;
using System.Collections.Generic;

public class UnitAction : ScriptableObject {
	public int ActionID;
	public string ActionName;
	public Sprite icon;

	public UnitAction(int id, string name) {
		ActionID = id;
		ActionName = name;
	}

	public virtual List<Cell> GetPossibleTargets(PlayableUnit punit) {
		return new List<Cell>();
	}
	public virtual void PerformAction(PlayableUnit punit, Cell other) {}
}