using UnityEngine;

public class ObstacleObject : MonoBehaviour {
	public Obstacle Create(Vector2Int position) {
		Obstacle obstacle = new Obstacle(position);
		obstacle.Attach(gameObject);
		return obstacle;
	}
}