using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FamilyWikGame;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar : MonoBehaviour
{
    [SerializeField] private HashSet<Node> openList;
    [SerializeField] private HashSet<Node> closedList;
    [SerializeField] private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private Node currentNode;
    [SerializeField] private Vector3Int goalPos;
    private Stack<Vector3Int> path;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap wall, interactable;
    [SerializeField] private Tile tile;
    [ContextMenu("Start Path")]
    public void Algorithm(out Stack<Vector3Int> dest, Vector3Int goal)
    {
        tilemap = GameManager._instance.walkableTileMap;
        wall = GameManager._instance.wallTileMap;
        interactable = GameManager._instance.interactableTileMap;
        goalPos = goal;
        if (currentNode == null)
            Init();

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(currentNode.position);
            
            
            Examineneighbors(neighbors, currentNode);
            
            UpdateCurrentTile(ref currentNode);
            
            path = GeneratePath(currentNode);
        }

        dest = path;
        
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        allNodes.Clear();
        currentNode = null;
        path = null;
    }
    private void Examineneighbors(List<Node> neighbors, Node current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];
            if (ConnectedDiagonally(current, neighbor))
            {
                continue;
            }
            
            int gScore = DetermineGScore(neighbors[i].position, current.position);

            if (openList.Contains(neighbor))
            {
                if (current.g + gScore < neighbor.g)
                    CalcNodes(current, neighbor, gScore);
            } 
            else if (!closedList.Contains(neighbor))
            {
                CalcNodes(current,neighbor,gScore);

                openList.Add(neighbor);
            }
        }
    }

    private void CalcNodes(Node parent, Node neighbor, int cost)
    {
        neighbor.parent = parent;

        neighbor.g = parent.g + cost;

        neighbor.h = (Math.Abs(neighbor.position.x - goalPos.x) + Math.Abs(neighbor.position.y - goalPos.y)) * 10;

        neighbor.f = neighbor.g + neighbor.h;
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;
        
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Math.Abs(x - y) % 2 == 1)
        {
            gScore = 10;
        }
        else gScore = 14;

        return gScore;
    }
    void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(p => p.f).First();
        }
    }

    private bool ConnectedDiagonally(Node current, Node neighbor)
    {
        Vector3Int direction = currentNode.position - neighbor.position;
        
        Vector3Int first = new Vector3Int(current.position.x + direction.x * -1, current.position.y, current.position.z);
        Vector3Int second = new Vector3Int(current.position.x, current.position.y + direction.y * -1, current.position.z);

        if (wall.GetTile(first) != null || wall.GetTile(second) != null || interactable.GetTile(first) != null || interactable.GetTile(second) != null)
        {
            return true;
        }
        else return false;
    }
    
    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPosition = new Vector3Int(parentPosition.x - x, parentPosition.y-y, parentPosition.z);
                if (x != 0 || y != 0)
                {
                    if (tilemap.GetTile(neighborPosition))
                        neighbors.Add(GetNode(neighborPosition));
                }
            }
        }
        return neighbors;
    }
    void Init()
    {
        currentNode = GetNode(Vector3Int.FloorToInt(transform.position));
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        openList.Add(currentNode);
    }

    Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
            return allNodes[position];
        else
        {
            Node node = new Node(position);
            allNodes.Add(position,node);
            return node;
        }
    }

    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if (current.position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();
            
            while (current.position != Vector3Int.FloorToInt(transform.position))
            {
                finalPath.Push(current.position);

                current = current.parent;
            }
            return finalPath;
        }
        return null;
    }
}
