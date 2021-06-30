using UnityEngine;
using System.Collections.Generic;
using Thuleanx.Optimization;
using Thuleanx;

[CreateAssetMenu(fileName = "Pray", menuName = "~/Ability/Pray", order = 0)]
public class Pray : UnitAction {
	public BubblePool effect;
	public Pray(string name) : base(name) {}

	public override List<Cell> GetPossibleTargets(PlayableUnit punit) {
		return new List<Cell>{punit.grid.GetCell(punit.position.x, 
			punit.position.y)};
	}

	public override void PerformAction(PlayableUnit punit, Cell other) {
		// Does nothing
		effect?.Borrow(punit.grid.GetPosCenter(other.position), Quaternion.identity);

		App.LocalInstance._Narrator.OnPray(punit);
	}
}