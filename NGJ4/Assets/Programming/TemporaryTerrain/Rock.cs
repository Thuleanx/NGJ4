using UnityEngine;
using Thuleanx;

public class Rock : TemporaryTerrain {

	public override void Enter(Occupant occupant) {
		if (occupant is PlayableUnit) {
			// Counter Increment
			App.Instance._GameMaster.RocksGathered += 
				grid.GetBiome(position).biomeType.MeteoritePerChunk();
			App.LocalInstance._Narrator.OnCollectRock(occupant as PlayableUnit, 
				grid.GetCell(position));
			gameObject.GetComponent<RockObject>().OnCollect();
		}
	}
}