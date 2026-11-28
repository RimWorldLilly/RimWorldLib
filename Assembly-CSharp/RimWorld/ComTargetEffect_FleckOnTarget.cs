using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A11 RID: 6673
	public class ComTargetEffect_FleckOnTarget : CompTargetEffect
	{
		// Token: 0x17001D6B RID: 7531
		// (get) Token: 0x0600A7DE RID: 42974 RVA: 0x003BBB0C File Offset: 0x003B9D0C
		private CompProperties_TargetEffect_FleckOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_FleckOnTarget)this.props;
			}
		}

		// Token: 0x0600A7DF RID: 42975 RVA: 0x003BBB19 File Offset: 0x003B9D19
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.AttachedOverlay(target, this.Props.fleckDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
