using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x02000930 RID: 2352
	public class PsychicRitualToil_SkipAbductionPlayer : PsychicRitualToil
	{
		// Token: 0x0600480B RID: 18443 RVA: 0x001C462C File Offset: 0x001C282C
		protected PsychicRitualToil_SkipAbductionPlayer()
		{
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x001C4634 File Offset: 0x001C2834
		public PsychicRitualToil_SkipAbductionPlayer(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x001C4644 File Offset: 0x001C2844
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn);
			}
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x001C4670 File Offset: 0x001C2870
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
		{
			IntVec3 cell = psychicRitual.assignments.Target.Cell;
			bool value = false;
			IEnumerable<Pawn> source = psychicRitual.Map.attackTargetsCache.TargetsHostileToColony.Where(delegate(IAttackTarget t)
			{
				Pawn pawn4 = t.Thing as Pawn;
				return pawn4 != null && pawn4.RaceProps.Humanlike && !pawn4.IsSubhuman && pawn4.Faction != Faction.OfPlayer && !t.ThreatDisabled(invoker) && !pawn4.IsOnHoldingPlatform;
			}).Cast<Pawn>().ToArray<Pawn>();
			Pawn pawn = null;
			Pawn pawn2;
			if (source.TryRandomElement(out pawn2))
			{
				pawn = pawn2;
				psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(pawn, pawn.Map, 1f), pawn.PositionHeld, 60);
				SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(pawn.PositionHeld, pawn.Map, false));
				SkipUtility.SkipTo(pawn, cell, psychicRitual.Map);
			}
			else
			{
				List<Pawn> list = Find.WorldPawns.AllPawnsAlive.Where(delegate(Pawn p)
				{
					if (p.RaceProps.Humanlike && p.HostileTo(invoker) && !p.IsSubhuman)
					{
						Faction faction2 = p.Faction;
						return ((faction2 != null) ? faction2.leader : null) == p;
					}
					return false;
				}).ToList<Pawn>();
				List<Pawn> list2 = Find.WorldPawns.AllPawnsAlive.Where(delegate(Pawn p)
				{
					if (p.RaceProps.Humanlike && p.HostileTo(invoker) && !p.IsSubhuman)
					{
						Faction faction2 = p.Faction;
						return ((faction2 != null) ? faction2.leader : null) != p;
					}
					return false;
				}).ToList<Pawn>();
				float chance = 0.4f * Mathf.Clamp01((float)list2.Count / 20f);
				Pawn pawn3 = null;
				if (Rand.Chance(0.08f) && !list.NullOrEmpty<Pawn>())
				{
					list.TryRandomElement(out pawn3);
				}
				else if (Rand.Chance(chance))
				{
					list2.TryRandomElement(out pawn3);
				}
				Faction faction;
				if (pawn3 != null)
				{
					pawn = (Pawn)GenSpawn.Spawn(pawn3, cell, psychicRitual.Map, WipeMode.Vanish);
					value = true;
				}
				else if ((from f in Find.FactionManager.AllFactionsVisible
				where f.def.humanlikeFaction && f.HostileTo(Faction.OfPlayer)
				select f).TryRandomElement(out faction))
				{
					pawn = (Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(faction.RandomPawnKind(), faction, PawnGenerationContext.NonPlayer, new PlanetTile?(psychicRitual.Map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), cell, psychicRitual.Map, WipeMode.Vanish);
				}
			}
			if (pawn == null)
			{
				Log.Error("Could not find target pawn for player's skip abduction ritual.");
				return;
			}
			if (pawn.Dead)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Skip abduction ritual abducted a dead pawn. World pawn abducted: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (pawn.IsSubhuman)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Skip abduction ritual abducted a mutant. World pawn abducted: ");
				defaultInterpolatedStringHandler2.AppendFormatted<bool>(value);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(cell, psychicRitual.Map, 1f), cell, 60);
			SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map, false));
			int ticksToDisappear = Mathf.RoundToInt(((PsychicRitualDef_SkipAbductionPlayer)psychicRitual.def).comaDurationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent) * 60000f);
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, pawn, null);
			hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = ticksToDisappear;
			pawn.health.AddHediff(hediff, null, null, null);
			Pawn_NeedsTracker needs = pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.PsychicRitualVictim, null, null);
						}
					}
				}
			}
			TaggedString text = "SkipAbductionPlayerCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), pawn.Named("TARGET"), pawn.Faction.Named("FACTION"));
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, LetterDefOf.NeutralEvent, new LookTargets(pawn), null, null, null, null, 0, true);
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x001C4AAB File Offset: 0x001C2CAB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E8D RID: 11917
		private const float ChanceForLeader = 0.08f;

		// Token: 0x04002E8E RID: 11918
		private const float ChanceForWorldPawn = 0.4f;

		// Token: 0x04002E8F RID: 11919
		public PsychicRitualRoleDef invokerRole;
	}
}
