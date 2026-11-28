using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198E RID: 6542
	public class CompSpawnPawnsOnDamaged : ThingComp
	{
		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x0600A43D RID: 42045 RVA: 0x003AAD1C File Offset: 0x003A8F1C
		public CompProperties_SpawnPawnsOnDamaged Props
		{
			get
			{
				return (CompProperties_SpawnPawnsOnDamaged)this.props;
			}
		}

		// Token: 0x0600A43E RID: 42046 RVA: 0x003AAD2C File Offset: 0x003A8F2C
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!ModLister.CheckAnomaly("Pawn spawning pawns on hit"))
			{
				return;
			}
			if (!Rand.Chance(this.Props.chance))
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSpawned + this.Props.cooldownTicks)
			{
				return;
			}
			int randomInRange = this.Props.spawnPawnCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				PawnKindDef kind = this.Props.spawnPawnKindOptions.RandomElement<PawnKindDef>();
				Faction faction = this.parent.Faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(0f);
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				this.SpawnPawn(pawn, this.parent.PositionHeld, this.parent.MapHeld);
			}
			CompSpawnPawnsOnDamaged.tmpTakenCells.Clear();
			FleshbeastUtility.MeatSplatter(CompSpawnPawnsOnDamaged.BloodFilthCountRange.RandomInRange, this.parent.PositionHeld, this.parent.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
			FilthMaker.TryMakeFilth(this.parent.PositionHeld, this.parent.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
			this.lastSpawned = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A43F RID: 42047 RVA: 0x003AAED4 File Offset: 0x003A90D4
		private void SpawnPawn(Pawn pawn, IntVec3 position, Map map)
		{
			GenSpawn.Spawn(pawn, position, map, WipeMode.VanishOrMoveAside);
			IntVec3 intVec;
			if (CellFinder.TryFindRandomSpawnCellForPawnNear(position, map, out intVec, 5, (IntVec3 c) => !CompSpawnPawnsOnDamaged.tmpTakenCells.Contains(c) && GenSight.LineOfSight(position, c, map, true, null, 0, 0)))
			{
				pawn.rotationTracker.FaceCell(intVec);
				CompSpawnPawnsOnDamaged.tmpTakenCells.Add(intVec);
				PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, null, null, default(LocalTargetInfo));
				if (pawnFlyer != null)
				{
					GenSpawn.Spawn(pawnFlyer, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600A440 RID: 42048 RVA: 0x003AAF78 File Offset: 0x003A9178
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSpawned, "lastSpawned", -99999, false);
		}

		// Token: 0x04006115 RID: 24853
		private static readonly IntRange BloodFilthCountRange = new IntRange(1, 2);

		// Token: 0x04006116 RID: 24854
		private const int InitialJumpDistance = 5;

		// Token: 0x04006117 RID: 24855
		private int lastSpawned = -99999;

		// Token: 0x04006118 RID: 24856
		private static List<IntVec3> tmpTakenCells = new List<IntVec3>();
	}
}
