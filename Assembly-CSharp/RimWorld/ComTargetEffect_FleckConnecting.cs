using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0F RID: 6671
	public class ComTargetEffect_FleckConnecting : CompTargetEffect
	{
		// Token: 0x17001D6A RID: 7530
		// (get) Token: 0x0600A7DA RID: 42970 RVA: 0x003BBAA9 File Offset: 0x003B9CA9
		private CompProperties_TargetEffect_FleckConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_FleckConnecting)this.props;
			}
		}

		// Token: 0x0600A7DB RID: 42971 RVA: 0x003BBAB6 File Offset: 0x003B9CB6
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.ConnectingLine(user.DrawPos, target.DrawPos, this.Props.fleckDef, user.Map, 1f);
			}
		}
	}
}
