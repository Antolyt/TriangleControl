using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lineType
{
    horizontal,
    rising,
    falling
}

public class Line : FlexibleElement
{
    public lineType type;
    public Vector3 centerVector;
    public SpriteRenderer sr_background;
    public SpriteRenderer sr_border;
    public SpriteRenderer sr_gradiant;
    public SpriteRenderer sr_grid;
    private ParticleSystem ps;
    private ParticleSystem.MainModule particleSettings;
    public int controllingPlayer = -1;
    public TrianglePiece[] trianglePieces;
    public int id;

    public Color defaultColor;
    public Color controllerColor;

    public Vector3 center;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        sr_background.sprite = data.lineBackground;
        sr_border.sprite = data.lineBorder;
        sr_gradiant.sprite = data.lineGradiant;
        sr_grid.sprite = data.lineGrid;

        //sr_background.color = data.lineBackgroundColor;
        //sr_border.color = data.lineBorderColor;
        //sr_gradiant.color = data.lineGradiantColor;
        //sr_grid.color = data.lineGridColor;

        defaultColor = data.lineBackgroundColor;
    }

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        particleSettings = ps.main;
    }

    public void TakeControl(Player player)
    {
        sr_gradiant.color = PlayerOptions.playerConfig[player.activePlayer].color;
        controllerColor = PlayerOptions.playerConfig[player.activePlayer].color;
        particleSettings.startColor = PlayerOptions.playerConfig[player.activePlayer].color;
        ps.Play();
        controllingPlayer = player.activePlayer;
    }

    public void TakeControl(int i, Color color)
    {
        sr_gradiant.color = color;
        controllerColor = color;
        particleSettings.startColor = color;
        ps.Play();
        controllingPlayer = i;
    }

    public void Reset()
    {
        sr_gradiant.color = defaultColor;
        controllerColor = defaultColor;
        if(ps)
        {
            ps.SetParticles(null, 0);
            ps.Stop();
        }
        controllingPlayer = -1;
    }

    public float GetLength()
    {
        return sr_border.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public Vector3 GetCenter()
    {
        //switch(type)
        //{
        //    case lineType.rising:
        //         return GetLineRisingCenterVector();
        //    case lineType.horizontal:
        //        return GetLineHorizontalCenterVector();
        //    case lineType.falling:
        //        return GetLineFallingCenterVector();
        //    default:
        //        throw new UnityException("line type is not defined!");
        //}
        return Vector3.zero;
    }

    public bool IsOuterLine()
    {
        foreach(TrianglePiece triangle in trianglePieces)
        {
            if (triangle == null)
                return true;
        }
        return false;
    }

    public Vector3 GetLineRisingCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = Quaternion.Euler(0, 0, 60) * length / 2;

        return vec;
    }

    public Vector3 GetLineHorizontalCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = length / 2;

        return vec;
    }

    public Vector3 GetLineFallingCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = Quaternion.Euler(0, 0, -60) * length / 2;

        return vec;
    }

    public override bool Equals(object other)
    {
        if (typeof(Line) == other.GetType())
            return this.id == (other as Line).id;
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
