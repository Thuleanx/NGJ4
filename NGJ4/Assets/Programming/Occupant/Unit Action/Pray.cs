using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Pray", menuName = "~/Ability/Pray", order = 0)]
public class Pray : UnitAction {
	public Pray(string name) : base(name) {}

	public override List<Cell> GetPossibleTargets(PlayableUnit punit) {
		return new List<Cell>{punit.grid.GetCell(punit.position.x, 
			punit.position.y)};
	}

	public override void PerformAction(PlayableUnit punit, Cell other) {
		// Does nothing
	}
}