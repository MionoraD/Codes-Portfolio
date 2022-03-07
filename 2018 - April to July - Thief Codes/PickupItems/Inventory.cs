using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool hasPieces = false;
    public bool FilledPieces
    {
        get { return hasPieces; }
        set { hasPieces = value; }
    }

    private List<Chesspiece> pieces;
    private int pieceNr = 0;
    [Header("Red chess pieces")]
    [SerializeField] private GameObject redPawn;
    [SerializeField] private GameObject redRook;
    [SerializeField] private GameObject redKnight;
    [SerializeField] private GameObject redBishop;
    [SerializeField] private GameObject redQueen;
    [SerializeField] private GameObject redKing;

    [Header("Blue chess pieces")]
    [SerializeField] private GameObject bluePawn;
    [SerializeField] private GameObject blueRook;
    [SerializeField] private GameObject blueKnight;
    [SerializeField] private GameObject blueBishop;
    [SerializeField] private GameObject blueQueen;
    [SerializeField] private GameObject blueKing;

    private List<LevelMap> maps;
    private int mapNr = 0;

    private bool hasMaps = false;
    public bool FilledMaps
    {
        get { return hasMaps; }
        set { hasMaps = value; }
    }

    [Header("LevelMaps")]
    [SerializeField] private GameObject kingMap;
    [SerializeField] private GameObject rookMap;
    [SerializeField] private GameObject knightMap;
    [SerializeField] private GameObject bishopMap;
    [SerializeField] private GameObject queenMap;
    [SerializeField] private GameObject pawnMap;

    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Chesspiece>();

        // 16 pawns per color
        for (int i = 9; i < 17; i++)
        {
            Chesspiece pawnRed = new Chesspiece(pieceNr, "Pawn", "Red", i, redPawn);
            Addpiece(pawnRed);

            Chesspiece pawnBlue = new Chesspiece(pieceNr, "Pawn", "Blue", i + 8, bluePawn);
            Addpiece(pawnBlue);
        }

        // Add 4 rooks
        Addpiece(new Chesspiece(pieceNr, "Rook", "Red", 1, redRook));
        Addpiece(new Chesspiece(pieceNr, "Rook", "Red", 8, redRook));
        Addpiece(new Chesspiece(pieceNr, "Rook", "Blue", 25, blueRook));
        Addpiece(new Chesspiece(pieceNr, "Rook", "Blue", 32, blueRook));

        // Add 4 knights
        Addpiece(new Chesspiece(pieceNr, "Knight", "Red", 2, redKnight));
        Addpiece(new Chesspiece(pieceNr, "Knight", "Red", 7, redKnight));
        Addpiece(new Chesspiece(pieceNr, "Knight", "Blue", 26, blueKnight));
        Addpiece(new Chesspiece(pieceNr, "Knight", "Blue", 31, blueKnight));

        // Add 4 bishops
        Addpiece(new Chesspiece(pieceNr, "Bishop", "Red", 3, redBishop));
        Addpiece(new Chesspiece(pieceNr, "Bishop", "Red", 6, redBishop));
        Addpiece(new Chesspiece(pieceNr, "Bishop", "Blue", 27, blueBishop));
        Addpiece(new Chesspiece(pieceNr, "Bishop", "Blue", 31, blueBishop));

        // Add 2 queens
        Addpiece(new Chesspiece(pieceNr, "Queen", "Red", 4, redQueen));
        Addpiece(new Chesspiece(pieceNr, "Queen", "Blue", 29, blueQueen));

        // Add 2 kings
        Addpiece(new Chesspiece(pieceNr, "King", "Red", 5, redKing));
        Addpiece(new Chesspiece(pieceNr, "King", "Blue", 28, blueKing));

        FilledPieces = true;

        maps = new List<LevelMap>();

        // Add level maps
        AddMap(new LevelMap(mapNr, "King", kingMap));
        AddMap(new LevelMap(mapNr, "Rook", rookMap));
        AddMap(new LevelMap(mapNr, "Knight", knightMap));
        AddMap(new LevelMap(mapNr, "Bishop", bishopMap));
        AddMap(new LevelMap(mapNr, "Queen", queenMap));
        AddMap(new LevelMap(mapNr, "pawn", pawnMap));

        FilledMaps = true;
    }
    
    private void Addpiece(Chesspiece piece)
    {
        pieces.Add(piece);
        pieceNr++;
    }
    
    private void AddMap(LevelMap map)
    {
        maps.Add(map);
        mapNr++;
    }

    public Item FindChessPiece(int itemId)
    {
        return pieces[itemId];
    }
    
    public Item FindLevelMap(int itemId)
    {
        return maps[itemId];
    }

    public int CountColorPieces(string name, string color)
    {
        int nr = 0;
        foreach(Chesspiece item in pieces)
        {
            if (item.CheckNameColor(name, color)) nr++;
        }
        return nr;
    }

    public int CountCurrentColorPieces(string name, string color)
    {
        int nr = 0;
        foreach (Chesspiece item in pieces)
        {
            if (item.CheckHasChessPiece(name, color)) nr++;
        }
        return nr;
    }

    public bool HasFoundMap(string name)
    {
        foreach(LevelMap map in maps)
        {
            if (map.HasMap(name)) return true;
        }

        return false;
    }
}
