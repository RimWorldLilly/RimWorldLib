using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001190 RID: 4496
	public class IncidentWorker_PsychicSoothe : IncidentWorker_PsychicEmanation
	{
		// Token: 0x06007099 RID: 28825 RVA: 0x00276854 File Offset: 0x00274A54
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicSootheGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x0600709A RID: 28826 RVA: 0x00276878 File Offset: 0x00274A78
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
			gameCondition_PsychicEmanation.gender = gender;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}
	}
}
