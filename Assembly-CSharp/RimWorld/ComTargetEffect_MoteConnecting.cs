using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A19 RID: 6681
	public class ComTargetEffect_MoteConnecting : CompTargetEffect
	{
		// Token: 0x17001D6E RID: 7534
		// (get) Token: 0x0600A7EE RID: 42990 RVA: 0x003BBD50 File Offset: 0x003B9F50
		private CompProperties_TargetEffect_MoteConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteConnecting)this.props;
			}
		}

		// Token: 0x0600A7EF RID: 42991 RVA: 0x003BBD5D File Offset: 0x003B9F5D
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeConnectingLine(user.DrawPos, target.DrawPos, this.Props.moteDef, user.Map, 1f);
			}
		}
	}
}
