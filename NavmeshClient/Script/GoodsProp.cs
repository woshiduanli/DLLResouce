using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GoodsProp : MonoBehaviour
{
    private bool selected_ = false;
    public bool selected
    {
        get { return selected_; }
        set { selected_ = value; }
    }

    private int goodsID_;
    public int goodsID
	{
        get { return goodsID_; }
        set { goodsID_ = value; }
	}

    private int typeID_;
    public int typeID
    {
        get { return typeID_; }
        set { typeID_ = value; }
    }

    private Color textColor_;
    public Color textColor
    {
        get { return textColor_; }
        set
        {
            textColor_ = value;
            textObject.GetComponent<GUIText>().material.color = textColor_;
        }
    }

    private int type_;
    public int type
    {
        get { return type_; }
        set { type_ = value; }
    }

    private int count_;
    public int count
    {
        get { return count_; }
        set { count_ = value; }
    }

    private string rangeType_;
    public string rangeType {
        get { return rangeType_; }
        set { rangeType_ = value; }
    }

    private string w_;
    public string w {
        get { return w_; }
        set { w_ = value; }
    }

    private string h_;
    public string h {
        get { return h_; }
        set { h_ = value; }
    }

    private string p1_;
    public string p1 {
        get { return p1_; }
        set { p1_ = value; }
    }

    private string p2_;
    public string p2 {
        get { return p2_; }
        set { p2_ = value; }
    }

    private string p3_;
    public string p3 {
        get { return p3_; }
        set { p3_ = value; }
    }

    public GameObject moduleObject;
    private GameObject tagObject;
    private GameObject textObject;

    void Awake()
    {
        moduleObject = transform.gameObject;
        tagObject = transform.Find("Tag").gameObject;
        textObject = transform.Find("Text").gameObject;

        textObject.SetActive( false );
        textObject.GetComponent<GUIText>().transform.position = new Vector3(0.5f, 0.5f, 0);
    }

    void Start() {
        StringBuilder sb = new StringBuilder();
        if (p1 != "")
        {
            sb.Append(" p1=");
            sb.Append(p1);
        }
        if (p2 != "")
        {
            sb.Append(" p2=");
            sb.Append(p2);
        }
        if (p3 != "")
        {
            sb.Append(" p3=");
            sb.Append(p3);
        }
        if (rangeType_ == "circle")
            textObject.GetComponent<GUIText>().text = string.Format("{0} r={1}{2}", typeID_, w_, sb.ToString());
        else
            textObject.GetComponent<GUIText>().text = string.Format("{0} {1}*{2}{3}", typeID_, w_, h_, sb.ToString());
    }

    void Update() {
        tagObject.SetActive( selected );
        textObject.SetActive( Input.GetKey( KeyCode.RightAlt ) );

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        textObject.transform.position = new Vector3(screenPoint.x / Screen.width, screenPoint.y / Screen.height, 0);
    }

	void OnMouseDown()
	{
        selected = !selected;
	}
}
