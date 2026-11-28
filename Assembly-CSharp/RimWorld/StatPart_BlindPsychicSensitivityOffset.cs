using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F75 RID: 8053
	public class StatPart_BlindPsychicSensitivityOffset : StatPart
	{
		// Token: 0x0600CCF9 RID: 52473 RVA: 0x004B6AE4 File Offset: 0x004B4CE4
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetPsychicOffset(req.Thing, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CCFA RID: 52474 RVA: 0x004B6B08 File Offset: 0x004B4D08
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (req.HasThing && this.TryGetPsychicOffset(req.Thing, out f))
			{
				return "StatsReport_BlindPsychicSensitivityOffset".Translate() + (": +" + f.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CCFB RID: 52475 RVA: 0x004B6B58 File Offset: 0x004B4D58
		private bool TryGetPsychicOffset(Thing t, out float offset)
		{
			offset = 0f;
			Pawn pawn = t as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!this.ConsideredBlind(pawn) || pawn.Ideo == null)
			{
				return false;
			}
			foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
			{
				offset += precept.def.blindPsychicSensitivityOffset;
			}
			return !Mathf.Approximately(offset, 0f);
		}

		// Token: 0x0600CCFC RID: 52476 RVA: 0x004B6BF0 File Offset: 0x004B4DF0
		private bool ConsideredBlind(Pawn pawn)
		{
			foreach (BodyPartRecord part in pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource))
			{
				if (!pawn.health.hediffSet.PartIsMissing(part))
				{
					return false;
				}
			}
			return true;
		}
	}
}
