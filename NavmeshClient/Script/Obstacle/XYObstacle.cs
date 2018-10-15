using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public interface IXYBinarySerializable {
	void Write(BinaryWriter writer);
}

/// <summary>
/// 用于定位格子的坐标
/// </summary>
public struct XYGridPos : IXYBinarySerializable {

	public void Write(BinaryWriter writer) {
		writer.Write(x);
		writer.Write(y);
	}

	public static XYGridPos Read(BinaryReader reader) {
		short x = reader.ReadInt16();
		short y = reader.ReadInt16();
		return new XYGridPos(x, y);
	}

	//public bool InRect(XYGridPos minBound, XYGridPos maxBound) {
	//    return (x_ >= minBound.x) && (x_ <= maxBound.x)
	//        && (y_ >= minBound.y) && (y_ <= maxBound.y);
	//}

	//public bool InRect(XYObstacleLine line) {
	//    return InRect(line.MinBound, line.MaxBound);
	//}

	public readonly short x;
	public readonly short y;


	public XYGridPos(short x, short y) {
		this.x = x;
		this.y = y;
	}

	private static XYGridPos zero_ = new XYGridPos(0, 0);
	public static XYGridPos Zero { get { return zero_; } }

	public override string ToString() {
		return string.Format("({0},{1})", x.ToString(), y.ToString());
	}
	public override int GetHashCode() {
		return (int) (((uint)(ushort)y << 16) | (uint)(ushort)x);
	}
	public override bool Equals(object obj) {
		if (obj == null) {
			return false;
		}
		if (object.ReferenceEquals(this, obj)) {
			return true;
		}
		if (this.GetType() != obj.GetType()) {
			return false;
		}
		XYGridPos pos = (XYGridPos)obj;
		return pos.x == x && pos.y == y;
	}
	public static bool operator ==(XYGridPos p1, XYGridPos p2) {
		return p1.x == p2.x && p1.y == p2.y;
	}
	public static bool operator !=(XYGridPos p1, XYGridPos p2) {
		return !(p1 == p2);
	}
	//public static bool operator <(XYGridPos p1, XYGridPos p2) {
	//    return (p1.x < p2.x)
	//        || ((p1.x == p2.x) && (p1.y < p2.y));
	//}
	//public static bool operator >(XYGridPos p1, XYGridPos p2) {
	//    return (p1.x > p2.x)
	//        || ((p1.x == p2.x) && (p1.y > p2.y));
	//}
	//public static bool operator >=(XYGridPos p1, XYGridPos p2) {
	//    return p1.x >= p2.x && p1.y >= p2.y;
	//}
	//public static bool operator <=(XYGridPos p1, XYGridPos p2) {
	//    return p1.x <= p2.x && p1.y <= p2.y;
	//}

	public static int RelativeCCW(XYGridPos p1, XYGridPos p2, XYGridPos p3) {
		return RelativeCCW(p1.x, p1.y, p2.x, p2.y, p3.x, p3.y);
	}

	/// <summary>
	/// 判断3个点的时针序
	/// 即判断从点1到点2到点3的走向，是顺时针还是逆时针
	/// </summary>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="x2"></param>
	/// <param name="y2"></param>
	/// <param name="x3"></param>
	/// <param name="y3"></param>
	/// <returns>
	///		0: 三点共线，且点3在点1和点2之间
	///		1: 顺时针走向。或三点共线，且点2~3的方向与点1~2一致
	///		-1: 逆时针走向。或三点共线，且点2~3的方向与点1~2相反
	/// </returns>
	private static int RelativeCCW(int x1, int y1, int x2, int y2, int x3, int y3) {
		x2 -= x1;
		y2 -= y1;
		x3 -= x1;
		y3 -= y1;
		int ccw = x3 * y2 - y3 * x2;
		if (ccw == 0) {
			ccw = x3 * x2 + y3 * y2;
			if (ccw > 0) {
				x3 -= x2;
				y3 -= y2;
				ccw = x3 * x2 + y3 * y2;
				if (ccw < 0) {
					ccw = 0;
				}
			}
		}
		return (ccw < 0) ? -1 : ((ccw > 0) ? 1 : 0);
	}
}

/// <summary>
/// GridPos的列表
/// </summary>
public class XYGridPosList : IXYBinarySerializable, IEnumerable<XYGridPos> {
	public void Write(BinaryWriter writer) {
		writer.Write((short)list_.Count);
		foreach (XYGridPos pos in list_) {
			pos.Write(writer);
		}
	}

