using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x020007AA RID: 1962
	public struct GrammarRequest
	{
		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06003F3D RID: 16189 RVA: 0x00194B15 File Offset: 0x00192D15
		public List<Rule> RulesAllowNull
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x00194B1D File Offset: 0x00192D1D
		public List<Rule> Rules
		{
			get
			{
				if (this.rules == null)
				{
					this.rules = new List<Rule>();
				}
				return this.rules;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06003F3F RID: 16191 RVA: 0x00194B38 File Offset: 0x00192D38
		public List<RulePack> IncludesBareAllowNull
		{
			get
			{
				return this.includesBare;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x00194B40 File Offset: 0x00192D40
		public List<RulePack> IncludesBare
		{
			get
			{
				if (this.includesBare == null)
				{
					this.includesBare = new List<RulePack>();
				}
				return this.includesBare;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003F41 RID: 16193 RVA: 0x00194B5B File Offset: 0x00192D5B
		public List<RulePackDef> IncludesAllowNull
		{
			get
			{
				return this.includes;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x00194B63 File Offset: 0x00192D63
		public List<RulePackDef> Includes
		{
			get
			{
				if (this.includes == null)
				{
					this.includes = new List<RulePackDef>();
				}
				return this.includes;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06003F43 RID: 16195 RVA: 0x00194B7E File Offset: 0x00192D7E
		public Dictionary<string, string> ConstantsAllowNull
		{
			get
			{
				return this.constants;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x00194B86 File Offset: 0x00192D86
		public Dictionary<string, string> Constants
		{
			get
			{
				if (this.constants == null)
				{
					this.constants = new Dictionary<string, string>();
				}
				return this.constants;
			}
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x00194BA4 File Offset: 0x00192DA4
		public bool HasRule(string keyword)
		{
			GrammarRequest.<>c__DisplayClass22_0 CS$<>8__locals1 = new GrammarRequest.<>c__DisplayClass22_0();
			CS$<>8__locals1.keyword = keyword;
			return (this.rules != null && this.rules.Any(new Predicate<Rule>(CS$<>8__locals1.<HasRule>g__HasTargetRule|0))) || (this.includes != null && this.includes.Any((RulePackDef i) => i.RulesPlusIncludes.Any(new Predicate<Rule>(base.<HasRule>g__HasTargetRule|0)))) || (this.includesBare != null && this.includesBare.Any((RulePack rp) => rp.Rules.Any(new Predicate<Rule>(base.<HasRule>g__HasTargetRule|0))));
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x00194C28 File Offset: 0x00192E28
		public void Clear()
		{
			if (this.rules != null)
			{
				this.rules.Clear();
			}
			if (this.includesBare != null)
			{
				this.includesBare.Clear();
			}
			if (this.includes != null)
			{
				this.includes.Clear();
			}
			if (this.constants != null)
			{
				this.constants.Clear();
			}
		}

		// Token: 0x040029D5 RID: 10709
		private List<Rule> rules;

		// Token: 0x040029D6 RID: 10710
		private List<RulePack> includesBare;

		// Token: 0x040029D7 RID: 10711
		private List<RulePackDef> includes;

		// Token: 0x040029D8 RID: 10712
		private Dictionary<string, string> constants;

		// Token: 0x040029D9 RID: 10713
		public GrammarRequest.ICustomizer customizer;

		// Token: 0x02002AB1 RID: 10929
		public interface ICustomizer
		{
			// Token: 0x06010739 RID: 67385
			int PriorityBucket(Rule rule);

			// Token: 0x0601073A RID: 67386
			void Notify_RuleUsed(Rule rule);

			// Token: 0x0601073B RID: 67387
			bool ValidateRule(Rule rule);
		}
	}
}
