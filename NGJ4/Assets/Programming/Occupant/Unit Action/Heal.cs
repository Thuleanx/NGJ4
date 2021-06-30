using UnityEngine;
using System.Collections.Generic;
using Thuleanx.Optimization;
using Thuleanx;

[CreateAssetMenu(fileName = "Heal", menuName = "~/Ability/Heal", order = 0)]
public class Heal : UnitAction {
	public int range = 3;
	public BubblePool effect;

	public Heal(string name) : base(name) {}

	public override List<Cell> GetPossibleTargets(PlayableUnit punit) {
		List<Cell> cells = new List<Cell>();
		foreach (Unit unit in punit.grid.GetAllUnits())
			if (unit is PlayableUnit && !unit.IsDead() && Grid.Approx_Distance(punit.position, unit.position) < range)
				cells.Add(punit.grid.GetCell(unit.position));
		return cells;
	}

	public override void PerformAction(PlayableUnit punit, Cell other) {
		// Does nothing
		PlayableUnit pother = other.Occupant as PlayableUnit;
		pother.status.Health++;
		effect?.Borrow(punit.grid.GetPosCenter(other.position), Quaternion.identity);

		App.LocalInstance._Narrator.OnHeal(punit, pother);
	}
}