using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Thuleanx;

public class Unit : Occupant {
	public Status status;
	public int vision = 2;
	// public int range = 3;

	public int[] dx = {-1, 1, 0, 0};
	public int[] dy = {0, 0, 1, -1};

	public Unit(Vector2Int position) : base(position) {
		status = new Status(1);
	}
	public virtual List<Vector2Int> GetReachablePositions() 
		=> new List<Vector2Int>{position};
	public void MakeSelectable(Action onSelect)
		=> gameObject.GetComponent<Selectable>().MakeSelectable(onSelect);
	public void DisableSelectable() 
		=> gameObject.GetComponent<Selectable>().DisableSelectable();

	public virtual void Knockback(Vector2Int kb) {
		Vector2Int kbDir = kb / (Mathf.Abs(kb.x + kb.y));
		Vector2Int kbDisplacement = Vector2Int.zero;
		while (kbDisplacement != kb && grid.InGrid(position + kbDisplacement + kbDir) && grid.Unoccupied(position + kbDisplacement + kbDir))
			kbDisplacement += kbDir;
		grid.MoveOccupant(position, position+kbDisplacement);
	}

	public virtual void OnDeath() {}
	public bool IsDead() => status.Health == 0;
	public bool CanMove() => !IsDead() && !status.AffectedBy((int) StatusEffectID.STUN);

	public void TakeDamage(Unit other, int damage_amount) {
		if (!IsDead()) {
			status.Health -= damage_amount;
			if (status.Health == 0) {
				OnDeath();
				App.LocalInstance._Narrator.OnKill(other, this, grid.GetCell(other.position));
			}
		}
	}

	public override void Move(Vector2Int target) {
		position = target;
		gameObject?.GetComponent<UnitObject>()?.Move(grid.GetPosCenter(target));
	}
}