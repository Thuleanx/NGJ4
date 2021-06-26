using UnityEngine;
using System.Collections.Generic;

public class Pray : UnitAction {
	public Pray(int id, string name) : base(id, name) {}

	public override List<Cell> GetPossibleTargets(PlayableUnit punit) {
		return new List<Cell>{punit.grid.GetCell(punit.position.x, 
			punit.position.y)};
	}

	public override void PerformAction(PlayableUnit punit, Cell other) {
		// Does nothing
	}
}