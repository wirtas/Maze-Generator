using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] [Range(5,100)] 
    private int width = 10;
    
    [SerializeField] [Range(5,100)] 
    private int height = 10;
    
    [SerializeField] [Range(0.5f,5)] 
    private float size = 1;
    
    [SerializeField] 
    private Transform wallPrefab;

    [SerializeField] private Transform bg;

    private void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
        SetBackground();
    }

    private void Draw(WallState[,] maze)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                WallState cell = maze[i, j];

                Vector2 position = new Vector2(-width / 2 + i + 0.5f,  -width / 2 + j + 0.5f)*size;
                
                if (cell.HasFlag(WallState.Up))
                {
                    Transform upWall = Instantiate(wallPrefab, transform);
                    upWall.position = position + new Vector2(0, size / 2);
                    Vector2 upWallLocalScale = upWall.localScale;
                    upWallLocalScale = new Vector2(size, upWallLocalScale.y);
                    upWall.localScale = upWallLocalScale;
                }

                if (cell.HasFlag(WallState.Left))
                {
                    Transform leftWall = Instantiate(wallPrefab, transform);
                    leftWall.position = position + new Vector2(-size / 2, 0);
                    Vector2 leftWallLocalScale = leftWall.localScale;
                    leftWall.eulerAngles = new Vector3(0, 0, 90);
                    leftWallLocalScale = new Vector2(size, leftWallLocalScale.y);
                    leftWall.localScale = leftWallLocalScale;
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.Right))
                    {
                        Transform rightWall = Instantiate(wallPrefab, transform);
                        rightWall.position = position + new Vector2(size / 2, 0);
                        Vector2 rightWallLocalScale = rightWall.localScale;
                        rightWall.eulerAngles = new Vector3(0, 0, 90);
                        rightWallLocalScale = new Vector2(size, rightWallLocalScale.y);
                        rightWall.localScale = rightWallLocalScale;
                    } 
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.Down))
                    {
                        Transform downWall = Instantiate(wallPrefab, transform);
                        downWall.position = position + new Vector2(0, -size / 2);
                        Vector2 downWallLocalScale = downWall.localScale;
                        downWallLocalScale = new Vector2(size, downWallLocalScale.y);
                        downWall.localScale = downWallLocalScale;
                    }
                }
            }
        }
    }

    private void SetBackground()
    {
        bg.localScale = new Vector3(width, height, bg.localScale.z)*size;
        bg.gameObject.SetActive(true);
    }
}
