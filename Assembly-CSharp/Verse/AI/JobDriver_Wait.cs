using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200081F RID: 2079
	public class JobDriver_Wait : JobDriver
	{
		// Token: 0x0600415A RID: 16730 RVA: 0x0019EC8C File Offset: 0x0019CE8C
		public override string GetReport()
		{
			if (this.job.def == JobDefOf.Wait_Combat && this.pawn.RaceProps.Humanlike && this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return "ReportStanding".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x0019ECE1 File Offset: 0x0019CEE1
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x0019ECE4 File Offset: 0x0019CEE4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Wait.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Wait.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0019ECF4 File Offset: 0x0019CEF4
		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x0019ECF6 File Offset: 0x0019CEF6
		public override void Notify_StanceChanged()
		{
			if (this.pawn.stances.curStance is Stance_Mobile)
			{
				this.CheckForAutoAttack();
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0019ED18 File Offset: 0x0019CF18
		private void CheckForAutoAttack()
		{
			if (!this.pawn.kindDef.canMeleeAttack)
			{
				return;
			}
			if (this.pawn.Downed)
			{
				return;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return;
			}
			if (this.pawn.IsCarryingPawn(null))
			{
				return;
			}
			if (!this.pawn.IsPlayerControlled && this.pawn.IsPsychologicallyInvisible())
			{
				return;
			}
			if (this.pawn.IsShambler)
			{
				return;
			}
			this.collideWithPawns = false;
			bool flag = !this.pawn.WorkTagIsDisabled(WorkTags.Violent);
			bool flag2 = this.pawn.RaceProps.ToolUser && this.pawn.Faction == Faction.OfPlayer && !this.pawn.WorkTagIsDisabled(WorkTags.Firefighting);
			if (flag || flag2)
			{
				Fire fire = null;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.pawn.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (flag && this.pawn.kindDef.canMeleeAttack)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.ThreatDisabled(this.pawn) && this.pawn.HostileTo(pawn))
								{
									CompActivity comp = pawn.GetComp<CompActivity>();
									if ((comp == null || comp.IsActive) && !this.pawn.ThreatDisabledBecauseNonAggressiveRoamer(pawn) && GenHostility.IsActiveThreatTo(pawn, this.pawn.Faction, false, false))
									{
										this.pawn.meleeVerbs.TryMeleeAttack(pawn, null, false);
										this.collideWithPawns = true;
										return;
									}
								}
							}
							if (flag2)
							{
								Fire fire2 = thingList[j] as Fire;
								if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != this.pawn))
								{
									fire = fire2;
								}
							}
						}
					}
				}
				if (fire != null && (!this.pawn.InMentalState || this.pawn.MentalState.def.allowBeatfire))
				{
					this.pawn.natives.TryBeatFire(fire);
					return;
				}
				if (flag && this.job.canUseRangedWeapon && this.job.def == JobDefOf.Wait_Combat && (this.pawn.drafter == null || this.pawn.drafter.FireAtWill))
				{
					Verb currentEffectiveVerb = this.pawn.CurrentEffectiveVerb;
					if (currentEffectiveVerb == null || currentEffectiveVerb.verbProps.IsMeleeAttack)
					{
						return;
					}
					TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
					if (currentEffectiveVerb.IsIncendiary_Ranged())
					{
						targetScanFlags |= TargetScanFlags.NeedNonBurning;
					}
					Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this.pawn, targetScanFlags, null, 0f, 9999f);
					if (thing != null)
					{
						this.pawn.TryStartAttack(thing);
						this.collideWithPawns = true;
					}
				}
			}
		}

		// Token: 0x04002B26 RID: 11046
		private const int TargetSearchInterval = 4;
	}
}
