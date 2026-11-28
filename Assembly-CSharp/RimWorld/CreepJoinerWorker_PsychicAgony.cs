using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001488 RID: 5256
	public class CreepJoinerWorker_PsychicAgony : BaseCreepJoinerWorker
	{
		// Token: 0x060080F7 RID: 33015 RVA: 0x002DFF14 File Offset: 0x002DE114
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			this.ApplyOrRefreshHediff(base.Pawn);
			foreach (Pawn pawn in base.Pawn.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Position.InHorDistOf(base.Pawn.Position, 14.9f))
				{
					this.ApplyOrRefreshHediff(pawn);
				}
			}
			SoundDefOf.PsychicBanshee.PlayOneShot(base.Pawn);
			MoteMaker.MakeAttachedOverlay(base.Pawn, ThingDefOf.Mote_PsychicBanshee, Vector3.zero, 1f, -1f);
		}

		// Token: 0x060080F8 RID: 33016 RVA: 0x002DFFD4 File Offset: 0x002DE1D4
		private void ApplyOrRefreshHediff(Pawn pawn)
		{
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.AgonyPulse, out hediff))
			{
				hediff.Severity = 0f;
			}
			else
			{
				hediff = pawn.health.AddHediff(HediffDefOf.AgonyPulse, null, null, null);
			}
			HediffWithComps hediffWithComps = hediff as HediffWithComps;
			HediffComp_Disappears hediffComp_Disappears = (hediffWithComps != null) ? hediffWithComps.GetComp<HediffComp_Disappears>() : null;
			if (hediffComp_Disappears != null)
			{
				hediffComp_Disappears.ticksToDisappear = CreepJoinerWorker_PsychicAgony.DisappearSeconds.RandomInRange * 60;
			}
		}

		// Token: 0x0400503B RID: 20539
		private const float Range = 14.9f;

		// Token: 0x0400503C RID: 20540
		private static readonly IntRange DisappearSeconds = new IntRange(2000, 4000);
	}
}
