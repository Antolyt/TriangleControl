using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TriangleTyp{
    up,
    down
}

public enum Functionality
{
    normal,
    bomb,
    conquerNext
}

public enum Direction
{
    normal,
    up,
    upRight,
    right,
    downRight,
    down,
    downLeft,
    left,
    upLeft
}

public class TrianglePiece : MonoBehaviour {

    public int id;
    public TriangleTyp type;
    public Triangle triangle;
    public Line[] lines; // right, lower, left if facing downwards // upper, right, left if facing upwards
    [Range(-3,3)]
    public int points = 1;
    public int controllingPlayer;
    public Functionality functionality = Functionality.normal;
    public Direction conquerNextDirection = Direction.normal;
    public Image functionalityImage;

    public Vector3 center;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetLength()
    {
        return GetComponent<BoxCollider2D>().size.x;
    }

    public float GetHeight()
    {
        return GetComponent<BoxCollider2D>().size.y;
    }

    public void TakeControl(Player player)
    {
        controllingPlayer = player.activePlayer;
        switch (functionality)
        {
            case Functionality.normal:
                triangle.sr_gradiant_hole.color = PlayerOptions.playerConfig[player.activePlayer].color;
                player.players[player.activePlayer].score.text = (int.Parse(player.players[player.activePlayer].score.text) + points).ToString();
                break;
            case Functionality.bomb:
                ActivateBomb(player);
                break;
            case Functionality.conquerNext:
                triangle.sr_gradiant_hole.color = PlayerOptions.playerConfig[player.activePlayer].color;
                player.players[player.activePlayer].score.text = (int.Parse(player.players[player.activePlayer].score.text) + points).ToString();
                ConquerNext(player);
                break;
        }
    }

