using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001900 RID: 6400
	public class FocusStrengthOffset_Lit : FocusStrengthOffset
	{
		// Token: 0x0600A071 RID: 41073 RVA: 0x003968F4 File Offset: 0x00394AF4
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x0600A072 RID: 41074 RVA: 0x00396943 File Offset: 0x00394B43
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x0600A073 RID: 41075 RVA: 0x00396973 File Offset: 0x00394B73
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x0600A074 RID: 41076 RVA: 0x0039697C File Offset: 0x00394B7C
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			CompGlower compGlower = parent.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}
	}
}
