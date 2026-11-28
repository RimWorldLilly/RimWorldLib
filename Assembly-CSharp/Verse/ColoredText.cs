using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005AB RID: 1451
	public static class ColoredText
	{
		// Token: 0x06002BF4 RID: 11252 RVA: 0x0010A47C File Offset: 0x0010867C
		public static void ResetStaticData()
		{
			ColoredText.DateTimeRegexes = new List<Regex>();
			ColoredText.AddRegexPatternsForDateString("PeriodYears".Translate(), "Period1Year".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodQuadrums".Translate(), "Period1Quadrum".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodDays".Translate(), "Period1Day".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodHours".Translate(), "Period1Hour".Translate());
			ColoredText.AddRegexPatternsForDateString("PeriodSeconds".Translate(), "Period1Second".Translate());
			string str = string.Concat(new string[]
			{
				"(",
				FactionDefOf.PlayerColony.pawnsPlural,
				"|",
				FactionDefOf.PlayerColony.pawnSingular,
				")"
			});
			ColoredText.ColonistCountRegex = new Regex("\\d+\\.?\\d* " + str);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x0010A598 File Offset: 0x00108798
		private static void AddRegexPatternsForDateString(string dateMany, string dateOne)
		{
			if (dateMany.Contains("{0}"))
			{
				ColoredText.DateTimeRegexes.Add(new Regex(string.Concat(new string[]
				{
					"(",
					string.Format(dateMany, "\\d+\\.?\\d*"),
					"|",
					dateOne,
					")"
				})));
				return;
			}
			List<string> list = GrammarResolverSimple.TryParseNumCase(dateMany);
			if (!list.NullOrEmpty<string>())
			{
				foreach (string text in list)
				{
					ColoredText.DateTimeRegexes.Add(new Regex(string.Concat(new string[]
					{
						"(\\d+\\.?\\d* ",
						text,
						"|",
						dateOne,
						")"
					})));
				}
			}
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x0010A67C File Offset: 0x0010887C
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x0010A688 File Offset: 0x00108888
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x0010A6D4 File Offset: 0x001088D4
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x0010A6ED File Offset: 0x001088ED
		public static TaggedString ApplyTag(this string s, Ideo ideo)
		{
			if (ideo == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Ideo, ideo.GetUniqueLoadID());
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x0010A708 File Offset: 0x00108908
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x0010A75A File Offset: 0x0010895A
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x0010A768 File Offset: 0x00108968
		public static string Resolve(TaggedString taggedStr)
		{
			if (taggedStr == null)
			{
				return null;
			}
			string rawText = taggedStr.RawText;
			if (rawText.NullOrEmpty())
			{
				return rawText;
			}
			string result;
			if (ColoredText.cache.TryGetValue(rawText, out result))
			{
				return result;
			}
			ColoredText.resultBuffer.Length = 0;
			if (rawText.IndexOf("(*") < 0)
			{
				ColoredText.resultBuffer.Append(rawText);
			}
			else
			{
				for (int i = 0; i < rawText.Length; i++)
				{
					char c = rawText[i];
					if (c == '(' && i < rawText.Length - 1 && rawText[i + 1] == '*' && rawText.IndexOf(')', i) > i + 1)
					{
						bool flag = false;
						int num = i;
						ColoredText.tagBuffer.Length = 0;
						ColoredText.argBuffer.Length = 0;
						ColoredText.capStage = ColoredText.CaptureStage.Tag;
						for (i += 2; i < rawText.Length; i++)
						{
							char c2 = rawText[i];
							if (c2 == ')')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Result;
								if (flag)
								{
									string value = rawText.Substring(num, i - num + 1).SwapTagWithColor(ColoredText.tagBuffer.ToString(), ColoredText.argBuffer.ToString());
									ColoredText.resultBuffer.Append(value);
									break;
								}
							}
							else if (c2 == '/')
							{
								flag = true;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Arg)
							{
								ColoredText.argBuffer.Append(c2);
							}
							if (!flag && c2 == '=')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Arg;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Tag)
							{
								ColoredText.tagBuffer.Append(c2);
							}
						}
						if (!flag)
						{
							ColoredText.resultBuffer.Append(c);
							i = num + 1;
						}
					}
					else
					{
						ColoredText.resultBuffer.Append(c);
					}
				}
			}
			string text = ColoredText.resultBuffer.ToString();
			for (int j = 0; j < ColoredText.DateTimeRegexes.Count; j++)
			{
				text = ColoredText.DateTimeRegexes[j].Replace(text, "$&".Colorize(ColoredText.DateTimeColor));
			}
			text = ColoredText.CurrencyRegex.Replace(text, "$&".Colorize(ColoredText.CurrencyColor));
			text = ColoredText.ColonistCountRegex.Replace(text, "$&".Colorize(ColoredText.ColonistCountColor));
			ColoredText.cache.Add(rawText, text);
			return text;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x0010A9A0 File Offset: 0x00108BA0
		public static string Colorize(this TaggedString ts, Color color)
		{
			return ts.Resolve().Colorize(color);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x0010A9AF File Offset: 0x00108BAF
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x0010A9C4 File Offset: 0x00108BC4
		private static string SwapTagWithColor(this string str, string tag, string arg)
		{
			TagType tagType = ColoredText.ParseEnum<TagType>(tag.CapitalizeFirst(), true);
			string text = str.StripTags();
			switch (tagType)
			{
			case TagType.Undefined:
				return str;
			case TagType.Name:
				return text.Colorize(ColoredText.NameColor);
			case TagType.Faction:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction = Find.FactionManager.AllFactions.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				return text.Colorize(ColoredText.GetFactionRelationColor(faction));
			}
			case TagType.Settlement:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction2 = Find.FactionManager.AllFactionsVisible.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction2 == null)
				{
					return text.Colorize(ColoredText.SubtleGrayColor);
				}
				if (faction2 == null)
				{
					return text;
				}
				return text.Colorize(faction2.Color);
			}
			case TagType.DateTime:
				return text.Colorize(ColoredText.DateTimeColor);
			case TagType.ColonistCount:
				return text.Colorize(ColoredText.ColonistCountColor);
			case TagType.Threat:
				return text.Colorize(ColoredText.ThreatColor);
			case TagType.Ideo:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Ideo ideo = Find.IdeoManager.IdeosListForReading.Find((Ideo x) => x.GetUniqueLoadID() == arg);
				if (ideo == null)
				{
					return text;
				}
				return text.Colorize(ideo.TextColor);
			}
			case TagType.SectionTitle:
				return text.Colorize(ColoredText.TipSectionTitleColor);
			case TagType.Red:
				return text.Colorize(ColorLibrary.RedReadable);
			case TagType.Reward:
				return text.Colorize(ColoredText.CurrencyColor);
			case TagType.Gray:
				return text.Colorize(ColoredText.SubtleGrayColor);
			default:
				Log.ErrorOnce("Invalid tag '" + tag + "'", tag.GetHashCode());
				return text;
			}
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x0010AB8C File Offset: 0x00108D8C
		private static Color GetFactionRelationColor(Faction faction)
		{
			if (faction == null)
			{
				return Color.white;
			}
			if (faction.IsPlayer)
			{
				return faction.Color;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return ColoredText.FactionColor_Hostile;
			case FactionRelationKind.Neutral:
				return ColoredText.FactionColor_Neutral;
			case FactionRelationKind.Ally:
				return ColoredText.FactionColor_Ally;
			default:
				return faction.Color;
			}
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x0010ABEC File Offset: 0x00108DEC
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x0010AC2B File Offset: 0x00108E2B
		public static StringBuilder AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			return sb.Append(taggedString.Resolve());
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x0010AC3A File Offset: 0x00108E3A
		public static StringBuilder AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			return sb.AppendLine(taggedString.Resolve());
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x0010AC49 File Offset: 0x00108E49
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x0010AC56 File Offset: 0x00108E56
		public static string AsTipTitle(this TaggedString ts)
		{
			return ts.Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x0010AC63 File Offset: 0x00108E63
		public static string AsTipTitle(this string s)
		{
			return s.Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x04001DBA RID: 7610
		private static StringBuilder resultBuffer = new StringBuilder();

		// Token: 0x04001DBB RID: 7611
		private static StringBuilder tagBuffer = new StringBuilder();

		// Token: 0x04001DBC RID: 7612
		private static StringBuilder argBuffer = new StringBuilder();

		// Token: 0x04001DBD RID: 7613
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		// Token: 0x04001DBE RID: 7614
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		// Token: 0x04001DBF RID: 7615
		private static Regex ColonistCountRegex;

		// Token: 0x04001DC0 RID: 7616
		private static List<Regex> DateTimeRegexes;

		// Token: 0x04001DC1 RID: 7617
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		// Token: 0x04001DC2 RID: 7618
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		// Token: 0x04001DC3 RID: 7619
		public static readonly Color TipSectionTitleColor = new Color(0.9f, 0.9f, 0.3f);

		// Token: 0x04001DC4 RID: 7620
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		// Token: 0x04001DC5 RID: 7621
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		// Token: 0x04001DC6 RID: 7622
		public static readonly Color FactionColor_Hostile = ColorLibrary.RedReadable;

		// Token: 0x04001DC7 RID: 7623
		public static readonly Color ThreatColor = GenColor.FromHex("d46f68");

		// Token: 0x04001DC8 RID: 7624
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		// Token: 0x04001DC9 RID: 7625
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		// Token: 0x04001DCA RID: 7626
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		// Token: 0x04001DCB RID: 7627
		public static readonly Color SubtleGrayColor = GenColor.FromHex("999999");

		// Token: 0x04001DCC RID: 7628
		public static readonly Color ExpectationsColor = new Color(0.57f, 0.9f, 0.69f);

		// Token: 0x04001DCD RID: 7629
		public static readonly Color ImpactColor = GenColor.FromHex("c79fef");

		// Token: 0x04001DCE RID: 7630
		public static readonly Color GeneColor = ColorLibrary.LightBlue;

		// Token: 0x04001DCF RID: 7631
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		// Token: 0x04001DD0 RID: 7632
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		// Token: 0x04001DD1 RID: 7633
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		// Token: 0x04001DD2 RID: 7634
		private const string Digits = "\\d+\\.?\\d*";

		// Token: 0x04001DD3 RID: 7635
		private const string Replacement = "$&";

		// Token: 0x04001DD4 RID: 7636
		private const string TagStartString = "(*";

		// Token: 0x04001DD5 RID: 7637
		private const char TagStartChar = '(';

		// Token: 0x04001DD6 RID: 7638
		private const char TagEndChar = ')';

		// Token: 0x020027A2 RID: 10146
		private enum CaptureStage
		{
			// Token: 0x0400A218 RID: 41496
			Tag,
			// Token: 0x0400A219 RID: 41497
			Arg,
			// Token: 0x0400A21A RID: 41498
			Result
		}
	}
}
