using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160A RID: 5642
	public abstract class GoodwillSituationWorker
	{
		// Token: 0x06008CA8 RID: 36008 RVA: 0x00331FF3 File Offset: 0x003301F3
		public virtual string GetPostProcessedLabel(Faction other)
		{
			return this.def.label;
		}

		// Token: 0x06008CA9 RID: 36009 RVA: 0x00332000 File Offset: 0x00330200
		public string GetPostProcessedLabelCap(Faction other)
		{
			return this.GetPostProcessedLabel(other).CapitalizeFirst(this.def);
		}

		// Token: 0x06008CAA RID: 36010 RVA: 0x00332014 File Offset: 0x00330214
		public virtual int GetMaxGoodwill(Faction other)
		{
			return 100;
		}

		// Token: 0x06008CAB RID: 36011 RVA: 0x00332018 File Offset: 0x00330218
		public virtual int GetNaturalGoodwillOffset(Faction other)
		{
			return 0;
		}

		// Token: 0x0400563B RID: 22075
		public GoodwillSituationDef def;
	}
}
