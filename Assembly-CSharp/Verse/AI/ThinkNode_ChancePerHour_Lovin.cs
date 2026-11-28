using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008BC RID: 2236
	public class ThinkNode_ChancePerHour_Lovin : ThinkNode_ChancePerHour
	{
		// Token: 0x06004595 RID: 17813 RVA: 0x001B7130 File Offset: 0x001B5330
		protected override float MtbHours(Pawn pawn)
		{
			if (pawn.CurrentBed() == null)
			{
				return -1f;
			}
			Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
			if (partnerInMyBed == null)
			{
				return -1f;
			}
			return LovePartnerRelationUtility.GetLovinMtbHours(pawn, partnerInMyBed);
		}
	}
}
