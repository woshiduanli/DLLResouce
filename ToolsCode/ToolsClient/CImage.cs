using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class CImage : Image
{
    public string SpriteName;
    public string AtlasName;
}

public class CRawImage : RawImage
{
    public string RawImageName;
    public bool IsLoad;
}


public class CText : Text, IPointerClickHandler
{
    public string FontPath;
    public string FontName;
    public string Language;
    [HideInInspector]
    public int colorIdx;
    [HideInInspector]
    public Outline outlineEff;
    [HideInInspector]
    public Shadow shadowEff;
    public bool IsNeedEmoji;
    public bool UseHint;

    private const bool EMOJI_LARGE = true;
    private static Dictionary<string, EmojiInfo> EmojiIndex = null;
    private List<EmojiInfoMatch> EmojiInfoList = null;
    private List<HrefInfoMatch> HrefInfoList = null;

    struct EmojiInfo
    {
        public float x;
        public float y;
        public float size;
        public int len;
    }

    struct EmojiInfoMatch
    {
        public string Value;
        public int Length;
        public int Index;
    }

    struct HrefInfoMatch
    {
        public int Index;
        public string HrefIdx;
        public string Value;
        public int Length;
        public int Strlen;
    }

    private string m_OutputText;

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();

#if UNITY_EDITOR
        if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
        {
            return;
        }
#endif
        m_OutputText = GetOutputText(text);
    }

    protected bool isSingleNum(char c)
    {
        return c >= '0' && c <= '9';
    }

    private List<EmojiInfoMatch> MatchEmojiInfo(string OutputText)
    {
        if (EmojiInfoList == null)
            EmojiInfoList = new List<EmojiInfoMatch>();
        else
            EmojiInfoList.Clear();

        int length = OutputText.Length - 2;
        for (int i = 0; i < length; i++)
        {
            if (OutputText[i] == '[')
            {
                if (isSingleNum(OutputText[i + 1]))
                {
                    if (isSingleNum(OutputText[i + 2]) && i + 1 < length && OutputText[i + 3] == ']')
                    {
                        EmojiInfoMatch infoMatch = new EmojiInfoMatch();
                        infoMatch.Value = OutputText.Substring(i, 4);
                        infoMatch.Length = 4;
                        infoMatch.Index = i;
                        EmojiInfoList.Add(infoMatch);
                        i = i + 3;
                    }
                    else if (OutputText[i + 2] == ']')
                    {
                        EmojiInfoMatch infoMatch = new EmojiInfoMatch();
                        infoMatch.Value = OutputText.Substring(i, 3);
                        infoMatch.Length = 3;
                        infoMatch.Index = i;
                        EmojiInfoList.Add(infoMatch);
                        i = i + 2;
                    }
                }
            }
        }

        return EmojiInfoList;
    }

    readonly UIVertex[] m_TempVerts = new UIVertex[4];
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (font == null)
            return;

        if (EmojiIndex == null)
        {
            EmojiIndex = new Dictionary<string, EmojiInfo>();

            //load emoji data, and you can overwrite this segment code base on your project.
            TextAsset emojiContent = Resources.Load<TextAsset>("UI/Login/Emoji/emoji");
            if (!emojiContent)
            {
                base.OnPopulateMesh(toFill);
                return;
            }
            string[] lines = emojiContent.text.Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    string[] strs = lines[i].Split('\t');
                    EmojiInfo info;
                    info.x = float.Parse(strs[3]);
                    info.y = float.Parse(strs[4]);
                    info.size = float.Parse(strs[5]);
                    info.len = 0;
                    EmojiIndex.Add(strs[1], info);
                }
            }
        }

        Dictionary<int, EmojiInfo> emojiDic = new Dictionary<int, EmojiInfo>();
        if (supportRichText)
        {
            //MatchCollection matches = Regex.Matches(m_OutputText, "\\[[a-z0-9A-Z]+\\]");//把表情标签全部匹配出来
            List<EmojiInfoMatch> matches = MatchEmojiInfo(m_OutputText);
            for (int i = 0; i < matches.Count; i++)
            {
                EmojiInfo info;
                if (EmojiIndex.TryGetValue(matches[i].Value, out info))
                {
                    info.len = matches[i].Length;
                    emojiDic.Add(matches[i].Index, info);
                }
            }
        }

        // We don't care if we the font Texture changes while we are doing our Update.
        // The end result of cachedTextGenerator will be valid for this instance.
        // Otherwise we can get issues like Case 619238.
        m_DisableFontTextureRebuiltCallback = true;

        Vector2 extents = rectTransform.rect.size;

        var settings = GetGenerationSettings(extents);
        var orignText = m_Text;
        m_Text = m_OutputText;
        cachedTextGenerator.Populate(m_Text, settings);//重置网格
        m_Text = orignText;

        Rect inputRect = rectTransform.rect;

        // get the text alignment anchor point for the text in local space
        Vector2 textAnchorPivot = GetTextAnchorPivot(alignment);
        Vector2 refPoint = Vector2.zero;
        refPoint.x = Mathf.Lerp(inputRect.xMin, inputRect.xMax, textAnchorPivot.x);
        refPoint.y = Mathf.Lerp(inputRect.yMin, inputRect.yMax, textAnchorPivot.y);

        // Determine fraction of pixel to offset text mesh.
        Vector2 roundingOffset = PixelAdjustPoint(refPoint) - refPoint;

        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        //Last 4 verts are always a new line...
        int vertCount = verts.Count - 4;

        toFill.Clear();
        if (roundingOffset != Vector2.zero)
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        }
        else
        {
            float repairDistance = 0;
            float repairDistanceHalf = 0;
            float repairY = 0;
            if (vertCount > 0)
            {
                repairY = verts[3].position.y;
            }
            for (int i = 0; i < vertCount; ++i)
            {
                EmojiInfo info;
                int index = i / 4;//每个字符4个顶点
                if (emojiDic.TryGetValue(index, out info))
                {//这个顶点位置是否为表情开始的index

                    HrefInfosIndexAdjust(i);//矫正一下超链接的Index

                    //compute the distance of '[' and get the distance of emoji 
                    //计算表情标签2个顶点之间的距离， * 3 得出宽度（表情有3位）
                    float charDis = (verts[i + 1].position.x - verts[i].position.x) * 3;
                    m_TempVerts[3] = verts[i];//1
                    m_TempVerts[2] = verts[i + 1];//2
                    m_TempVerts[1] = verts[i + 2];//3
                    m_TempVerts[0] = verts[i + 3];//4

                    //the real distance of an emoji
                    m_TempVerts[2].position += new Vector3(charDis, 0, 0);
                    m_TempVerts[1].position += new Vector3(charDis, 0, 0);

                    float fixWidth = m_TempVerts[2].position.x - m_TempVerts[3].position.x;
                    float fixHeight = (m_TempVerts[2].position.y - m_TempVerts[1].position.y);
                    //make emoji has equal width and height
                    float fixValue = (fixWidth - fixHeight);//把宽度变得跟高度一样
                    m_TempVerts[2].position -= new Vector3(fixValue, 0, 0);
                    m_TempVerts[1].position -= new Vector3(fixValue, 0, 0);

                    float curRepairDis = 0;
                    if (verts[i].position.y < repairY)
                    {// to judge current char in the same line or not
                        repairDistance = repairDistanceHalf;
                        repairDistanceHalf = 0;
                        repairY = verts[i + 3].position.y;
                    }
                    curRepairDis = repairDistance;
                    int dot = 0;//repair next line distance
                    for (int j = info.len - 1; j > 0; j--)
                    {
                        int infoIndex = i + j * 4 + 3;
                        if (verts.Count > infoIndex && verts[infoIndex].position.y >= verts[i + 3].position.y)
                        {
                            repairDistance += verts[i + j * 4 + 1].position.x - m_TempVerts[2].position.x;
                            break;
                        }
                        else
                        {
                            dot = i + 4 * j;

                        }
                    }
                    if (dot > 0)
                    {
                        int nextChar = i + info.len * 4;
                        if (nextChar < verts.Count)
                        {
                            repairDistanceHalf = verts[nextChar].position.x - verts[dot].position.x;
                        }
                    }

                    //repair its distance
                    for (int j = 0; j < 4; j++)
                    {
                        m_TempVerts[j].position -= new Vector3(curRepairDis, 0, 0);
                    }

                    m_TempVerts[0].position *= unitsPerPixel;
                    m_TempVerts[1].position *= unitsPerPixel;
                    m_TempVerts[2].position *= unitsPerPixel;
                    m_TempVerts[3].position *= unitsPerPixel;

                    float pixelOffset = emojiDic[index].size / 32 / 2;
                    m_TempVerts[0].uv1 = new Vector2(emojiDic[index].x + pixelOffset, emojiDic[index].y + pixelOffset);
                    m_TempVerts[1].uv1 = new Vector2(emojiDic[index].x - pixelOffset + emojiDic[index].size, emojiDic[index].y + pixelOffset);
                    m_TempVerts[2].uv1 = new Vector2(emojiDic[index].x - pixelOffset + emojiDic[index].size, emojiDic[index].y - pixelOffset + emojiDic[index].size);
                    m_TempVerts[3].uv1 = new Vector2(emojiDic[index].x + pixelOffset, emojiDic[index].y - pixelOffset + emojiDic[index].size);

                    toFill.AddUIVertexQuad(m_TempVerts);

                    i += 4 * info.len - 1;
                }
                else
                {
                    int tempVertsIndex = i & 3;
                    if (tempVertsIndex == 0 && verts[i].position.y < repairY)
                    {
                        repairY = verts[i + 3].position.y;
                        repairDistance = repairDistanceHalf;
                        repairDistanceHalf = 0;
                    }
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position -= new Vector3(repairDistance, 0, 0);
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
        }
        m_DisableFontTextureRebuiltCallback = false;

        UIVertex vert = new UIVertex();
        // 处理超链接包围框
        foreach (var hrefInfo in m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }
            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
            var pos = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
            {
                if (i >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);
                pos = vert.position;
                if (pos.x < bounds.min.x) // 换行重新添加包围框
                {
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                }
                else
                {
                    bounds.Encapsulate(pos); // 扩展包围框
                }
            }
            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
        }
    }

    private List<HrefInfoMatch> MatchHrefInfo(string outputText)
    {
        if (HrefInfoList == null)
            HrefInfoList = new List<HrefInfoMatch>();
        else
            HrefInfoList.Clear();
        
        int length = outputText.Length;
        for (int i = 0; i < length; i++)
        {
            if (outputText[i] == '<' && i + 1 < length && outputText[i + 1] == 'a')
            {
                if (i + 8 <= length && outputText.Substring(i, 8) == "<a href=")
                {
                    for (int j = i+10; j < length; j++)
                    {
                        if (outputText[j] == '<')
                        {
                            if (j + 4 <= length && outputText.Substring(j, 4) == "</a>")
                            {
                                HrefInfoMatch hrefInfo = new HrefInfoMatch();
                                hrefInfo.Index = i;
                                hrefInfo.HrefIdx = outputText[i+8].ToString();
                                hrefInfo.Value = outputText.Substring(i + 10, j - i - 10);
                                hrefInfo.Length = hrefInfo.Value.Length;
                                hrefInfo.Strlen = j + 4 - i;
                                HrefInfoList.Add(hrefInfo);
                                i = j + 3;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return HrefInfoList;
    }

    /// <summary>
    /// 超链接正则
    /// </summary>
    public static readonly Regex s_HrefRegex =
        new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<HrefInfo> m_HrefInfos = new List<HrefInfo>();

    /// <summary>
    /// 文本构造器
    /// </summary>
    protected static readonly StringBuilder s_TextBuilder = new StringBuilder();
    /// <summary>
    /// 获取超链接解析后的最后输出文本
    /// </summary>
    /// <returns></returns>
    protected virtual string GetOutputText(string outputText)
    {
        if (!UseHint)
        {
            return outputText;
        }
        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear();
        var indexText = 0;

        //foreach (Match match in s_HrefRegex.Matches(outputText))
        //{
        //    s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));
        //    //s_TextBuilder.Append("<color='#9ed7ff'>");  // 超链接颜色ff6600
        //    var group = match.Groups[1];
        //    var hrefInfo = new HrefInfo
        //    {
        //        startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
        //        endIndex = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
        //        name = group.Value
        //    };
        //    m_HrefInfos.Add(hrefInfo);

        //    s_TextBuilder.Append(match.Groups[2].Value);
        //    //s_TextBuilder.Append("</color>");
        //    indexText = match.Index + match.Length;
        //}

        //s_TextBuilder.Length = 0;
        //m_HrefInfos.Clear();
        //indexText = 0;
        List<HrefInfoMatch> hrefInfos = MatchHrefInfo(outputText);
        int length = hrefInfos.Count;
        for (int i = 0; i < length; i++)
        {
            s_TextBuilder.Append(outputText.Substring(indexText, hrefInfos[i].Index - indexText));
            var hrefInfo = new HrefInfo
            {
                startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
                endIndex = (s_TextBuilder.Length + hrefInfos[i].Length - 1) * 4 + 3,
                name = hrefInfos[i].HrefIdx
            };
            m_HrefInfos.Add(hrefInfo);

            s_TextBuilder.Append(hrefInfos[i].Value);
            //s_TextBuilder.Append("</color>");
            indexText = hrefInfos[i].Index + hrefInfos[i].Strlen;
        }

        s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
        return s_TextBuilder.ToString();
    }

    private void HrefInfosIndexAdjust(int imgIndex)
    {
        foreach (var hrefInfo in m_HrefInfos)//如果后面有超链接，需要把位置往前挪
        {
            if (imgIndex < hrefInfo.startIndex)
            {
                hrefInfo.startIndex -= 8;
                hrefInfo.endIndex -= 8;
            }
        }
    }

    protected static readonly StringBuilder s_TextWithOutHrefBuilder = new StringBuilder();
    private string GetTextWithOutHref(string outputText)
    {
        if (!UseHint)
        {
            return outputText;
        }
        s_TextWithOutHrefBuilder.Length = 0;
        var indexText = 0;
        //foreach (Match match in s_HrefRegex.Matches(outputText))
        //{
        //    s_TextWithOutHrefBuilder.Append(outputText.Substring(indexText, match.Index - indexText));

        //    s_TextWithOutHrefBuilder.Append(match.Groups[2].Value);
        //    indexText = match.Index + match.Length;
        //}

        List<HrefInfoMatch> hrefInfos = MatchHrefInfo(outputText);
        int length = hrefInfos.Count;
        for (int i = 0; i < length; i++)
        {
            s_TextWithOutHrefBuilder.Append(outputText.Substring(indexText, hrefInfos[i].Index - indexText));

            s_TextWithOutHrefBuilder.Append(hrefInfos[i].Value);
            indexText = hrefInfos[i].Index + hrefInfos[i].Strlen;
        }

        s_TextWithOutHrefBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
        return s_TextWithOutHrefBuilder.ToString();
    }

    public override float preferredWidth
    {
        get
        {
            var settings = GetGenerationSettings(Vector2.zero);
            return cachedTextGeneratorForLayout.GetPreferredWidth(GetTextWithOutHref(m_Text), settings) / pixelsPerUnit;
        }
    }

    public override float preferredHeight
    {
        get
        {
            var settings = GetGenerationSettings(new Vector2(GetPixelAdjustedRect().size.x, 0.0f));
            return cachedTextGeneratorForLayout.GetPreferredHeight(GetTextWithOutHref(m_Text), settings) / pixelsPerUnit;
        }
    }

    public delegate void VoidOnHrefClick(string hefName);
    public VoidOnHrefClick onHrefClick;

    /// <summary>
    /// 点击事件检测是否点击到超链接文本
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 lp;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out lp);
        foreach (var hrefInfo in m_HrefInfos)
        {
            var boxes = hrefInfo.boxes;

            for (var i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Contains(lp))
                {
                    if (onHrefClick != null)
                    {
                        onHrefClick(hrefInfo.name);
                    }
                    Debug.Log("点击了:" + hrefInfo.name);
                    return;
                }
            }
        }

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        GameObject current = eventData.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                if (ExecuteEvents.Execute(results[i].gameObject, eventData, ExecuteEvents.pointerClickHandler))
                    break;
            }
        }
    }

    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HrefInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }
}
