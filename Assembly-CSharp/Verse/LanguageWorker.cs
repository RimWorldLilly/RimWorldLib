using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000214 RID: 532
	public abstract class LanguageWorker
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x00056F90 File Offset: 0x00055190
		public virtual int TotalNumCaseCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00056F94 File Offset: 0x00055194
		public virtual string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("IndefiniteForm".CanTranslate())
			{
				return "IndefiniteForm".Translate(str);
			}
			return "IndefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00056FF6 File Offset: 0x000551F6
		public string WithIndefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithIndefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), plural, name);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0005700E File Offset: 0x0005520E
		public string WithIndefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00057021 File Offset: 0x00055221
		public string WithIndefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, plural, name));
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00057034 File Offset: 0x00055234
		public virtual string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("DefiniteForm".CanTranslate())
			{
				return "DefiniteForm".Translate(str);
			}
			return "DefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00057096 File Offset: 0x00055296
		public string WithDefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithDefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), plural, name);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000570AE File Offset: 0x000552AE
		public string WithDefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000570C1 File Offset: 0x000552C1
		public string WithDefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, plural, name));
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000570D2 File Offset: 0x000552D2
		public virtual string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString();
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000570DB File Offset: 0x000552DB
		public virtual string PostProcessed(string str)
		{
			return str.MergeMultipleSpaces(true);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000570E4 File Offset: 0x000552E4
		public virtual string ToTitleCase(string str)
		{
			return str.CapitalizeFirst();
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000570EC File Offset: 0x000552EC
		public virtual string Pluralize(string str, Gender gender, int count = -1)
		{
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			return str;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00057109 File Offset: 0x00055309
		public string Pluralize(string str, int count = -1)
		{
			return this.Pluralize(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), count);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00057120 File Offset: 0x00055320
		public virtual bool TryLookupPluralForm(string str, Gender gender, out string plural, int count = -1)
		{
			plural = null;
			if (str.NullOrEmpty() || (count != -1 && count < 2))
			{
				return false;
			}
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable("plural");
			if (lookupTable == null)
			{
				return false;
			}
			string key = str.ToLower();
			if (!lookupTable.ContainsKey(key))
			{
				return false;
			}
			string[] array = lookupTable[key];
			if (array.Length < 2)
			{
				return false;
			}
			plural = array[1];
			if (str.Length != 0 && char.IsUpper(str[0]))
			{
				plural = plural.CapitalizeFirst();
			}
			return true;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000571A8 File Offset: 0x000553A8
		public virtual bool TryLookUp(string tableName, string keyName, int index, out string result, string fullStringForReference = null)
		{
			result = null;
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable(tableName);
			if (lookupTable == null)
			{
				return false;
			}
			if (keyName.NullOrEmpty())
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning("Tried to lookup an empty key in table '" + tableName + "'.");
				}
				result = keyName;
				return true;
			}
			string text = keyName.ToLower();
			if (!lookupTable.ContainsKey(text))
			{
				LanguageWorker.ParenthesisRegex.Replace(text, "");
				text = text.Trim();
				if (!lookupTable.ContainsKey(text))
				{
					if (DebugSettings.logTranslationLookupErrors)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Tried a lookup for key '");
						defaultInterpolatedStringHandler.AppendFormatted(keyName);
						defaultInterpolatedStringHandler.AppendLiteral("' in table '");
						defaultInterpolatedStringHandler.AppendFormatted(tableName);
						defaultInterpolatedStringHandler.AppendLiteral("', which doesn't exist.");
						Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					result = keyName;
					return true;
				}
			}
			string[] array = lookupTable[text];
			if (array.Length < index + 1)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(64, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("Tried a lookup an out-of-bounds index '");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(index);
					defaultInterpolatedStringHandler2.AppendLiteral("' for key '");
					defaultInterpolatedStringHandler2.AppendFormatted(keyName);
					defaultInterpolatedStringHandler2.AppendLiteral("' in table '");
					defaultInterpolatedStringHandler2.AppendFormatted(tableName);
					defaultInterpolatedStringHandler2.AppendLiteral("'.");
					Log.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				result = keyName;
				return true;
			}
			result = array[index];
			return true;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00057303 File Offset: 0x00055503
		public virtual string PostProcessThingLabelForRelic(string thingLabel)
		{
			if (thingLabel.IndexOf(' ') != -1)
			{
				return null;
			}
			return thingLabel;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00057314 File Offset: 0x00055514
		public virtual string ResolveNumCase(float number, List<string> args)
		{
			string formOne = args[0].Trim('\'');
			string text = args[1].Trim('\'');
			string formMany = args[2].Trim('\'');
			if (number - Mathf.Floor(number) > 1E-45f)
			{
				return number.ToString() + " " + text;
			}
			return number.ToString() + " " + this.GetFormForNumber((int)number, formOne, text, formMany);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0005738C File Offset: 0x0005558C
		protected virtual string GetFormForNumber(int num, string formOne, string formSeveral, string formMany)
		{
			int num2 = num % 10;
			if (num / 10 % 10 == 1)
			{
				return formMany;
			}
			if (num2 == 1)
			{
				return formOne;
			}
			if (num2 - 2 > 2)
			{
				return formMany;
			}
			return formSeveral;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000573C0 File Offset: 0x000555C0
		public virtual string ResolveReplace(List<string> args)
		{
			if (args.Count == 0)
			{
				return null;
			}
			string text = args[0];
			if (args.Count == 1)
			{
				return text;
			}
			for (int i = 1; i < args.Count; i++)
			{
				string input = args[i];
				Match match = LanguageWorker.replaceArgRegex.Match(input);
				if (!match.Success)
				{
					return null;
				}
				string value = match.Groups["old"].Value;
				string value2 = match.Groups["new"].Value;
				if (text.Contains(value))
				{
					return text.Replace(value, value2);
				}
			}
			return text;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0005745D File Offset: 0x0005565D
		public virtual string ResolveFunction(string functionName, List<string> args, string fullStringForReference)
		{
			if (functionName == "lookup")
			{
				return this.ResolveLookup(args, fullStringForReference);
			}
			if (functionName == "replace")
			{
				return this.ResolveReplace(args);
			}
			return "";
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00057490 File Offset: 0x00055690
		protected string ResolveLookup(List<string> args, string fullStringForReference)
		{
			if (args.Count != 2 && args.Count != 3)
			{
				Log.Error("Invalid argument number for 'lookup' function, expected 2 or 3. A key to lookup, table name and optional index if there's more than 1 entry per key. Full string: " + fullStringForReference);
				return "";
			}
			string text = args[1];
			int index = 1;
			if (args.Count == 3 && !int.TryParse(args[2], out index))
			{
				Log.Error("Invalid lookup index value: '" + args[2] + "' Full string: " + fullStringForReference);
				return "";
			}
			string result;
			if (this.TryLookUp(text.ToLower(), args[0], index, out result, fullStringForReference))
			{
				return result;
			}
			return "";
		}

		// Token: 0x04000ECD RID: 3789
		private static Regex ParenthesisRegex = new Regex("\\((.*?)\\)");

		// Token: 0x04000ECE RID: 3790
		private static readonly Regex replaceArgRegex = new Regex("(?<old>[^\"]*?)\"-\"(?<new>[^\"]*?)\"", RegexOptions.Compiled);
	}
}
