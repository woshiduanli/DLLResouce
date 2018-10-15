using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 设计时刻，在指定GameObject周围显示网格
// 本脚本必须绑在主摄像机上
[ExecuteInEditMode()]
public class XYGridCamera : MonoBehaviour {

	private GameObject target_;
	public GameObject Target {
		get { return target_; }
		set {
			RefreshGizmos();
			target_ = value;
			RefreshGizmos();
		}
	}

	private Bounds targetBounds_;	// 目标的Bounds
	public Bounds TargetBounds {
		get { return targetBounds_; }
	}
	

	public bool ShowBounds = true;
	public bool ShowGrid = true;
	public int GridSize = 24;
	public Color GridColor = new Color(0.5f, 0, 0.5f, 1);
	public Color BoundsColor = new Color(0, 1, 1, 1);


	void Awake() {
        if (Application.isPlaying)
        {
            enabled = false;
        }

	}

	/// <summary>
	/// 暂时找了这么一个方法，来使Gizmos被更新
	/// </summary>
	private void RefreshGizmos() {
		if (target_) {
			target_.SetActive( !target_.activeSelf );
            target_.SetActive( !target_.activeSelf );
		}
	}

	private bool GetTargetWorldBounds() {
		if (target_) {
			return GetGameObjectWorldBounds(target_, out targetBounds_);
		} else {
			return false;
		}
	}

	private static readonly Bounds EMPTY_BOUNDS = new Bounds();
	public static bool GetGameObjectWorldBounds(GameObject go, out Bounds bounds) {
		if (go) {
			Collider c = go.GetComponent<Collider>();
			if (c) {
				bounds = c.bounds;
				return true;
			}
			Renderer r = go.GetComponent<Renderer>();
			if (r) {
				bounds = r.bounds;
				return true;
			}
		}
		bounds = EMPTY_BOUNDS;
		return false;
	}

	void OnDrawGizmos() {
		GridSize = Mathf.Clamp(GridSize, 2, 24);
		if (!target_) { return; }
		if (!ShowBounds && !ShowGrid) { return; }
		bool hasBounds = GetTargetWorldBounds();
		// 绘制Grid
		if (ShowGrid) {
			// 将位置对齐到网格交叉点
			Vector3 pos = target_.transform.position;
			pos.x = Mathf.Floor(pos.x * 2) * 0.5f;	
			pos.z = Mathf.Floor(pos.z * 2) * 0.5f;
            if (hasBounds)
            {
                pos.y = this.targetBounds_.min.y;
            }
            else
            {
                pos.y = XYObstacleCamera.GetTerrainHeight(pos);
            }
			int halfGridCount = GridSize / 2;
			Vector3 p1 = pos;
			p1.x -= halfGridCount;
			p1.z -= halfGridCount;
			Vector3 p2 = pos;
			p2.x += halfGridCount;
			p2.z += halfGridCount;
			Gizmos.color = GridColor;
			for (float x = p1.x; x <= p2.x; x += 0.5f) {
                Gizmos.DrawLine(new Vector3(x, pos.y, p1.z), new Vector3(x, pos.y, p2.z));
			}
			for (float z = p1.z; z <= p2.z; z += 0.5f) {
                Gizmos.DrawLine(new Vector3(p1.x, pos.y, z), new Vector3(p2.x, pos.y, z));
			}
		}
		// 绘制Bounds
		if (hasBounds && ShowBounds) {
			Gizmos.color = BoundsColor;
			Gizmos.DrawWireCube(targetBounds_.center, targetBounds_.size);
		}

	}

}
