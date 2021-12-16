#ifndef INCLUDED_NODE
#define INCLUDED_NODE

#include <vector>
#include <map>
#include <string>

class Node
{
public:
	enum State
	{
		None = 0,
		Adversary,
		Friend,
		Starting_Node
	};

	/*
	typedef std::pair<std::string, std::string> stringpair_t;
	const stringpair_t map_start_values[] = {
	  stringpair_t("Cat", "Feline"),
	  stringpair_t("Dog", "Canine"),
	  stringpair_t("Fish", "Fish") };
	*/

	//const map<int, string> StateMap{ {0, "None"}, {1, "Adversary"}, {2, "Friend"}, {3, "Starting node"} };

	/*
	const StateChar StateMap;
	{
	  (None, "None"),
	  (Adversary, "Adversary"),
	  (Friend, "Friend"),
	  (Starting_Node, "Starting node")
	};*/



	void SetNodeNummer(int c);
	int GetNodeNummer();

	State GetAdversarysState();
	State GetSelfState();
	std::string GetSelfStateString();

	void SetStartNode();
	void SetState(State s);


	void SetNodesConnected(std::vector<Node*> nodesCon);

	std::vector<Node*> GetNodesConnected();


private:
	State selfState;
	std::vector<Node*> nodesConnected;
	int nodeNummer = 0;
};

#endif