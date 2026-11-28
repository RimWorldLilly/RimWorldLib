using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001688 RID: 5768
	public class RitualBehaviorWorker_AnimaLinking : RitualBehaviorWorker
	{
		// Token: 0x060090A0 RID: 37024 RVA: 0x003511C3 File Offset: 0x0034F3C3
		public RitualBehaviorWorker_AnimaLinking()
		{
		}

		// Token: 0x060090A1 RID: 37025 RVA: 0x003511CB File Offset: 0x0034F3CB
		public RitualBehaviorWorker_AnimaLinking(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x060090A2 RID: 37026 RVA: 0x003511D4 File Offset: 0x0034F3D4
		public override string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			int count = assignments.SpectatorsForReading.Count;
			float value = RitualOutcomeEffectWorker_AnimaTreeLinking.RestoredGrassFromQuality.Evaluate(quality);
			TaggedString taggedString = "AnimaLinkingExplanationBase".Translate(count, value);
			if (assignments.ExtraRequiredPawnsForReading.Any<Pawn>())
			{
				TaggedString psylinkAffectedByTraitsNegativelyWarning = RoyalTitleUtility.GetPsylinkAffectedByTraitsNegativelyWarning(assignments.ExtraRequiredPawnsForReading.FirstOrDefault<Pawn>());
				if (psylinkAffectedByTraitsNegativelyWarning.RawText != null)
				{
					taggedString += "\n\n" + psylinkAffectedByTraitsNegativelyWarning.Resolve();
				}
			}
			return taggedString;
		}

		// Token: 0x060090A3 RID: 37027 RVA: 0x00351258 File Offset: 0x0034F458
		public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			int count = assignments.SpectatorsForReading.Count;
			return Mathf.RoundToInt((float)ritual.behavior.def.durationTicks.max / RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)(count + 1))).ToStringTicksToPeriod(false, false, true, true, false);
		}
	}
}
