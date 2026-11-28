using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x020007AD RID: 1965
	public class RulePack
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x00195C48 File Offset: 0x00193E48
		public List<Rule> Rules
		{
			get
			{
				if (this.rulesResolved == null)
				{
					this.rulesResolved = RulePack.GetRulesResolved(this.rulesRaw, this.rulesStrings, this.rulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.rulesResolved.AddRange(rulePackDef.RulesPlusIncludes);
						}
					}
				}
				return this.rulesResolved;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x00195CD8 File Offset: 0x00193ED8
		public List<Rule> UntranslatedRules
		{
			get
			{
				if (this.untranslatedRulesResolved == null)
				{
					this.untranslatedRulesResolved = RulePack.GetRulesResolved(this.untranslatedRulesRaw, this.untranslatedRulesStrings, this.untranslatedRulesFiles);
					if (this.include != null)
					{
						foreach (RulePackDef rulePackDef in this.include)
						{
							this.untranslatedRulesResolved.AddRange(rulePackDef.UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.untranslatedRulesResolved;
			}
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x00195D68 File Offset: 0x00193F68
		public void PostLoad()
		{
			this.untranslatedRulesStrings = this.rulesStrings.ToList<string>();
			this.untranslatedRulesFiles = this.rulesFiles.ToList<string>();
			if (this.rulesRaw != null)
			{
				this.untranslatedRulesRaw = new List<Rule>();
				for (int i = 0; i < this.rulesRaw.Count; i++)
				{
					this.untranslatedRulesRaw.Add(this.rulesRaw[i].DeepCopy());
				}
			}
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x00195DDC File Offset: 0x00193FDC
		private static List<Rule> GetRulesResolved(List<Rule> rulesRaw, List<string> rulesStrings, List<string> rulesFiles)
		{
			List<Rule> list = new List<Rule>();
			for (int i = 0; i < rulesStrings.Count; i++)
			{
				try
				{
					Rule_String rule_String = new Rule_String(rulesStrings[i]);
					rule_String.Init();
					list.Add(rule_String);
				}
				catch (Exception ex)
				{
					string str = "Exception parsing grammar rule from ";
					string str2 = rulesStrings[i];
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			for (int j = 0; j < rulesFiles.Count; j++)
			{
				try
				{
					string[] array = rulesFiles[j].Split(new string[]
					{
						"->"
					}, StringSplitOptions.None);
					Rule_File rule_File = new Rule_File();
					rule_File.keyword = array[0].Trim();
					rule_File.path = array[1].Trim();
					rule_File.Init();
					list.Add(rule_File);
				}
				catch (Exception ex3)
				{
					string str4 = "Error initializing Rule_File ";
					string str5 = rulesFiles[j];
					string str6 = ": ";
					Exception ex4 = ex3;
					Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			if (rulesRaw != null)
			{
				for (int k = 0; k < rulesRaw.Count; k++)
				{
					try
					{
						rulesRaw[k].Init();
						list.Add(rulesRaw[k]);
					}
					catch (Exception ex5)
					{
						string str7 = "Error initializing rule ";
						string str8 = rulesRaw[k].ToStringSafe<Rule>();
						string str9 = ": ";
						Exception ex6 = ex5;
						Log.Error(str7 + str8 + str9 + ((ex6 != null) ? ex6.ToString() : null));
					}
				}
			}
			return list;
		}

		// Token: 0x040029E8 RID: 10728
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x040029E9 RID: 10729
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		// Token: 0x040029EA RID: 10730
		private List<Rule> rulesRaw;

		// Token: 0x040029EB RID: 10731
		public List<RulePackDef> include;

		// Token: 0x040029EC RID: 10732
		[Unsaved(false)]
		private List<Rule> rulesResolved;

		// Token: 0x040029ED RID: 10733
		[Unsaved(false)]
		private List<Rule> untranslatedRulesResolved;

		// Token: 0x040029EE RID: 10734
		[Unsaved(false)]
		private List<string> untranslatedRulesStrings;

		// Token: 0x040029EF RID: 10735
		[Unsaved(false)]
		private List<string> untranslatedRulesFiles;

		// Token: 0x040029F0 RID: 10736
		[Unsaved(false)]
		private List<Rule> untranslatedRulesRaw;
	}
}
