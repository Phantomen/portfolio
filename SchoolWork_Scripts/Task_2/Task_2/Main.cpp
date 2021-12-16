
#include <iostream>
#include <vector>
#include <queue>
#include "Node.h"

using namespace std;

vector<Node*> CreateNodes();
void connectNodes(vector<Node*> nodes);
queue<Node*> DFS_Algorithm(vector<Node*> nodes, int startNodeIndex);
bool NodeInQueue(Node* n, queue<Node*> q);
void PrintQueue(queue <Node*> q);

int main()
{
	vector<Node*> nodes = CreateNodes();
	connectNodes(nodes);
	queue<Node*> cq = DFS_Algorithm(nodes, 2);
	PrintQueue(cq);
	return 0;
}

vector<Node*> CreateNodes()
{
	vector<Node*> nodes;
	int numberOfNodes = 7;

	for (int i = 0; i < numberOfNodes; i++)
	{
		Node* n = new Node;
		n->SetNodeNummer(i);
		nodes.push_back(n);
	}

	return nodes;
}

void connectNodes(vector<Node*> nodes)
{
	vector<Node*> n;
	n.push_back(nodes[1]);
	n.push_back(nodes[2]);
	nodes[0]->SetNodesConnected(n);

	n.clear();
	n.push_back(nodes[1]);
	n.push_back(nodes[3]);
	n.push_back(nodes[4]);
	nodes[1]->SetNodesConnected(n);

	n.clear();
	n.push_back(nodes[0]);
	n.push_back(nodes[5]);
	n.push_back(nodes[6]);
	nodes[2]->SetNodesConnected(n);

	n.clear();
	n.push_back(nodes[1]);
	n.push_back(nodes[4]);
	nodes[3]->SetNodesConnected(n);

	n.clear();
	n.push_back(nodes[1]);
	n.push_back(nodes[3]);
	nodes[4]->SetNodesConnected(n);

	n.clear();
	n.push_back(nodes[1]);
	n.push_back(nodes[2]);
	nodes[5]->SetNodesConnected(n);

	n.clear();
	n.push_back(nodes[2]);
	nodes[6]->SetNodesConnected(n);
}

queue<Node*> DFS_Algorithm (vector<Node*> nodes, int startNodeIndex)
{
	queue<Node*> waitQueue;
	queue<Node*> completedQueue;
	waitQueue.push(nodes[startNodeIndex]);
	nodes[startNodeIndex]->SetStartNode();

	Node* n;

	while (!waitQueue.empty())
	{
		n = waitQueue.front();
		waitQueue.pop();
		completedQueue.push(n);

		vector<Node*> cn = n->GetNodesConnected();

		for (int i = 0; i < cn.size(); i++)
		{
			if (!NodeInQueue(cn[i], completedQueue) && !NodeInQueue(cn[i], waitQueue))
			{
				cn[i]->SetState(n->GetAdversarysState());
				waitQueue.push(cn[i]);
			}
		}
	}

	return completedQueue;
}

bool NodeInQueue(Node* n, queue<Node*> q)
{
	bool inQueue = false;
	while(q.size() > 0)
	{
		Node* cn = q.front();
		q.pop();

		if (n == cn)
		{
			inQueue = true;
			return inQueue;
		}
	}

	return inQueue;
}

void PrintQueue(queue <Node*> q)
{
	vector<Node*> n;

	while (!q.empty())
	{
		n.push_back(q.front());
		q.pop();
	}

	for (int i = 0; i < n.size(); i++)
	{
		cout << "Node: " << n[i]->GetNodeNummer()
			<< "   Queue number: " << i
			<< "   State: " << n[i]->GetSelfStateString() << "\n";
	}

	system("pause");
}