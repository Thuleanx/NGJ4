using UnityEngine;
using System.IO;
using System.Collections.Generic;

class Rule {
	public string token;
	public List<string> possibleSentences = new List<string>();

	public Rule(string token) { this.token = token; }
}

public class NarrativeGenerator : MonoBehaviour {
	Dictionary<string, Rule> ruleMap = new Dictionary<string, Rule>();
	mt19937 rng = new mt19937(1234);

	void Awake() { LoadBanks(); }
	void Start() {
	}

	void LoadBanks() {
		TextAsset[] textFiles = Resources.LoadAll<TextAsset>("NarrativeBank");
		Debug.Log(textFiles.Length);
		foreach (TextAsset textFile in textFiles) {
			Debug.Log("Parsing file: " + textFile.name);
			ParseFile(textFile.text);
		}
	}

	bool isSubstantive(string line) => !string.IsNullOrWhiteSpace(line);
	void ParseFile(string plainText) {
		Rule current = null;
		bool found = false;

		var result = plainText.Split(new [] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < result.Length; i++) {
			string line = result[i];

			if (line.StartsWith("//") || !isSubstantive(line)) continue;

			if (line.StartsWith("+")) {
				string token = line.Substring(1).Trim();
				if (found) {
					ruleMap[current.token] = current;
					Debug.Log("loaded: " + current.token);
				}
				found = true;
				current = new Rule(token);
			} else current.possibleSentences.Add(line);
		}

		if (found) {
			ruleMap[current.token] = current;
			Debug.Log("loaded: " + current.token);
		}
	}
	string dfs(string rule) {
		if (!ruleMap.ContainsKey(rule)) {
			Debug.LogError("Rule " + rule + " not found within knowledge bank. Please check your spelling / syntax.");
			throw new System.Exception("Format error");
		}
		Rule current = ruleMap[rule];
		return parse(current.possibleSentences[rng.Range(0, current.possibleSentences.Count-1)]);
	}	
	public string parse(string cmd) {
		string res = "";

		string temp = "";
		bool parsingCmd = false;

		foreach (char c in cmd) {
			if (c == '#') {
				if (temp.Length > 0 && parsingCmd) res += dfs(temp);
				parsingCmd = true;
				temp = "";
			} else {
				if (parsingCmd && (c == ' ' || c == '.' || c == ',')) {
					if (temp.Length > 0) res += dfs(temp);
					parsingCmd = false;
					res += c;
				} else if (parsingCmd)
					temp += c;
				else res += c;
			}
		}
		if (parsingCmd && temp.Length > 0)
			res += dfs(temp);
		return res;
	}
}