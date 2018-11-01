using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    right,
    left,
    down
}

public class TrianglePiece : MonoBehaviour {

    public int id;
    public TriangleTyp type;
    public Triangle triangle;
    public Line[] lines; // right, lower, left if facing downwards // upper, right, left if facing upwards
    public int points = 1;
    public int controllingPlayer;
    public Functionality functionality = Functionality.normal;
    public Direction conquerNextDirection = Direction.normal;

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
                controllingPlayer = 1;
                break;
            case Functionality.bomb:
                //ActivateBomb(player);
                break;
            case Functionality.conquerNext:
                //triangle.sr_gradiant_hole.color = PlayerOptions.playerConfig[player.activePlayer].color;
                //player.players[player.activePlayer].score.text = (int.Parse(player.players[player.activePlayer].score.text) + points).ToString();
                //ConquerNext(player);
                break;
        }
    }

    public void Reset()
    {
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
