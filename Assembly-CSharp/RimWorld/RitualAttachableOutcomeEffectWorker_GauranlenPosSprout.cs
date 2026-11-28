using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200165A RID: 5722
	public class RitualAttachableOutcomeEffectWorker_GauranlenPosSprout : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FEC RID: 36844 RVA: 0x0034E498 File Offset: 0x0034C698
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (this.CanApplyNow(jobRitual.Ritual, jobRitual.Map))
			{
				IncidentParms parms = new IncidentParms
				{
					target = jobRitual.Map,
					customLetterText = IncidentDefOf.GauranlenPodSpawn.letterText + "\n\n" + "RitualAttachedOutcome_GauranlenTreePod_ExtraDesc".Translate(jobRitual.RitualLabel)
				};
				if (IncidentDefOf.GauranlenPodSpawn.Worker.TryExecute(parms))
				{
					extraOutcomeDesc = this.def.letterInfoText;
				}
			}
		}

		// Token: 0x06008FED RID: 36845 RVA: 0x0034E530 File Offset: 0x0034C730
		public override AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			if (!IncidentWorker_GauranlenPodSpawn.IsGoodBiome(map.Biome))
			{
				return "RitualAttachedOutcomeCantApply_ExtremeBiome".Translate();
			}
			IntVec3 intVec;
			if (!IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out intVec))
			{
				return "RitualAttachedOutcomeCantApply_NoValidSpot".Translate();
			}
			return base.CanApplyNow(ritual, map);
		}
	}
}
