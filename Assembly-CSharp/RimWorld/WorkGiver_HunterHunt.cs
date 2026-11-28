using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C23 RID: 3107
	public class WorkGiver_HunterHunt : WorkGiver_Scanner
	{
		// Token: 0x06005889 RID: 22665 RVA: 0x0020B935 File Offset: 0x00209B35
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_HunterHunt.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_HunterHunt.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600588A RID: 22666 RVA: 0x0020B945 File Offset: 0x00209B45
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x0020B948 File Offset: 0x00209B48
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x0020B94B File Offset: 0x00209B4B
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) || WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn) || !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Hunt);
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x0020B97C File Offset: 0x00209B7C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2.AnimalOrWildMan() && pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) != null && (!HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn2) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && this.CanFindHuntingPosition(pawn, pawn2);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x0020BA33 File Offset: 0x00209C33
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Hunt, t);
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x0020BA48 File Offset: 0x00209C48
		public static bool HasHuntingWeapon(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon && p.equipment.PrimaryEq.PrimaryVerb.HarmsHealth() && !p.equipment.PrimaryEq.PrimaryVerb.UsesExplosiveProjectiles();
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x0020BAAC File Offset: 0x00209CAC
		private bool CanFindHuntingPosition(Pawn hunter, Pawn animal)
		{
			CastPositionRequest castPositionRequest = new CastPositionRequest
			{
				caster = hunter,
				target = animal,
				verb = hunter.TryGetAttackVerb(animal, false, false),
				wantCoverFromTarget = false
			};
			castPositionRequest.maxRangeFromTarget = (animal.Downed ? Mathf.Min(castPositionRequest.verb.EffectiveRange, (float)animal.RaceProps.executionRange) : Mathf.Max(castPositionRequest.verb.EffectiveRange * 0.95f, 1.42f));
			IntVec3 intVec;
			return CastPositionFinder.TryFindCastPosition(castPositionRequest, out intVec);
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x0020BB3C File Offset: 0x00209D3C
		public static bool HasShieldAndRangedWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && p.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def.IsShieldThatBlocksRanged)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
