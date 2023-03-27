﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

public class Generator2D : MonoBehaviour
{
    private enum CellType
    {
        None,
        Room,
        Hallway
    }

    private class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !(a.bounds.position.x >= b.bounds.position.x + b.bounds.size.x ||
                     a.bounds.position.x + a.bounds.size.x <= b.bounds.position.x
                     || a.bounds.position.y >= b.bounds.position.y + b.bounds.size.y ||
                     a.bounds.position.y + a.bounds.size.y <= b.bounds.position.y);
        }
    }

    [SerializeField] private Vector2Int size;
    [SerializeField] private int roomCount;
    [SerializeField] private Vector2Int roomMaxSize;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;

    private Random random;
    private Grid2D<CellType> grid;
    private List<Room> rooms;
    private Delaunay2D delaunay;
    private HashSet<Prim.Edge> selectedEdges;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        random = new Random(0);
        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    private void PlaceRooms()
    {
        for (var i = 0; i < roomCount; i++)
        {
            var location = new Vector2Int(
                random.Next(0, size.x),
                random.Next(0, size.y)
            );

            var roomSize = new Vector2Int(
                random.Next(1, roomMaxSize.x + 1),
                random.Next(1, roomMaxSize.y + 1)
            );

            var add = true;
            var newRoom = new Room(location, roomSize);
            var buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms)
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                                        || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
                add = false;

            if (add)
            {
                rooms.Add(newRoom);
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

                foreach (var pos in newRoom.bounds.allPositionsWithin) grid[pos] = CellType.Room;
            }
        }
    }

    private void Triangulate()
    {
        var vertices = new List<Vertex>();

        foreach (var room in rooms)
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + (Vector2)room.bounds.size / 2, room));

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    private void CreateHallways()
    {
        var edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) edges.Add(new Prim.Edge(edge.U, edge.V));

        var mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
            if (random.NextDouble() < 0.125)
                selectedEdges.Add(edge);
    }

    private void PathfindHallways()
    {
        var aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) =>
            {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos); //heuristic

                if (grid[b.Position] == CellType.Room)
                    pathCost.cost += 10;
                else if (grid[b.Position] == CellType.None)
                    pathCost.cost += 5;
                else if (grid[b.Position] == CellType.Hallway) pathCost.cost += 1;

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null)
            {
                for (var i = 0; i < path.Count; i++)
                {
                    var current = path[i];

                    if (grid[current] == CellType.None) grid[current] = CellType.Hallway;

                    if (i > 0)
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }

                foreach (var pos in path)
                    if (grid[pos] == CellType.Hallway)
                        PlaceHallway(pos);
            }
        }
    }

    private void PlaceCube(Vector2Int location, Vector2Int size, Material material)
    {
        var go = Instantiate(cubePrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
        go.GetComponent<Transform>().localScale = new Vector3(size.x, 1, size.y);
        go.GetComponent<MeshRenderer>().material = material;
    }

    private void PlaceRoom(Vector2Int location, Vector2Int size)
    {
        PlaceCube(location, size, redMaterial);
    }

    private void PlaceHallway(Vector2Int location)
    {
        PlaceCube(location, new Vector2Int(1, 1), blueMaterial);
    }
}