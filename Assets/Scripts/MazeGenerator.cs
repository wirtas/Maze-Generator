using System;
using System.Collections.Generic;


[Flags] public enum WallState : short
    {
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        Visited = 128
    }

public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}

public static class MazeGenerator
{
    private static readonly Dictionary<WallState, WallState> GETOppositeWall = new Dictionary<WallState, WallState>
    {
        {WallState.Right, WallState.Left},
        {WallState.Left, WallState.Right},
        {WallState.Up, WallState.Down},
        {WallState.Down, WallState.Up}
    };


    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        Random rng = new Random(/*seed*/);
        var positionStack = new Stack<Position>();
        Position position = new Position
        {
            X = rng.Next(0, width),
            Y = rng.Next(0, height)
        };

        maze[position.X, position.Y] |= WallState.Visited;
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            Position current = positionStack.Pop();
            var neighbours = UnvisitedNeighbours(current, maze, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                int randIndex = rng.Next(0, neighbours.Count);
                Neighbour randomNeighbour = neighbours[randIndex];

                Position nPosition = randomNeighbour.Position;
                
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GETOppositeWall[randomNeighbour.SharedWall];
                maze[nPosition.X, nPosition.Y] |= WallState.Visited;
                
                positionStack.Push(nPosition);
            }
        }
        
        return maze;
    }
    private static List<Neighbour> UnvisitedNeighbours(Position position, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>();
        
        //---------------------- Left
        if (position.X > 0)         
        {
            if (!maze[position.X - 1, position.Y].HasFlag(WallState.Visited))
            {
                Neighbour visited = new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X - 1,
                        Y = position.Y
                    },
                    SharedWall = WallState.Left
                };
                list.Add(visited);
            }
        }
        //---------------------- Right
        if (position.X < width - 1)         
        {
            if (!maze[position.X + 1, position.Y].HasFlag(WallState.Visited))
            {
                Neighbour visited = new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X +1,
                        Y = position.Y
                    },
                    SharedWall = WallState.Right
                };
                list.Add(visited);
            }
        }
        //---------------------- Up
        if (position.Y < height - 1)         
        {
            if (!maze[position.X, position.Y + 1].HasFlag(WallState.Visited))
            {
                Neighbour visited = new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X,
                        Y = position.Y + 1
                    },
                    SharedWall = WallState.Up
                };
                list.Add(visited);
            }
        }
        //---------------------- Down
        if (position.Y > 0)         
        {
            if (!maze[position.X, position.Y - 1].HasFlag(WallState.Visited))
            {
                Neighbour visited = new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X,
                        Y = position.Y - 1
                    },
                    SharedWall = WallState.Down
                };
                list.Add(visited);
            }
        }
        return list;
    }
    public static WallState[,] Generate(int width, int height)
    {
        var maze = new WallState[width, height];
        const WallState initial = WallState.Right | WallState.Left | WallState.Up | WallState.Down;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = initial;
            }
        }

        return ApplyRecursiveBacktracker(maze,width,height);
    }
}
