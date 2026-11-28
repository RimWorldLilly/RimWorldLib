using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000223 RID: 547
	public class LanguageWorker_Russian : LanguageWorker
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0005956E File Offset: 0x0005776E
		public override int TotalNumCaseCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00059571 File Offset: 0x00057771
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0005957C File Offset: 0x0005777C
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty() || (count != -1 && count < 2))
			{
				return str;
			}
			string text;
			if (!this.TryLookupPluralForm(str, gender, out text, count))
			{
				text = this.PluralizeFallback(str, gender, count);
			}
			if (count == -1)
			{
				return text;
			}
			string formSeveral;
			string formMany;
			if (this.TryLookUp("Case", str, 1, out formSeveral, null) && this.TryLookUp("Case", text, 1, out formMany, null))
			{
				return this.GetFormForNumber(count, str, formSeveral, formMany);
			}
			return text;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x000595E8 File Offset: 0x000577E8
		public override bool TryLookUp(string tableName, string key, int index, out string result, string fullStringForReference = null)
		{
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable(tableName);
			if (lookupTable == null)
			{
				result = null;
				return false;
			}
			key = key.ToLower().Trim();
			if (key.NullOrEmpty())
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to lookup a bad key '");
					defaultInterpolatedStringHandler.AppendFormatted(key);
					defaultInterpolatedStringHandler.AppendLiteral("' in table '");
					defaultInterpolatedStringHandler.AppendFormatted(tableName);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = key;
				return true;
			}
			bool result2;
			try
			{
				int i = key.Length;
				while (i > 0)
				{
					string key2 = key.Substring(0, i);
					string str = key.Substring(i);
					string[] array = lookupTable.TryGetValue(key2, null);
					if (array != null)
					{
						if (index < 0 || array.Length <= index)
						{
							if (DebugSettings.logTranslationLookupErrors)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(64, 3);
								defaultInterpolatedStringHandler2.AppendLiteral("Tried a lookup an out-of-bounds index '");
								defaultInterpolatedStringHandler2.AppendFormatted<int>(index);
								defaultInterpolatedStringHandler2.AppendLiteral("' for key '");
								defaultInterpolatedStringHandler2.AppendFormatted(key);
								defaultInterpolatedStringHandler2.AppendLiteral("' in table '");
								defaultInterpolatedStringHandler2.AppendFormatted(tableName);
								defaultInterpolatedStringHandler2.AppendLiteral("'.");
								Log.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
							}
							result = key;
							return true;
						}
						result = array[index] + str;
						return true;
					}
					else
					{
						i = key.LastIndexOf(' ', i - 1);
					}
				}
				if (DebugSettings.logTranslationLookupErrors)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(59, 2);
					defaultInterpolatedStringHandler3.AppendLiteral("Tried a lookup for key '");
					defaultInterpolatedStringHandler3.AppendFormatted(key);
					defaultInterpolatedStringHandler3.AppendLiteral("' in table '");
					defaultInterpolatedStringHandler3.AppendFormatted(tableName);
					defaultInterpolatedStringHandler3.AppendLiteral("', which doesn't exist.");
					Log.Warning(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				result = key;
				result2 = true;
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(85, 5);
				defaultInterpolatedStringHandler4.AppendLiteral("Exception while looking up in tableName: ");
				defaultInterpolatedStringHandler4.AppendFormatted(tableName);
				defaultInterpolatedStringHandler4.AppendLiteral(", key: ");
				defaultInterpolatedStringHandler4.AppendFormatted(key);
				defaultInterpolatedStringHandler4.AppendLiteral(", index: ");
				defaultInterpolatedStringHandler4.AppendFormatted<int>(index);
				defaultInterpolatedStringHandler4.AppendLiteral(", fullStringForReference: ");
				defaultInterpolatedStringHandler4.AppendFormatted(fullStringForReference);
				defaultInterpolatedStringHandler4.AppendLiteral(": ");
				defaultInterpolatedStringHandler4.AppendFormatted(ex.Message);
				Log.Error(defaultInterpolatedStringHandler4.ToStringAndClear());
				result = key;
				result2 = true;
			}
			return result2;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00059858 File Offset: 0x00057A58
		private string PluralizeFallback(string str, Gender gender, int count = -1)
		{
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			switch (gender)
			{
			case Gender.None:
			{
				if (c <= 'O')
				{
					if (c != 'E')
					{
						if (c != 'O')
						{
							break;
						}
						return str.Substring(0, str.Length - 1) + "A";
					}
				}
				else if (c != 'e')
				{
					if (c == 'o')
					{
						return str.Substring(0, str.Length - 1) + "a";
					}
					break;
				}
				char value = char.ToUpper(c2);
				if ("ГКХЖЧШЩЦ".IndexOf(value) >= 0)
				{
					if (c == 'E')
					{
						return str.Substring(0, str.Length - 1) + "A";
					}
					if (c == 'e')
					{
						return str.Substring(0, str.Length - 1) + "a";
					}
				}
				else
				{
					if (c == 'E')
					{
						return str.Substring(0, str.Length - 1) + "Я";
					}
					if (c == 'e')
					{
						return str.Substring(0, str.Length - 1) + "я";
					}
				}
				break;
			}
			case Gender.Male:
				if (LanguageWorker_Russian.IsConsonant(c))
				{
					return str + "ы";
				}
				if (c <= 'Ь')
				{
					if (c == 'Й')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'Ь')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
				}
				else
				{
					if (c == 'й')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
				}
				break;
			case Gender.Female:
				if (c <= 'Ь')
				{
					if (c != 'A' && c != 'a')
					{
						if (c == 'Ь')
						{
							return str.Substring(0, str.Length - 1) + "И";
						}
					}
					else
					{
						char value2 = char.ToUpper(c2);
						if ("ГКХЖЧШЩ".IndexOf(value2) >= 0)
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "и";
							}
							return str.Substring(0, str.Length - 1) + "И";
						}
						else
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "ы";
							}
							return str.Substring(0, str.Length - 1) + "Ы";
						}
					}
				}
				else
				{
					if (c == 'Я')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'я')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
				}
				break;
			}
			return str;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00059B6A File Offset: 0x00057D6A
		private static bool IsConsonant(char ch)
		{
			return "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСТФХЦЧШЩ".IndexOf(ch) >= 0;
		}
	}
}
