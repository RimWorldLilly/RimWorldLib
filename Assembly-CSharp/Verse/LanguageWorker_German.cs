using System;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x0200021C RID: 540
	public class LanguageWorker_German : LanguageWorker
	{
		// Token: 0x06000FB3 RID: 4019 RVA: 0x00058A5C File Offset: 0x00056C5C
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "ein " + str;
			case Gender.Male:
				return "ein " + str;
			case Gender.Female:
				return "eine " + str;
			default:
				return str;
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00058AA8 File Offset: 0x00056CA8
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "das " + str;
			case Gender.Male:
				return "der " + str;
			case Gender.Female:
				return "die " + str;
			default:
				return str;
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00058AF4 File Offset: 0x00056CF4
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			return Regex.Replace(str, "(.)(.)(\\(/Name\\)|</color>)?'s(?=\\s|$)", delegate(Match m)
			{
				string value = m.Groups[1].Value;
				string value2 = m.Groups[2].Value;
				string value3 = m.Groups[3].Value;
				if (value2 == "s" || value2 == "ß" || value2 == "z" || value2 == "x" || (value == "c" && value2 == "e"))
				{
					return value + value2 + value3 + "'";
				}
				return value + value2 + value3 + "s";
			}, RegexOptions.Compiled);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00058B2A File Offset: 0x00056D2A
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString() + ".";
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00058B3D File Offset: 0x00056D3D
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			return LanguageWorker_German.englishWorker.Pluralize(str, gender, count);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00058B4C File Offset: 0x00056D4C
		public override string PostProcessThingLabelForRelic(string thingLabel)
		{
			char[] anyOf = new char[]
			{
				' ',
				'-'
			};
			int num = thingLabel.LastIndexOfAny(anyOf);
			if (num != -1)
			{
				thingLabel = thingLabel.Substring(num + 1);
			}
			foreach (string text in new string[]
			{
				"Horn",
				"Lanze",
				"Pulser",
				"Werfer",
				"Axt",
				"Flinte",
				"Bogen",
				"Revolver",
				"Gewehr",
				"Stoßzahn",
				"Stab",
				"Hammer",
				"Schwert",
				"Pistole",
				"Dolch",
				"Büchse",
				"Kanone",
				"Granaten",
				"Granate",
				"Keule",
				"Säbel",
				"Messer",
				"Rapier",
				"Klinge",
				"Sense",
				"Speer"
			})
			{
				if (thingLabel.EndsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					thingLabel = text;
					break;
				}
			}
			return thingLabel;
		}

		// Token: 0x04000EE1 RID: 3809
		private static readonly LanguageWorker_English englishWorker = new LanguageWorker_English();
	}
}