    public void TakeControl(int i, Color color)
    {
        switch (functionality)
        {
            case Functionality.normal:
                triangle.sr_gradiant_hole.color = color;
                controllingPlayer = i;
                break;
            case Functionality.bomb:
                ActivateBomb(i);
                break;
            case Functionality.conquerNext:
                triangle.sr_gradiant_hole.color = color;
                ConquerNext(i, color);
                break;
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        triangle.sr_gradiant_hole.color = Color.white;
        controllingPlayer = -1;
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactives current and connected Triangles, reduces controllers points by point value
    /// </summary>
    /// <param name="player">Player Object</param>
    public void ActivateBomb(Player player)
    {
        foreach(Line l in lines)
        {
            foreach(TrianglePiece t in l.trianglePieces)
            {
                if(t.gameObject.activeSelf)
                {
                    if (t.controllingPlayer > 0)
                    {
                        player.players[player.activePlayer].score.text = (int.Parse(player.players[player.activePlayer].score.text) - points).ToString();
                    }
                    t.gameObject.SetActive(false);
                }
            }
            l.gameObject.SetActive(false);
        }
    }

    public void ActivateBomb(int i)
    {
        List<Line> checkedLines = new List<Line>();
        foreach (Line l in lines)
        {
            checkedLines.Add(l);
            foreach (TrianglePiece t in l.trianglePieces)
            {
                if(t)
                {
                    if (t.gameObject.activeSelf)
                    {
                        t.gameObject.SetActive(false);
                    }

                    foreach (Line l2 in t.lines)
                    {
                        if (!checkedLines.Contains(l2))
                            checkedLines.Add(l2);
                    }
                }
            }
        }
        foreach (Line l in checkedLines)
        {
            bool allTrianglePiecesDeactive = true;
            foreach (TrianglePiece t in l.trianglePieces)
            {
                if (!t)
                    continue;

                if (t.gameObject.activeSelf)
                {
                    allTrianglePiecesDeactive = false;
                    break;
                }
            }
            if (allTrianglePiecesDeactive)
                l.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Takes control of the next Triangle in direction for player
    /// </summary>
    /// <param name="player">Player Object</param>
    /// <param name="direction">direction of the Triangle to Conquer</param>
    public void ConquerNext(Player player)
    {
        switch (type)
        {
            case TriangleTyp.down:
                switch (conquerNextDirection)
                {
                    case Direction.up:
                        foreach (TrianglePiece t in lines[0].trianglePieces)
                        {
                            if(t != this)
                            {
                                t.TakeControl(player);
                                t.TakeControlOfLines(player);
                            }
                        } 
                        break;
                    case Direction.down:
                        throw new UnityException("conquerNextDirection not correct!");
                    case Direction.left:
                        foreach (TrianglePiece t in lines[2].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(player);
                                t.TakeControlOfLines(player);
                            }
                        }
                        break;
                    case Direction.right:
                        foreach (TrianglePiece t in lines[1].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(player);
                                t.TakeControlOfLines(player);
                            }
                        }
                        break;
                    default:
                        throw new UnityException("conquerNextDirection not correct!");
                }
                break;
            case TriangleTyp.up:
                switch (conquerNextDirection)
                {
                    case Direction.up:
                        throw new UnityException("conquerNextDirection not correct!");
                    case Direction.down:
                        foreach (TrianglePiece t in lines[1].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(player);
                                t.TakeControlOfLines(player);
                            }
                        }
                        break;
                    case Direction.left:
                        foreach (TrianglePiece t in lines[2].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(player);
                                t.TakeControlOfLines(player);
                            }
                        }
                        break;
                    case Direction.right:
                        foreach (TrianglePiece t in lines[0].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(player);
                                t.TakeControlOfLines(player);
                            }
                        }
                        break;
                    default:
                        throw new UnityException("conquerNextDirection not correct!");
                }
                break;
        }
    }

    public void ConquerNext(int i, Color color)
    {
        switch (type)
        {
            case TriangleTyp.down:
                switch (conquerNextDirection)
                {
                    case Direction.up:
                        foreach (TrianglePiece t in lines[0].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(i, color);
                                t.TakeControlOfLines(i, color);
                            }
                        }
                        break;
                    case Direction.down:
                        throw new UnityException("conquerNextDirection not correct!");
                    case Direction.downLeft:
                        foreach (TrianglePiece t in lines[2].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(i, color);
                                t.TakeControlOfLines(i, color);
                            }
                        }
                        break;
                    case Direction.downRight:
                        foreach (TrianglePiece t in lines[1].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(i, color);
                                t.TakeControlOfLines(i, color);
                            }
                        }
                        break;
                    default:
                        throw new UnityException("conquerNextDirection not correct!");
                }
                break;
            case TriangleTyp.up:
                switch (conquerNextDirection)
                {
                    case Direction.up:
                        throw new UnityException("conquerNextDirection not correct!");
                    case Direction.down:
                        foreach (TrianglePiece t in lines[1].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(i, color);
                                t.TakeControlOfLines(i, color);
                            }
                        }
                        break;
                    case Direction.upLeft:
                        foreach (TrianglePiece t in lines[2].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(i, color);
                                t.TakeControlOfLines(i, color);
                            }
                        }
                        break;
                    case Direction.upRight:
                        foreach (TrianglePiece t in lines[0].trianglePieces)
                        {
                            if (t != this)
                            {
                                t.TakeControl(i, color);
                                t.TakeControlOfLines(i, color);
                            }
                        }
                        break;
                    default:
                        throw new UnityException("conquerNextDirection not correct!");
                }
                break;
        }
    }

    public void TakeControlOfLines(Player player)
    {
        foreach(Line l in lines)
        {
            if(l.controllingPlayer != player.activePlayer)
            {
                l.TakeControl(player);
            }
        }
    }

    public void TakeControlOfLines(int i, Color color)
    {
        foreach (Line l in lines)
        {
            if (l.controllingPlayer != i)
            {
                l.TakeControl(i, color);
            }
        }
    }

    public override bool Equals(object other)
    {
        if (typeof(TrianglePiece) == other.GetType())
            return this.id == (other as TrianglePiece).id;
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
