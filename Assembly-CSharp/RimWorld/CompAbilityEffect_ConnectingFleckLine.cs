using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D5 RID: 5077
	public class CompAbilityEffect_ConnectingFleckLine : CompAbilityEffect
	{
		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x06007E5F RID: 32351 RVA: 0x002D290F File Offset: 0x002D0B0F
		public new CompProperties_AbilityConnectingFleckLine Props
		{
			get
			{
				return (CompProperties_AbilityConnectingFleckLine)this.props;
			}
		}

		// Token: 0x06007E60 RID: 32352 RVA: 0x002D291C File Offset: 0x002D0B1C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.ConnectingLine(this.parent.pawn.DrawPos, target.CenterVector3, this.Props.fleckDef, this.parent.pawn.Map, 1f);
			}
		}
	}
}
