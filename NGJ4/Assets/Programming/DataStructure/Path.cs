using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Path {
	public List<Vector2Int> nodes = new List<Vector2Int>();

	public void reverse() => nodes.Reverse();
	public void add_node(Vector2Int pos) => nodes.Add(pos);
	public void pop_node() => nodes.RemoveAt(nodes.Count-1);

	public int Length => nodes.Count;
	public Vector2Int Last => nodes[Length-1];
}