	public static XYGridPosList Read(BinaryReader reader) {
		int count = (int)reader.ReadInt16();
		XYGridPosList result = new XYGridPosList(count);
		while (count-- > 0) {
			result.Add(XYGridPos.Read(reader));
		}
		return result;
	}

	private List<XYGridPos> list_;

	public XYGridPosList()
		: this(0) { }

	public XYGridPosList(int capacity) {
		list_ = new List<XYGridPos>(capacity);
	}

	public IEnumerator<XYGridPos> GetEnumerator() {
		foreach (XYGridPos pos in list_) {
			yield return pos;
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
	public void Add(XYGridPos pos) {
		list_.Add(pos);
	}
	public void RemoveAt(int index) {
		list_.RemoveAt(index);
	}
	public int Count { get { return list_.Count; } }
	public XYGridPos this[int index] {
		get { return list_[index]; }
		set { list_[index] = value; }
	}
	public int IndexOf(XYGridPos gridPos) {
		return list_.IndexOf(gridPos);
	}
	public void Clear() {
		list_.Clear();
	}
}

/// <summary>
/// 障碍线段
/// </summary>
public struct XYObstacleLine : IXYBinarySerializable {
	private XYGridPos begin_;
	private XYGridPos end_;
	public XYGridPos Begin { get { return begin_; } }
	public XYGridPos End { get { return end_; } }

	private int deltaY { get { return end_.y - begin_.y; } }
	private int deltaX { get { return end_.x - begin_.x; } }

	// 判断两条线的斜率是否相等
	public bool IsSlopeEqual(XYObstacleLine other) {
		return deltaY * other.deltaX == deltaX * other.deltaY;
	}

	public class EInvalidLine : System.Exception {
		public EInvalidLine(XYGridPos p1, XYGridPos p2)
			: base(string.Format("Invalid line (({0},{1}) - ({2},{3}))", p1.x, p1.y, p2.x, p2.y))
		{}
	}

	public XYObstacleLine(XYGridPos begin, XYGridPos end) {
		if (begin == end) {
			throw new EInvalidLine(begin, end);
		}
		begin_ = begin;
		end_ = end;
	}

	public override int GetHashCode() {
		return begin_.GetHashCode() ^ end_.GetHashCode();
	}

	public override bool Equals(object obj) {
		if (obj == null || obj.GetType() != this.GetType()) {
			return false;
		}
		if (object.ReferenceEquals(this, obj)) {
			return true;
		}
		XYObstacleLine other = (XYObstacleLine)obj;
		return this == other;
	}

	public static bool operator ==(XYObstacleLine a, XYObstacleLine b) {
		return ((a.Begin == b.Begin) && (a.End == b.End))
			|| ((a.Begin == b.End) && (a.End == b.Begin));
	}
	public static bool operator !=(XYObstacleLine a, XYObstacleLine b) {
		return !(a == b);
	}

	public void Write(BinaryWriter writer) {
		begin_.Write(writer);
		end_.Write(writer);
	}

	public static XYObstacleLine Read(BinaryReader reader) {
		XYGridPos p1 = XYGridPos.Read(reader);
		XYGridPos p2 = XYGridPos.Read(reader);
		return new XYObstacleLine(p1, p2);
	}

	// 判断两线是否相交
	private static bool LinesIntersect(XYGridPos p1, XYGridPos p2, XYGridPos p3, XYGridPos p4) {
		return (
			(XYGridPos.RelativeCCW(p1, p2, p3) * XYGridPos.RelativeCCW(p1, p2, p4) <= 0)
			&&
			(XYGridPos.RelativeCCW(p3, p4, p1) * XYGridPos.RelativeCCW(p3, p4, p2) <= 0)
			);
	}

	// 判断两线是否相交
	public static bool LinesIntersect(XYObstacleLine line1, XYObstacleLine line2) {
		return LinesIntersect(line1.Begin, line1.End, line2.Begin, line2.End);
	}

	// 判断一个点是否在线内
	public bool PosInLine(XYGridPos pos) {
		return XYGridPos.RelativeCCW(begin_, end_, pos) == 0;
	}
}

public class BegeinEndNum {
    public int begin_num;
    public int end_num;

    public BegeinEndNum( int b, int e ) { begin_num = b; end_num = e; }
    public void AddBegin() { begin_num++; }
    public void AddEnd() { end_num++; }

    public void RemoveBegin() { begin_num--; }
    public void RemoveEnd() { end_num--; }
}

/// <summary>
/// 障碍线段的列表
/// </summary>
public class XYObstacleLineList : IEnumerable<XYObstacleLine>, IXYBinarySerializable {

	public List<XYObstacleLine> list_;
    public Dictionary<XYGridPos, BegeinEndNum> PointList;

	public IEnumerator<XYObstacleLine> GetEnumerator() {
		for (int i = 0; i < list_.Count; ++i) {
			yield return list_[i];
		}
	}
	IEnumerator IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}

	public void Write(BinaryWriter writer) {
		writer.Write((short)list_.Count);
		foreach (XYObstacleLine line in this) {
			line.Write(writer);
		}
	}

	public static XYObstacleLineList Read(BinaryReader reader) {
		int count = (int)reader.ReadInt16();
		XYObstacleLineList result = new XYObstacleLineList(count);
		while (count-- > 0) {
			XYObstacleLine line = XYObstacleLine.Read(reader);
			result.Add(line);
		}
		return result;
	}

	public XYObstacleLineList()
		: this(0) { }

	public XYObstacleLineList(int capacity) {
		list_ = new List<XYObstacleLine>(capacity);
        PointList = new Dictionary<XYGridPos, BegeinEndNum>();
	}

	public XYObstacleLineList(XYObstacleLineList other)
		: this(other.Count)
	{
		foreach (XYObstacleLine line in other) {
			Add(line);
		}
	}

	public bool Add(XYObstacleLine line) {
		if (CanAdd(line)) {
			list_.Add(line);
            //计算奇数点
            if (PointList.ContainsKey(line.Begin))
            {
                PointList[line.Begin].AddBegin();
            }
            else
            {
                PointList.Add(line.Begin, new BegeinEndNum(1, 0));
            }
            if (PointList.ContainsKey(line.End))
            {
                PointList[line.End].AddEnd();
            }
            else
            {
                PointList.Add( line.End, new BegeinEndNum( 0, 1 ) );
            }
			return true;
		} else {
			return false;
		}
	}

	public bool RemoveLast() {
		if (list_.Count > 0) {
            //计算奇数点
            if (PointList.ContainsKey(list_[list_.Count - 1].Begin))
            {
                PointList[list_[list_.Count - 1].Begin].RemoveBegin();
            }
            if (PointList.ContainsKey(list_[list_.Count - 1].End))
            {
                PointList[list_[list_.Count - 1].End].RemoveEnd();
            }

            list_.RemoveAt(list_.Count - 1);
			return true;
		} else {
			return false;
		}
	}

	public bool RemoveWithPos(XYGridPos pos, out XYObstacleLine line) {
		int index = FindWithPos(pos);
		if (index >= 0) {
			line = list_[index];
            //计算奇数点
            if (PointList.ContainsKey(line.Begin))
            {
                PointList[line.Begin].RemoveBegin();
            }
            if (PointList.ContainsKey(line.End))
            {
                PointList[line.End].RemoveEnd();
            }
			list_.RemoveAt(index);
			return true;
		} else {
            Debug.LogWarning( "remove 失败" );
			line = new XYObstacleLine();
			return false;
		}
	}

	private int FindWithPos(XYGridPos pos) {
		for (int i = list_.Count - 1; i >= 0; --i) {
			XYObstacleLine line = list_[i];
			if (line.Begin == pos || line.End == pos) {
				return i;
			}
			if (0 == XYGridPos.RelativeCCW(line.Begin, line.End, pos)) {
				return i;
			}
		}
		// 找离该点最近的线
		float sqrDistance;
		int index = FindNearestLine(pos, out sqrDistance);
		if (index >= 0 && sqrDistance <= 1) {
			return index;
		} else {
			return -1;
		}
	}

	// 判断点是不是在线上（不包含端点）
	public bool IsPosInLineAndNotAtEndPoint(XYGridPos pos) {
		for (int i = 0; i < list_.Count; ++i) {
			XYObstacleLine line = list_[i];
			if (line.Begin != pos && line.End != pos) {
				if (0 == XYGridPos.RelativeCCW(line.Begin, line.End, pos)) {
					return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// 找出离给定点最近的线段 
	/// </summary>
	/// <param name="pos">指定点的坐标</param>
	/// <param name="sqrDistance">距离的平方</param>
	/// <returns>找到线段在列表中的索引值，或-1表示未找到</returns>
	private int FindNearestLine(XYGridPos pos, out float sqrDistance) {
		sqrDistance = float.MaxValue;
		int index = -1;
		for (int i = 0; i < list_.Count; ++i) {
			XYObstacleLine line = list_[i];
			Vector2 v = new Vector2(line.End.x - line.Begin.x, line.End.y - line.Begin.y);
			Vector2 v2 = new Vector2(pos.x - line.Begin.x, pos.y - line.Begin.y);
			float t = Vector2.Dot(v, v2) / v.sqrMagnitude;
			if (t >= 0 && t <= 1) {
				Vector2 v3 = v * t;
				v3.x += line.Begin.x;
				v3.y += line.Begin.y;
				v3.x = pos.x - v3.x;
				v3.y = pos.y - v3.y;
				float dist = v3.sqrMagnitude;
				if (dist < sqrDistance) {
					sqrDistance = dist;
					index = i;
				}
			}
		}
		return index;
	}

	public int Count { get { return list_.Count; } }
	public XYObstacleLine this[int index] {
		get { return list_[index]; }
	}

	public void Clear() {
		list_.Clear();
        PointList.Clear();
	}

	/// <summary>
	/// 判断是否可以添加
	/// 条件是：
	///		1、与已存在的线不相交
	///		2、如果相交，交点必须是线段端点
	///		3、如果相交且交点是端点，两线斜率必须不一样
	/// </summary>
	/// <param name="line"></param>
	/// <returns></returns>
	private bool CanAdd(XYObstacleLine line) {
		foreach (XYObstacleLine existsLine in this) {
			if (line == existsLine) {
				return false;
			}
			if (XYObstacleLine.LinesIntersect(existsLine, line)) {
				if (line.Begin != existsLine.Begin && line.Begin != existsLine.End
					&& line.End != existsLine.Begin && line.End != existsLine.End) {
					return false;
				}
				if (existsLine.IsSlopeEqual(line)) {
					return false;
				}
			}
		}
		return true;
	}
}

/// <summary>
/// 负责将障碍线段信息序列化
/// </summary>
public static class XYObstacleSerializer {
	public static readonly short VERSION = 1;

	public static void WriteToBinary(BinaryWriter writer, XYObstacleLineList lines, short xMapSize, short zMapSize) {
		writer.Write(VERSION);
		writer.Write(xMapSize);
		writer.Write(zMapSize);
		lines.Write(writer);
	}

	public static XYObstacleLineList ReadFromBinary(BinaryReader reader, out short xMapSize, out short zMapSize) {
		reader.ReadInt16(); // version;
		xMapSize = reader.ReadInt16();
		zMapSize = reader.ReadInt16();
		return XYObstacleLineList.Read(reader);
	}

	public static XYObstacleLineList ReadFromBinary(BinaryReader reader) {
		short x, z;
		return ReadFromBinary(reader, out x, out z);
	}

	// 写到文本
	private struct LinePointIndex {
		public int p1;
		public int p2;
	}
	public static void WriteToText(TextWriter writer, XYObstacleLineList lines) {
		List<XYGridPos> pointList = new List<XYGridPos>(lines.Count * 2);
		List<LinePointIndex> indexList = new List<LinePointIndex>(lines.Count);
		for (int i = 0; i < lines.Count; ++i) {
			XYObstacleLine line = lines[i];
			LinePointIndex lpi;
			lpi.p1 = GetPointIndex(pointList, line.Begin);
			lpi.p2 = GetPointIndex(pointList, line.End);
			indexList.Add(lpi);
		}
		//
		writer.WriteLine("{0} {1} 0", pointList.Count.ToString(), indexList.Count.ToString());
		foreach (XYGridPos pos in pointList) {
			writer.WriteLine("{0} {1} 0", pos.x.ToString(), pos.y.ToString());
		}
		foreach (LinePointIndex lpi in indexList) {
			writer.WriteLine("{0} {1}", (lpi.p1 + 1).ToString(), (lpi.p2 + 1).ToString());
		}
	}

	private static int GetPointIndex(List<XYGridPos> pointList, XYGridPos point) {
		int index = pointList.IndexOf(point);
		if (index >= 0) {
			return index;
		} else {
			pointList.Add(point);
			return pointList.Count - 1;
		}
	}

}

