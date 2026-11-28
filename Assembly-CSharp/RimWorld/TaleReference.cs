using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D8 RID: 6104
	public class TaleReference : IExposable
	{
		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x06009850 RID: 38992 RVA: 0x00373EAB File Offset: 0x003720AB
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x06009851 RID: 38993 RVA: 0x00373EB3 File Offset: 0x003720B3
		public bool IsNullTale
		{
			get
			{
				return this.tale == null;
			}
		}

		// Token: 0x06009852 RID: 38994 RVA: 0x00373EBE File Offset: 0x003720BE
		public TaleReference()
		{
		}

		// Token: 0x06009853 RID: 38995 RVA: 0x00373EC6 File Offset: 0x003720C6
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x06009854 RID: 38996 RVA: 0x00373EE6 File Offset: 0x003720E6
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x06009855 RID: 38997 RVA: 0x00373F0B File Offset: 0x0037210B
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x06009856 RID: 38998 RVA: 0x00373F27 File Offset: 0x00372127
		public TaggedString GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude, List<Rule> extraRules = null, Dictionary<string, string> extraConstants = null)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, new List<RulePackDef>
			{
				extraInclude
			}, extraRules, extraConstants);
		}

		// Token: 0x06009857 RID: 38999 RVA: 0x00373F4C File Offset: 0x0037214C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"TaleReference(tale=",
				(this.tale == null) ? "null" : this.tale.ToString(),
				", seed=",
				this.seed.ToString(),
				")"
			});
		}

		// Token: 0x04005B20 RID: 23328
		private Tale tale;

		// Token: 0x04005B21 RID: 23329
		private int seed;
	}
}
