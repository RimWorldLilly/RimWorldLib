using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191F RID: 6431
	public class CompObelisk_Duplicator : CompObelisk_ExplodingSpawner
	{
		// Token: 0x0600A154 RID: 41300 RVA: 0x0039A800 File Offset: 0x00398A00
		public override void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false)
		{
			IntVec3 targetCell;
			if (!CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 16, new Predicate<IntVec3>(this.IsValidSpawnCell), out targetCell, 100))
			{
				return;
			}
			Pawn pawn;
			if (!AnomalyUtility.TryDuplicatePawn(interactor, targetCell, this.parent.Map, out pawn, null, false, true, true))
			{
				if (triggeredByPlayer)
				{
					this.parent.GetComp<CompObeliskTriggerInteractor>().ResetCooldown(false);
					Find.LetterStack.ReceiveLetter("ObeliskDuplicationFailedLetterLabel".Translate(), "ObeliskDuplicationFailedLetter".Translate(interactor.Named("PAWN")), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
				}
				return;
			}
			this.lastInteractionEffectTick = Find.TickManager.TicksGame;
			if (pawn.Faction == Faction.OfEntities)
			{
				base.Lord.AddPawn(pawn);
				return;
			}
			Find.LetterStack.ReceiveLetter("ObeliskDuplicationLetterLabel".Translate(), "ObeliskDuplicationLetter".Translate(interactor.Named("PAWN")), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
			Messages.Message("ObeliskDuplicationSuccessMessage".Translate(interactor.Named("PAWN")), interactor, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600A155 RID: 41301 RVA: 0x0039A938 File Offset: 0x00398B38
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			this.pointsRemaining *= 0.6f;
			int num = Mathf.CeilToInt(this.pointsRemaining / 35f);
			this.outsideBlastCount = Mathf.CeilToInt((float)num * 0.7f);
			Find.LetterStack.ReceiveLetter("DuplicatorObeliskLetterLabel".Translate(), "DuplicatorObeliskLetter".Translate(), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x0600A156 RID: 41302 RVA: 0x0039A9B8 File Offset: 0x00398BB8
		public override void CompTick()
		{
			base.CompTick();
			if (!this.activated)
			{
				return;
			}
			if (base.ActivityComp.Deactivated)
			{
				return;
			}
			if (this.explodeTick > 0)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.nextSpawnTick)
			{
				return;
			}
			if (!this.warmupComplete)
			{
				return;
			}
			this.nextSpawnTick = Find.TickManager.TicksGame + this.SpawnIntervalTicks.RandomInRange;
			CompObelisk_Duplicator.tmpDuplicateCandidates.Clear();
			bool flag = false;
			if (Find.Storyteller.difficulty.childRaidersAllowed)
			{
				CompObelisk_Duplicator.tmpDuplicateCandidates = (from p in this.parent.Map.mapPawns.FreeColonistsSpawned
				where p.ageTracker.AgeBiologicalYears >= 10
				select p).ToList<Pawn>();
				if (CompObelisk_Duplicator.tmpDuplicateCandidates.Count == 0)
				{
					CompObelisk_Duplicator.tmpDuplicateCandidates = (from p in Find.WorldPawns.AllPawnsAlive
					where p.ageTracker.AgeBiologicalYears >= 10
					select p).ToList<Pawn>();
					flag = true;
				}
			}
			else
			{
				CompObelisk_Duplicator.tmpDuplicateCandidates = this.parent.Map.mapPawns.FreeAdultColonistsSpawned;
				if (CompObelisk_Duplicator.tmpDuplicateCandidates.Count == 0)
				{
					CompObelisk_Duplicator.tmpDuplicateCandidates = (from p in Find.WorldPawns.AllPawnsAlive
					where p.DevelopmentalStage.Adult()
					select p).ToList<Pawn>();
					flag = true;
				}
			}
			if (CompObelisk_Duplicator.tmpDuplicateCandidates.Count == 0)
			{
				return;
			}
			Pawn pawn = CompObelisk_Duplicator.tmpDuplicateCandidates.RandomElement<Pawn>();
			IntVec3 root;
			int squareRadius;
			if (this.spawnedCount < this.outsideBlastCount)
			{
				root = (flag ? this.parent.Position : pawn.Position);
				squareRadius = 20;
			}
			else
			{
				root = this.parent.Position;
				squareRadius = Mathf.FloorToInt(4.9f);
			}
			IntVec3 targetCell;
			Pawn pawn2;
			if (CellFinder.TryFindRandomCellNear(root, this.parent.Map, squareRadius, new Predicate<IntVec3>(this.IsValidSpawnCell), out targetCell, 100) && AnomalyUtility.TryDuplicatePawn(pawn, targetCell, this.parent.Map, out pawn2, Faction.OfEntities, false, false, false))
			{
				base.Lord.AddPawn(pawn2);
				EffecterDefOf.ObeliskSpark.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
				this.pointsRemaining -= CompObelisk_Duplicator.CombatPointsSpawnPointsCurve.Evaluate(this.totalCombatPoints);
				this.spawnedCount++;
				Messages.Message("MessageHostileDuplicate".Translate(pawn), pawn2, MessageTypeDefOf.ThreatBig, true);
			}
			if (this.pointsRemaining <= 0f)
			{
				base.PrepareExplosion();
			}
		}

		// Token: 0x0600A157 RID: 41303 RVA: 0x0039AC74 File Offset: 0x00398E74
		private bool IsValidSpawnCell(IntVec3 pos)
		{
			return pos.Standable(this.parent.Map) && pos.Walkable(this.parent.Map) && !pos.Fogged(this.parent.Map);
		}

		// Token: 0x0600A158 RID: 41304 RVA: 0x0039ACB2 File Offset: 0x00398EB2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.outsideBlastCount, "outsideBlastCount", 0, false);
			Scribe_Values.Look<int>(ref this.spawnedCount, "spawnedCount", 0, false);
		}

		// Token: 0x04005F4C RID: 24396
		private int outsideBlastCount;

		// Token: 0x04005F4D RID: 24397
		private int spawnedCount;

		// Token: 0x04005F4E RID: 24398
		private static List<Pawn> tmpDuplicateCandidates = new List<Pawn>();

		// Token: 0x04005F4F RID: 24399
		private const float DuplicateCombatPoints = 35f;

		// Token: 0x04005F50 RID: 24400
		private const float BlastRadius = 4.9f;

		// Token: 0x04005F51 RID: 24401
		private const float PercentSpawnedOutsideBlast = 0.7f;

		// Token: 0x04005F52 RID: 24402
		private const float CombatPointsFactor = 0.6f;

		// Token: 0x04005F53 RID: 24403
		private const int MinAgeYears = 10;

		// Token: 0x04005F54 RID: 24404
		private static readonly SimpleCurve CombatPointsSpawnPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(500f, 50f),
				true
			},
			{
				new CurvePoint(1000f, 100f),
				true
			},
			{
				new CurvePoint(5000f, 200f),
				true
			}
		};
	}
}
