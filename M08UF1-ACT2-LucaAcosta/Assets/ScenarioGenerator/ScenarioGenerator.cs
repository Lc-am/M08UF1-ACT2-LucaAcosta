using UnityEngine;

public class ScenarioGenerator : MonoBehaviour
{
    static public ScenarioGenerator instance;
    [SerializeField] int numPiecesToGenerateOnStart = 4;
    [SerializeField] GameObject[] piecesPrefabs;

    [Header("Debug")]
    [SerializeField] bool debugEndOfPieceReached;

    Transform nextPiecePosition;

    int numPiecesFinished = 0;

    private void OnValidate()
    {
        if(debugEndOfPieceReached)
        {
            debugEndOfPieceReached = false;
            EndOfPieceReached();
        }
    }

    private void Awake()
    {
        instance = this;
        nextPiecePosition = transform;
    }
    private void Start()
    {
        for (int i = 0; i < numPiecesToGenerateOnStart; i++)
        {
            AddNewPiece();
        }
    }

    public void EndOfPieceReached()
    {
        numPiecesFinished++;
        if (numPiecesFinished > 1)
        {
            DestroyOldestPiece();
            AddNewPiece();
        }
    }

    void AddNewPiece()
    {
        GameObject piecesToInstantiate = piecesPrefabs[Random.Range(0, piecesPrefabs.Length)];
        GameObject newPiece = Instantiate(piecesToInstantiate, nextPiecePosition.position, nextPiecePosition.rotation, transform);
        nextPiecePosition = newPiece.GetComponentInChildren<NextPiecePosition>().transform;
    }

    void DestroyOldestPiece()
    {
        GameObject oldestPiece = transform.GetChild(0).gameObject;
        Destroy(oldestPiece);
    }
}


