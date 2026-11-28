using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200085D RID: 2141
	public class MentalBreakWorker_WildDecree : MentalBreakWorker
	{
		// Token: 0x06004330 RID: 17200 RVA: 0x001A825C File Offset: 0x001A645C
		public override bool BreakCanOccur(Pawn pawn)
		{
			return base.BreakCanOccur(pawn) && pawn.IsColonist && !pawn.IsPrisoner && pawn.royalty != null && pawn.royalty.PossibleDecreeQuests.Any<QuestScriptDef>();
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x001A8294 File Offset: 0x001A6494
		public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			if (pawn.royalty == null)
			{
				return 0f;
			}
			float num = 0f;
			List<RoyalTitle> allTitlesInEffectForReading = pawn.royalty.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				num = Mathf.Max(num, allTitlesInEffectForReading[i].def.decreeMentalBreakCommonality);
			}
			return num;
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x001A82EC File Offset: 0x001A64EC
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.royalty.IssueDecree(true, reason);
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, false, causedByMood, null, true, false, false);
			}
			else if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, causedByMood, null, true, false, false);
			}
			return true;
		}
	}
}
