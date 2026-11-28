using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E31 RID: 3633
	public class Thought_SharedBed : Thought_Situational
	{
		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06006156 RID: 24918 RVA: 0x0023287C File Offset: 0x00230A7C
		protected override float BaseMoodOffset
		{
			get
			{
				Pawn mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(this.pawn);
				if (mostDislikedNonPartnerBedOwner == null)
				{
					return 0f;
				}
				return Mathf.Min(0.05f * (float)this.pawn.relations.OpinionOf(mostDislikedNonPartnerBedOwner) - 5f, 0f);
			}
		}
	}
}
