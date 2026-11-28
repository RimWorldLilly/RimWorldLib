using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002380 RID: 9088
	public static class CaravanBedUtility
	{
		// Token: 0x0600E203 RID: 57859 RVA: 0x0053D61C File Offset: 0x0053B81C
		public static bool InCaravanBed(this Pawn p)
		{
			return p.CurrentCaravanBed() != null;
		}

		// Token: 0x0600E204 RID: 57860 RVA: 0x0053D628 File Offset: 0x0053B828
		public static Building_Bed CurrentCaravanBed(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			if (caravan == null)
			{
				return null;
			}
			return caravan.beds.GetBedUsedBy(p);
		}

		// Token: 0x0600E205 RID: 57861 RVA: 0x0053D64D File Offset: 0x0053B84D
		public static bool WouldBenefitFromRestingInBed(Pawn p)
		{
			return !p.Dead && p.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x0600E206 RID: 57862 RVA: 0x0053D66C File Offset: 0x0053B86C
		public static string AppendUsingBedsLabel(string str, int bedCount)
		{
			string str2 = (bedCount == 1) ? "UsingBedroll".Translate() : "UsingBedrolls".Translate(bedCount);
			return str + " (" + str2 + ")";
		}
	}
}
