#region 脚本说明
/*----------------------------------------------------------------
// 脚本作用：实现Line拖尾效果
// 创建者：黑仔
//----------------------------------------------------------------*/
#endregion

using UnityEngine;
using System.Collections;

public class LineRebdererAni : MonoBehaviour 
{

    public int vertexCount = 5;
    public float trailLength = 5f;
    public float startPhase = 0f;

    public float Anispeed = 5f;
    public float ani_offset = 0.5f;
    public AnimationCurve ac;
    public float fallowPosSpeed = 10f;
    public float fallowRoatSpeed = 10f;


    float ani_fudu = 0.2f;
    float a;
    float a_cycle;

    LineRenderer lineR;
    Vector3[] helpers;
    Quaternion[] helpersQ;



    // Use this for initialization
    void Start()
    {
        lineR = GetComponent<LineRenderer>();
        lineR.positionCount = (vertexCount + 1);
        helpers = new Vector3[vertexCount + 1];
        helpersQ = new Quaternion[vertexCount + 1];

        for (int i = 0; i < vertexCount + 1; i++)
        {
            //  GameObject g = new GameObject();
            // helpers[i]= g.transform;
            //Destroy(g);
            if (i == 0)
            {
                helpers[i] = this.transform.position;
                helpersQ[i] = this.transform.rotation;
            }
            else
            {
                helpers[i] = helpers[i - 1] + this.transform.forward * (trailLength / (float)vertexCount);
            }
            helpersQ[i] = this.transform.rotation;
            lineR.SetPosition(i, helpers[i]);
        }



        startPhase = startPhase % 360f;
        a = startPhase;
        a_cycle = Mathf.PI * 3600f;


    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < vertexCount + 1; i++)
        {
            if (i == 0)
            {
                helpers[i] = this.transform.position;
                helpersQ[i] = this.transform.rotation;

            }
            else
            {
                Vector3 v = helpers[i - 1] + this.transform.forward * (trailLength / (float)vertexCount);
                helpers[i] = Vector3.Lerp(helpers[i], v, Time.deltaTime * 10f * fallowPosSpeed);
                helpersQ[i] = Quaternion.Slerp(helpersQ[i], helpersQ[i - 1], Time.deltaTime * 10f * fallowRoatSpeed);

            }

            a += Time.deltaTime * Anispeed / (i + 2f);
            if (a > a_cycle)
            {
                a = 0f;
            }

            ani_fudu = ac.Evaluate(i * 1f / ((float)(vertexCount - 1)));//用曲线控制飘带各节点摆动的幅度；
            Vector3 pos = new Vector3(0f, Mathf.Sin(a - i * ani_offset) * (ani_fudu / 2f), 0f);
            pos = helpersQ[i] * pos;
            helpers[i] += pos;

            lineR.SetPosition(i, helpers[i]);
        }

    }
}