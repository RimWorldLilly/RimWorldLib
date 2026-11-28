using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FD RID: 6397
	public class FocusStrengthOffset_ThroneSatisfiesRequirements : FocusStrengthOffset
	{
		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x0600A061 RID: 41057 RVA: 0x003966D1 File Offset: 0x003948D1
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A062 RID: 41058 RVA: 0x003966D4 File Offset: 0x003948D4
		public override string GetExplanation(Thing parent)
		{
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x0600A063 RID: 41059 RVA: 0x003966DD File Offset: 0x003948DD
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_SatisfiesTitle".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x0600A064 RID: 41060 RVA: 0x0039670D File Offset: 0x0039490D
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x0600A065 RID: 41061 RVA: 0x00396718 File Offset: 0x00394918
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (user == null)
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = user.royalty;
			bool? flag = (royalty != null) ? new bool?(royalty.AnyUnmetThroneroomRequirements()) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}
	}
}
