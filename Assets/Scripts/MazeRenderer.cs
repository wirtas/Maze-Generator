using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] [Range(5,30)] 
    private int width = 10;
    
    [SerializeField] [Range(5,30)] 
    private int height = 10;
    
    [SerializeField] [Range(0.5f,3)] 
    private float size = 1;
    
    [SerializeField] 
    private Transform wallPrefab, startPrefab, endPrefab, background, bgParent;

    private void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    private void Draw(WallState[,] maze)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                WallState cell = maze[i, j];

                Vector3 position = new Vector3(-width / 2 + i, -width / 2 + j) * size;

                if (cell.HasFlag(WallState.Up))
                {
                    Transform upWall = Instantiate(wallPrefab, transform);
                    upWall.position = position + new Vector3(0, size / 2);
                    Vector3 upWallLocalScale = upWall.localScale;
                    upWallLocalScale = new Vector3(size, upWallLocalScale.y);
                    upWall.localScale = upWallLocalScale;
                }

                if (cell.HasFlag(WallState.Left))
                {
                    Transform leftWall = Instantiate(wallPrefab, transform);
                    leftWall.position = position + new Vector3(-size / 2, 0);
                    Vector3 leftWallLocalScale = leftWall.localScale;
                    leftWall.eulerAngles = new Vector3(0, 0, 90);
                    leftWallLocalScale = new Vector3(size, leftWallLocalScale.y);
                    leftWall.localScale = leftWallLocalScale;
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.Right))
                    {
                        Transform rightWall = Instantiate(wallPrefab, transform);
                        rightWall.position = position + new Vector3(size / 2, 0);
                        Vector3 rightWallLocalScale = rightWall.localScale;
                        rightWall.eulerAngles = new Vector3(0, 0, 90);
                        rightWallLocalScale = new Vector3(size, rightWallLocalScale.y);
                        rightWall.localScale = rightWallLocalScale;
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.Down))
                    {
                        Transform downWall = Instantiate(wallPrefab, transform);
                        downWall.position = position + new Vector3(0, -size / 2);
                        Vector3 downWallLocalScale = downWall.localScale;
                        downWallLocalScale = new Vector3(size, downWallLocalScale.y);
                        downWall.localScale = downWallLocalScale;
                    }
                }

                if (i == 0 && j == 0)
                {
                    Transform startPosition = Instantiate(startPrefab, bgParent);
                    startPosition.position = position+ new Vector3(0,0,1);
                    Vector3 startPositionLocalScale = startPosition.localScale;
                    startPositionLocalScale = new Vector3(size, startPositionLocalScale.y);
                    startPosition.localScale = startPositionLocalScale;
                }
                else if (i == width - 1 && j == height - 1)
                {
                    Transform endPosition = Instantiate(endPrefab, bgParent);
                    endPosition.position = position+ new Vector3(0,0,1);
                    Vector3 endPositionLocalScale = endPosition.localScale;
                    endPositionLocalScale = new Vector3(size, endPositionLocalScale.y);
                    endPosition.localScale = endPositionLocalScale;
                }
                else
                {
                    Transform bg = Instantiate(background, bgParent);
                    bg.position = position + new Vector3(0,0,1);
                    Vector3 bgLocalScale = bg.localScale;
                    bgLocalScale = new Vector3(size, bgLocalScale.y);
                    bg.localScale = bgLocalScale;
                }
            }
        }
    }
}
