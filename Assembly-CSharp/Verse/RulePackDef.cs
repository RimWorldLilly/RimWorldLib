using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001B6 RID: 438
	public class RulePackDef : Def
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x000404A8 File Offset: 0x0003E6A8
		public List<Rule> RulesPlusIncludes
		{
			get
			{
				if (this.cachedRules == null)
				{
					this.cachedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedRules.AddRange(this.rulePack.Rules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedRules.AddRange(this.include[i].RulesPlusIncludes);
						}
					}
				}
				return this.cachedRules;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00040528 File Offset: 0x0003E728
		public List<Rule> UntranslatedRulesPlusIncludes
		{
			get
			{
				if (this.cachedUntranslatedRules == null)
				{
					this.cachedUntranslatedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedUntranslatedRules.AddRange(this.rulePack.UntranslatedRules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedUntranslatedRules.AddRange(this.include[i].UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.cachedUntranslatedRules;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x000405A6 File Offset: 0x0003E7A6
		public List<Rule> RulesImmediate
		{
			get
			{
				RulePack rulePack = this.rulePack;
				if (rulePack == null)
				{
					return null;
				}
				return rulePack.Rules;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x000405B9 File Offset: 0x0003E7B9
		public List<Rule> UntranslatedRulesImmediate
		{
			get
			{
				RulePack rulePack = this.rulePack;
				if (rulePack == null)
				{
					return null;
				}
				return rulePack.UntranslatedRules;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x000405CC File Offset: 0x0003E7CC
		public string FirstRuleKeyword
		{
			get
			{
				List<Rule> rulesPlusIncludes = this.RulesPlusIncludes;
				if (!rulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return rulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x000405FC File Offset: 0x0003E7FC
		public string FirstUntranslatedRuleKeyword
		{
			get
			{
				List<Rule> untranslatedRulesPlusIncludes = this.UntranslatedRulesPlusIncludes;
				if (!untranslatedRulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return untranslatedRulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0004062A File Offset: 0x0003E82A
		public override IEnumerable<string> ConfigErrors()
		{
			RulePackDef.<ConfigErrors>d__17 <ConfigErrors>d__ = new RulePackDef.<ConfigErrors>d__17(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0004063A File Offset: 0x0003E83A
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x04000B7B RID: 2939
		public List<RulePackDef> include;

		// Token: 0x04000B7C RID: 2940
		private RulePack rulePack;

		// Token: 0x04000B7D RID: 2941
		public bool directTestable;

		// Token: 0x04000B7E RID: 2942
		[Unsaved(false)]
		private List<Rule> cachedRules;

		// Token: 0x04000B7F RID: 2943
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}
