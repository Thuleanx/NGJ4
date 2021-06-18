using System;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<TItem, TPriority> where TPriority : IComparable<TPriority> {

	List<KeyValuePair<TPriority, TItem>> tree;

	public PriorityQueue() { tree = new List<KeyValuePair<TPriority, TItem>>(); }

	int parent(int x) => (x-1)/2;
	int left_child(int x) => 2*x+1;
	int right_child(int x) => 2*x+2;

	public void push(TItem node, TPriority prio) {
		tree.Add(new KeyValuePair<TPriority, TItem>(prio, node));
		int cur = tree.Count-1;
		while (cur > 0) {
			if (tree[cur].Key.CompareTo(tree[parent(cur)].Key) < 0)
				swap(cur, parent(cur));
			else break;
			cur = parent(cur);
		}
	}

	void swap(int i, int j) {
		KeyValuePair<TPriority, TItem> kvp = tree[i];
		tree[i] = tree[j]; tree[j] = kvp;
	}

	public void pop() {
		swap(0, tree.Count - 1);
		tree.RemoveAt(tree.Count-1);
		int cur = 0;
		while (left_child(cur) < tree.Count) {
			int x = left_child(cur), y = right_child(cur);
			if (y >= tree.Count) {
				if (tree[cur].Key.CompareTo(tree[left_child(cur)].Key) > 0) {
					swap(cur, x);
					cur = x;
				} else break;
			} else {
				if (tree[x].Key.CompareTo(tree[y].Key) > 0) {
					x^=y; y^=x; x^=y;
				}

				if (tree[cur].Key.CompareTo(tree[x].Key) > 0) {
					swap(cur, x); cur=x;
				} else if (tree[cur].Key.CompareTo(tree[y].Key) > 0) {
					swap(cur, y); cur=y;
				} else break;
			}
		}
	}

	public TItem peek() => tree[0].Value;
	public TPriority peek_prio() => tree[0].Key;

	public void clear() => tree.Clear();
	
	public int Count { get { return tree.Count; } }
}