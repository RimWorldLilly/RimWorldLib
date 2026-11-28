using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E18 RID: 3608
	public class Thought_Tale : Thought_SituationalSocial
	{
		// Token: 0x0600611B RID: 24859 RVA: 0x00231BF6 File Offset: 0x0022FDF6
		protected override ThoughtState CurrentStateInternal()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return false;
			}
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x00231C30 File Offset: 0x0022FE30
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			Tale latestTale = Find.TaleManager.GetLatestTale(this.def.taleDef, this.otherPawn);
			if (latestTale != null)
			{
				float num = 1f;
				if (latestTale.def.type == TaleType.Expirable)
				{
					float value = (float)latestTale.AgeTicks / (latestTale.def.expireDays * 60000f);
					num = Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, value);
				}
				return base.CurStage.baseOpinionOffset * num;
			}
			return 0f;
		}
	}
}
