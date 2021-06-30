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
	Dictionary<string, string> ruleOverride = new Dictionary<string, string>();
	mt19937 rng = new mt19937(1234);

	void Awake() { LoadBanks(); }
	void Start() {
	}

	void LoadBanks() {
		TextAsset[] textFiles = Resources.LoadAll<TextAsset>("NarrativeBank");
		// Debug.Log(textFiles.Length);
		foreach (TextAsset textFile in textFiles) {
			// Debug.Log("Parsing file: " + textFile.name);
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
					// Debug.Log("loaded: " + current.token);
				}
				found = true;
				current = new Rule(token);
			} else current.possibleSentences.Add(line);
		}

		if (found) {
			ruleMap[current.token] = current;
			// Debug.Log("loaded: " + current.token);
		}
	}
	string dfs(string rule) {
		if (ruleOverride.ContainsKey(rule))	return parse_internal(ruleOverride[rule]);
		if (!ruleMap.ContainsKey(rule)) {
			Debug.LogError("Rule " + rule + " not found within knowledge bank. Please check your spelling / syntax.");
			// throw new System.Exception("Format error");
			return "";
		}
		Rule current = ruleMap[rule];
		return parse_internal(current.possibleSentences[rng.Range(0, current.possibleSentences.Count-1)]);
	}	
	public string parse_internal(string cmd) {
		string res = "";

		string temp = "";
		bool parsingCmd = false;

		foreach (char c in cmd) {
			if (c == '#') {
				if (temp.Length > 0 && parsingCmd) res += dfs(temp);
				parsingCmd = true;
				temp = "";
			} else {
				if (parsingCmd && (c == ' ' || c == '.' || c == ',' || c == '?' || c == '!' || c == ';' || c == 39 || c == 34)) {
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

	public string parse(string cmd) {
		try {
			string res = parse_internal(cmd);
			res = res.Replace("%", "\n");
			return res;
		} catch {
			return "";
		}
	}

	public void ClearOverrides() => ruleOverride.Clear();

	public void Load(CharacterInfo info) {
		ruleOverride["charactername"] = info.fullName;
		ruleOverride["fname"] = info.firstName;
		ruleOverride["lname"] = info.lastName;
		ruleOverride["spronoun"] = info.gender.SPronoun();
		ruleOverride["opronoun"] = info.gender.OPronoun();
		ruleOverride["ppronoun"] = info.gender.PPronoun();
		ruleOverride["wpn"] = info.equipment;
		ruleOverride["jobs"] = info.job;
		ruleOverride["traits"] = info.trait;
	}

	public void Load(EnemyInfo info) {
		ruleOverride["enemyz"] = ruleOverride["enemyc"] = ruleOverride["enemys"] = info.trait;
		ruleOverride["enemyType"] = info.enemyType.Name();
		ruleOverride["enemy_type"] = info.enemyType.Name();
	}

	public void Load(PlayableUnit p1, PlayableUnit p2, PlayableUnit p3) {
		ruleOverride["charactername1"] = p1.info.fullName;
		ruleOverride["fname1"] = p1.info.firstName;
		ruleOverride["lname1"] = p1.info.lastName;
		ruleOverride["spronoun1"] = p1.info.gender.SPronoun();
		ruleOverride["opronoun1"] = p1.info.gender.OPronoun();
		ruleOverride["ppronoun1"] = p1.info.gender.PPronoun();
		ruleOverride["wpn1"] = p1.info.equipment;
		ruleOverride["jobs1"] = p1.info.job;
		ruleOverride["traits1"] = p1.info.trait;

		ruleOverride["charactername2"] = p2.info.fullName;
		ruleOverride["fname2"] = p2.info.firstName;
		ruleOverride["lname2"] = p2.info.lastName;
		ruleOverride["spronoun2"] = p2.info.gender.SPronoun();
		ruleOverride["opronoun2"] = p2.info.gender.OPronoun();
		ruleOverride["ppronoun2"] = p2.info.gender.PPronoun();
		ruleOverride["wpn2"] = p2.info.equipment;
		ruleOverride["jobs2"] = p2.info.job;
		ruleOverride["traits2"] = p2.info.trait;

		ruleOverride["charactername3"] = p3.info.fullName;
		ruleOverride["fname3"] = p3.info.firstName;
		ruleOverride["lname3"] = p3.info.lastName;
		ruleOverride["spronoun3"] = p3.info.gender.SPronoun();
		ruleOverride["opronoun3"] = p3.info.gender.OPronoun();
		ruleOverride["ppronoun3"] = p3.info.gender.PPronoun();
		ruleOverride["wpn3"] = p3.info.equipment;
		ruleOverride["jobs3"] = p3.info.job;
		ruleOverride["traits3"] = p3.info.trait;
	}
}