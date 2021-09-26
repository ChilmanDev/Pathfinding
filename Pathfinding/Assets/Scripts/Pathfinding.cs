using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
	PathRequestManager requestManager;
	Grid grid;

	void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
		StartCoroutine(FindPath(startPos, targetPos));
    }

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];
		bool pathSucess = false;
		//Nodos referentes a posição de mundo do player e to alvo
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		if (startNode.walkable && targetNode.walkable)
        {
			//Conjuntos de nodos abertos e fechados
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();

			//Inserir o nodo referente a posição do player no conjunto de nodos abertos
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				//Primeiro nodo dos abertos
				//Anda pelo conjunto de abertos para escolher o com menor custo F(soma da dist do nodo ao player com a dist do nodo ao alvo)
				Node currentNode = openSet.RemoveFirst();


				closedSet.Add(currentNode);

				//Se o nodo for o mesmo que o do alvo a busca se encerra e o caminho é encontrado
				if (currentNode == targetNode)
				{
					pathSucess = true;
					break;
				}

				//Verificar e atualizar os nodos vizinhos
				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					//Caso o nodo vizinho não seja caminhável ou já esteja no conjunto fechado a verificação passa para o próximo vizinho
					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					//Calcula o novo custo G (distância do nodo inicial) para alcançar o nodo vizinho (desde o nodo inicial até o vizinho)
					//O custo H (distância do nodo final) nunca vai ser alterado, então não é verificado
					int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

					/*
					Caso o novo custo G do vizinho seja inferior ao registrado anteriormente
					OU o nodo vizinho não esteja no conjunto aberto (não possui custo registrado ainda)
					então é registrado o custo novo e o nodo atual é registrado como pai do nodo vizinho (formando o caminho até ele)
					*/
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						//Se o nodo vizinho não estava no conjunto aberto, adicioná-lo a ele
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}

		yield return null;
        if (pathSucess)
        {
			waypoints = RetracePath(startNode, targetNode);
		}
		requestManager.FinishProcessingPath(waypoints, pathSucess);
	}

	//Refaz o caminho de traz pra frente, inverte (para ficar na ordem certa) e envia e registra ele pro grid
	Vector3[] RetracePath(Node _startNode, Node _endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = _endNode;

		while (currentNode != _startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;

	}

	//Guarda apenas os nodos das "curvas" (quando muda a direção)
	Vector3[] SimplifyPath(List<Node> _path)
    {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector3.zero;

		for(int i = 1; i < _path.Count; i++)
        {
			Vector2 directionNew = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
			//if (directionNew != directionOld) - DESATIVEI A FUNÇÃO, não estava funcionando bem nesse ambiente
            {
				waypoints.Add(_path[i].worldPosition);
            }
			directionOld = directionNew;
        }
		return waypoints.ToArray();
    }

	//Retorna a distancia entre 2 nodos
	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}