using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


static public class FormattedXmlParser {

	static XmlReaderSettings xrs_ = new XmlReaderSettings();
	static FormattedXmlParser() {
		xrs_.CheckCharacters = true;
		xrs_.IgnoreComments = true;
		xrs_.IgnoreWhitespace = true;
		xrs_.ConformanceLevel = ConformanceLevel.Fragment;
	}

	public interface IObserver {
		void ParseException(System.Exception e);
		void ParseText(string text, uint color);
		void ParseAutomove(string text, int x, int y, int mapId);
		void ParseVar(string varName, string varValue, uint color);
	}

	private static int ConvertToInt32(string s) {
		if (s == null || s.Length == 0) {
			return 0;
		}
		int result;
		if (int.TryParse(s, out result)) {
			return result;
		} else {
			return 0;
		}
	}

	static public bool Execute(string xml, IObserver observer) {
		Stack<uint> colorStack = new Stack<uint>(10);
		colorStack.Push(0xff3a3123);	// 缺省颜色
		try {
			using (XmlReader reader = XmlReader.Create(new StringReader(xml), xrs_)) {
				bool automove = false;
				int x = 0;
				int y = 0;
				int mapId = 0;
				while (reader.Read()) {
					switch (reader.NodeType) {
					case XmlNodeType.Element:
						switch (reader.Name) {
						case "color":
							string colorName = reader.GetAttribute("value");
							uint color = ColorTranslater.FromName(colorName);
							colorStack.Push(color);
							break;
						case "var":
							string varName = reader.GetAttribute("name");
							string varValue = reader.GetAttribute("value");
							observer.ParseVar(varName, varValue, colorStack.Peek());
							break;
						case "automove":
							automove = true;
							x = ConvertToInt32(reader.GetAttribute("x"));
							y = ConvertToInt32(reader.GetAttribute("y"));
							mapId = ConvertToInt32(reader.GetAttribute("map"));
							break;
						}
						break;
					case XmlNodeType.EndElement:
						switch (reader.Name) {
						case "color":
							colorStack.Pop();
							break;
						case "automove":
							automove = false;
							break;
						}
						break;
					case XmlNodeType.Text:
						if (automove) {
							observer.ParseAutomove(reader.Value, x, y, mapId);
						} else {
							observer.ParseText(reader.Value, colorStack.Peek());
						}
						break;
					}
				}
			}
		} catch (System.Exception e) {
			observer.ParseException(e);
			return false;
		}
		return true;
	}

	static public class ColorTranslater {

		static public readonly uint COLOR_DEFAULT_TEXT = 0xff3a3123;

		static readonly private Dictionary<string, uint> nameTable_ = new Dictionary<string, uint>(32);
		
		static readonly private string[,] nameDesc_ = new string[16,2] {
			{ "black", "黑色" },
			{"silver", "银色"},
			{"gray", "深灰"},
			{"white", "白色"},

			{"maroon", "暗红"},
			{"red", "亮红"},
			{"purple", "紫色"},
			{"fuchsia", "洋红"},

			{"green", "深绿"},
			{"lime", "亮绿"},
			{"olive", "暗黄"},
			{"yellow", "亮黄"},

			{"navy", "深蓝"},
			{"blue", "亮蓝"},
			{"teal", "蓝绿"},
			{"aqua", "亮青"},
		};

		static ColorTranslater() {
			nameTable_.Add("none", 0x00000000);
			nameTable_.Add("black", 0xff000000);
			nameTable_.Add("silver", 0xffc0c0c0);
			nameTable_.Add("gray", 0xff808080);
			nameTable_.Add("white", 0xffffffff);

			nameTable_.Add("maroon", 0xff800000);
			nameTable_.Add("red", 0xffff0000);
			nameTable_.Add("purple", 0xff800080);
			nameTable_.Add("fuchsia", 0xffff00ff);

			nameTable_.Add("green", 0xff008000);
			nameTable_.Add("lime", 0xff00ff00);
			nameTable_.Add("olive", 0xff808000);
			nameTable_.Add("yellow", 0xffffff00);

			nameTable_.Add("navy", 0xff000080);
			nameTable_.Add("blue", 0xff0000ff);
			nameTable_.Add("teal", 0xff008080);
			nameTable_.Add("aqua", 0xff00ffff);
			nameTable_.Add("cyan", 0xff00ffff);
		}

		public delegate void ForEachColorCallback(string name, string desc, int index);
		static public void ForEachColor(ForEachColorCallback f) {
			for (int i=0; i<=nameDesc_.GetUpperBound(0); ++i) {
			    f(nameDesc_[i,0], nameDesc_[i,1], i);
			}
		}

		static public uint FromName(string name) {
			if (string.IsNullOrEmpty(name)) {
				return COLOR_DEFAULT_TEXT;
			}
			uint result;
			if (nameTable_.TryGetValue(name, out result)) {
				return result;
			}
			if (name.Length > 2 && name[0]=='0' && (name[1]=='x' || name[1]=='X')) {
				if (uint.TryParse(name.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier, null, out result)) {
					return result;
				}
			} 
			if (uint.TryParse(name, out result)) {
				return result;
			} else {
				return COLOR_DEFAULT_TEXT;
			}
		}

		static public void SplitRGB(uint v, out byte r, out byte g, out byte b, out byte a) {
			a = (byte)((v >> 24) & 0xff);
			r = (byte)((v >> 16) & 0xff);
			g = (byte)((v >> 8) & 0xff);
			b = (byte)(v & 0xff);
		}
	}

}
