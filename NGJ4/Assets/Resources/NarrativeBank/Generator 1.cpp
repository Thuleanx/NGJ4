#include <bits/stdc++.h>
#include <dirent.h>
/* #include "boost/filesystem.hpp" */
#include <Windows.h>

using namespace std;
/* using namespace boost::filesystem; */

#define f(i,a,b) for (int i = a; i < b; i++)
#define fr(i,a,b) for (int i = b-1; i >= a; i--)
/* #define IN(i,a,b) ((i) >= (a) && (i) <= (b)) */

#define mp make_pair
#define mt make_tuple


using pii = pair<int,int>;
using ll = long long;


struct RULE {
	string rep;
	vector<string> sens;
};


mt19937 rng(chrono::steady_clock::now().time_since_epoch().count());

map<string, RULE> rules;

string parse(string cmd);

void dfs(stringstream &ss, RULE cur) {
	// choose one sentence in the rule
	string sent = cur.sens[uniform_int_distribution<int>(0, cur.sens.size()-1)(rng)];

	/* cout << "GOING WITH: " << cur.rep << " === " << sent << endl; */
	/* if (cur.sens.size() < 10) { */
	/* 	for (string s : cur.sens) */
	/* 		cout << s << " " << s.length() << endl; */
	/* } */
	ss << parse(sent);
}

void go_with(stringstream &ss, string rule) {
	if (!rules.count(rule)) {
		cerr << "ERROR: Rule " << rule << " not found in knowledge bank. Maybe check the spelling" << endl;
		throw "FORMAT ERROR";
	}
	dfs(ss, rules[rule]);
}

string parse(string cmd) {
	stringstream ss;

	string sr;
	bool srmode = 0;
	for (char c : cmd) {
		if (c == '#') {
			if (sr.length() && srmode == 1) go_with(ss, sr);
			srmode = 1;
			sr = "";
		} else {
			if (srmode == 1 && (c == ' ' || c == '.')) {
				if (sr.length()) go_with(ss, sr);
				srmode = 0;
				ss << c;
			} else if (srmode == 1) 
				sr.push_back(c);
			else
				ss << c;
		}
	}
	if (sr.length() && srmode == 1) {
		go_with(ss, sr);
	}

	return ss.str();
}

bool is_substance(string s) {
	for (char c : s) if (c != ' ') return 1;
	return 0;
}

void parse_file(std::ifstream &file) {
	// parse
	RULE current;
	bool found = 0;
	string line;
	while (getline(file, line)) {
		if (line.rfind("//", 0) == 0 || !is_substance(line))
			continue;

		if (line[0] == '+') {
			string rep = line.substr(1);
			while (rep.back() == ' ') rep.pop_back();

			if (found) {
				rules[current.rep] = current;
				cout << "LOADED: #" << current.rep << endl;
			}
			
			current = RULE();
			current.rep = rep;

			found = 1;
		} else {
			/* if (current.rep == "story") */
			/* 	cout << line << endl; */
			current.sens.push_back(line);
		}
	}
	if (found) {
		rules[current.rep] = current;
		cout << "LOADED: #" << current.rep << endl;
	}
}

inline bool ends_with(std::string const & value, std::string const & ending)
{
    if (ending.size() > value.size()) return false;
    return std::equal(ending.rbegin(), ending.rend(), value.rbegin());
}


/* void list_dir(path dir_path) { */
/* 	if (exists(dir_path)) { */
/* 		directory_iterator end_itr; */

/* 		for (directory_iterator itr(dir_path); itr != end_itr; ++itr) { */
/* 			/1* cout << itr->path().leaf() << endl; *1/ */
/* 			if (is_directory(itr->status())) { */
/* 				list_dir(itr->path()); */
/* 			} */
/* 			else if (ends_with(itr->path().leaf().c_str(), ".txt")) { */
/* 				string filename = itr->path().leaf().c_str(); */
/* 				if (filename != "input.txt" && filename != "output.txt") { */
/* 					std::ifstream file; */
/* 					cout << "============" << filename << "=================" << endl; */
/* 					file.open(filename); */
/* 					parse_file(file); */
/* 					file.close(); */
/* 				} */
/* 			} */
/* 		} */
/* 	} */
/* } */

vector<string> get_all_files_names_within_folder(string folder)
{
    vector<string> names;
    string search_path = folder + "/*.txt";
    WIN32_FIND_DATA fd;
    HANDLE hFind = ::FindFirstFile(search_path.c_str(), &fd);
    if(hFind != INVALID_HANDLE_VALUE) {
        do {
            // read all (real) files in current folder
            // , delete '!' read other 2 default folder . and ..
            if(! (fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) {
                names.push_back(fd.cFileName);
            }
        }while(::FindNextFile(hFind, &fd));
        ::FindClose(hFind);
    }
    return names;
}

void list_dir_windows() {
	for (string filename : get_all_files_names_within_folder("./")) {
		if (filename != "input.txt" && filename != "output.txt") {
			std::ifstream file;
			cout << "============" << filename << "=================" << endl;
			file.open(filename);
			parse_file(file);
			file.close();
		}
	}
}

int main() {
	ios_base::sync_with_stdio(0);
	cin.tie(0);

	/* list_dir(path("./")); */
	list_dir_windows();

	cout << "<<< Dictionary Loaded. Proceed with prompts. >>>" << endl;
	cout << "================================================" << endl;

	string cmd;
	while (getline(cin, cmd)) {
		try {
			cout << parse(cmd) << endl;
		} catch (const char* msg) {
			cout << "BAD" << endl;
			cout << msg << endl;
		}
	}


	return 0;
}
