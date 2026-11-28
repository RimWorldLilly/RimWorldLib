using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1D RID: 6685
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x0600A7F8 RID: 43000 RVA: 0x003BBE58 File Offset: 0x003BA058
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn, null);
			BodyPartRecord part;
			pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out part);
			BattleLogEntry_ItemUsed battleLogEntry_ItemUsed = new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed);
			hediff.combatLogEntry = new WeakReference<LogEntry>(battleLogEntry_ItemUsed);
			hediff.combatLogText = battleLogEntry_ItemUsed.ToGameStringFromPOV(null, false);
			pawn.health.AddHediff(hediff, part, null, null);
			Find.BattleLog.Add(battleLogEntry_ItemUsed);
		}

		// Token: 0x0600A7F9 RID: 43001 RVA: 0x003BBEF4 File Offset: 0x003BA0F4
		public override bool CanApplyOn(Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				if (pawn.kindDef.forceDeathOnDowned)
				{
					return false;
				}
				if (pawn.IsMutant && pawn.mutant.Def.psychicShockUntargetable)
				{
					return false;
				}
			}
			return true;
		}
	}
}
