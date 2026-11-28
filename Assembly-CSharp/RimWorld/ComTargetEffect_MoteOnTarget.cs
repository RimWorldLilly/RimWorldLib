using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1B RID: 6683
	public class ComTargetEffect_MoteOnTarget : CompTargetEffect
	{
		// Token: 0x17001D6F RID: 7535
		// (get) Token: 0x0600A7F2 RID: 42994 RVA: 0x003BBDB4 File Offset: 0x003B9FB4
		private CompProperties_TargetEffect_MoteOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteOnTarget)this.props;
			}
		}

		// Token: 0x0600A7F3 RID: 42995 RVA: 0x003BBDC1 File Offset: 0x003B9FC1
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeAttachedOverlay(target, this.Props.moteDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
