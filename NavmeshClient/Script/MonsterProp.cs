using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterProp: MonoBehaviour{
	private bool selected_ = false;
    public bool selected
    {
        get { return selected_; }
        set { selected_ = value; }
    }

    private int monsterID_;
    public int monsterID
	{
        get { return monsterID_; }
        set {
            monsterID_ = value;
            textObject.GetComponent<GUIText>().text = string.Format("{0}:{1}", monsterID_, patrolType_);
        }
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

    private string patrolType_;
    public string patrolType
    {
        get { return patrolType_; }
        set { patrolType_ = value;
            textObject.GetComponent<GUIText>().text = string.Format("{0}:{1}", monsterID_, patrolType_);
        }
    }

    private Vector3 birthPoint_;
    public Vector3 birthPoint
    {
        get { return birthPoint_; }
        set { birthPoint_ = value; }
    }

    private int teamID_;
    public int teamID {
        get { return teamID_; }
        set {
            teamID_ = value;
            textObject.GetComponent<GUIText>().text = string.Format("{0}:{1}", monsterID_, patrolType_);
        }
    }

    private int followPointIdx = 0;
    public List<Vector3> followPoint_ = new List<Vector3>();
    public List<GameObject> followObj = new List<GameObject>();
    public List<int> stopTime_ = new List<int>();
    public void AddFollowPoint(Vector3 point, int stime)
    {
        RaycastHit raycast;
        if (Physics.Raycast(new Vector3(point.x, 2000, point.z), new Vector3(0, -1, 0), out raycast))
        {
            followPoint_.Add(raycast.point);
            GameObject obj = Object.Instantiate(tagObject) as GameObject;
            obj.transform.position = raycast.point;
            followObj.Add(obj);
            stopTime_.Add(stime);
        }
    }
    public void RemoveFollowPoint()
    {
        followPoint_.Clear();
        foreach (GameObject obj in followObj)
        {
            Object.Destroy(obj);
        }
        followObj.Clear();
    }

    private GameObject tagObject;
    private GameObject textObject;

    void Awake()
    {
        tagObject = transform.Find("Tag").gameObject;
        textObject = transform.Find("Text").gameObject;

        textObject.SetActive( false );
        textObject.GetComponent<GUIText>().transform.position = new Vector3(0.5f, 0.5f, 0);
    }

    void Update()
    {
        tagObject.SetActive( selected );
        textObject.SetActive( Input.GetKey( KeyCode.RightAlt ) );
        if ( teamID_ > Def.INVALID_ID && textObject.activeSelf )
            textObject.GetComponent<GUIText>().text = string.Format("{0}:{1} tID {2}", monsterID_, patrolType_, teamID_);
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        textObject.transform.position = new Vector3(screenPoint.x / Screen.width, screenPoint.y / Screen.height, 0);

        if (selected && Input.GetKey(KeyCode.LeftControl))
        {
            foreach (GameObject obj in followObj)
            {
                obj.SetActive( true );
            }
        }
        else
        {
            foreach (GameObject obj in followObj)
            {
                obj.SetActive( false );
            }
        }

        if (!selected && Input.GetKey(KeyCode.RightControl))
        {
            if (followPoint_.Count > 0)
            {
                if (Vector3.Distance(transform.position, followPoint_[followPointIdx]) < 1)
                {
                    ++followPointIdx;
                    if (followPointIdx == followPoint_.Count)
                    {
                        followPointIdx = 0;
                        transform.position = birthPoint;
                    }
                }

                Vector3 dir = followPoint_[followPointIdx] - transform.position;
                dir.y = 0;
                dir.Normalize();
                dir = dir * 5;
                CharacterController cc = gameObject.GetComponent(typeof(CharacterController)) as CharacterController;
                cc.SimpleMove(dir);
            }            
        }
    }
	
	void OnMouseDown()
	{
        selected = !selected;
        followPointIdx = 0;
        transform.position = birthPoint;
	}
}
