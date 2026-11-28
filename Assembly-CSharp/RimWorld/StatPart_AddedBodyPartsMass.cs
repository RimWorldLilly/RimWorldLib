using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6B RID: 8043
	public class StatPart_AddedBodyPartsMass : StatPart
	{
		// Token: 0x0600CCCF RID: 52431 RVA: 0x004B62B0 File Offset: 0x004B44B0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CCD0 RID: 52432 RVA: 0x004B62D0 File Offset: 0x004B44D0
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetValue(req, out num) && num != 0f)
			{
				return "StatsReport_AddedBodyPartsMass".Translate() + ": " + num.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0600CCD1 RID: 52433 RVA: 0x004B6318 File Offset: 0x004B4518
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => StatPart_AddedBodyPartsMass.GetAddedBodyPartsMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x0600CCD2 RID: 52434 RVA: 0x004B636C File Offset: 0x004B456C
		private static float GetAddedBodyPartsMass(Pawn p)
		{
			float num = 0f;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_AddedPart hediff_AddedPart = hediffs[i] as Hediff_AddedPart;
				if (hediff_AddedPart != null && hediff_AddedPart.def.spawnThingOnRemoved != null)
				{
					num += hediff_AddedPart.def.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.Mass, null) * 0.9f;
				}
			}
			return num;
		}

		// Token: 0x040082E6 RID: 33510
		private const float AddedBodyPartMassFactor = 0.9f;
	}
}
