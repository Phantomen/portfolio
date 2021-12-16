
#include "Node.h"

using namespace std;


Node::State Node::GetAdversarysState()
{
	State s;
	switch (selfState)
	{
		case State::Adversary :
		{
			s = State::Friend;
			break;
		}

		case State::Friend :
		{
			s = State::Adversary;
			break;
		}

		case State::Starting_Node :
		{
			s = State::Adversary;
			break;
		}

		case State::None:
		{
			//BUG, break
			s = State::Adversary;
			break;
		}
	}

	return s;
}

void Node::SetNodeNummer(int c)
{
	nodeNummer = c;
}

int Node::GetNodeNummer()
{
	return nodeNummer;
}


Node::State Node::GetSelfState()
{
	return selfState;
}

std::string Node::GetSelfStateString()
{
	string s = "NULL";
	switch (selfState)
	{
		case State::Adversary:
		{
			s = "Adversary";
			break;
		}

		case State::Friend:
		{
			s = "Friend";
			break;
		}

		case State::Starting_Node:
		{
			s = "Starting node";
			break;
		}

		case State::None:
		{
			//BUG, break
			s = "OUTSIDE";
			break;
		}
	}

	return s;
}


void Node::SetStartNode()
{
	selfState = State::Starting_Node;
}

void Node::SetState(State s)
{
	selfState = s;
}


void Node::SetNodesConnected(vector<Node*> nodesCon)
{
	//nodesConnected = nodesCon;
	for (int i = 0; i < nodesCon.size(); i++)
	{
		nodesConnected.push_back(nodesCon[i]);
	}
}

vector<Node*> Node::GetNodesConnected()
{
	return nodesConnected;
}