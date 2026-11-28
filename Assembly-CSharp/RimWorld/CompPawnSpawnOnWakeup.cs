using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200192C RID: 6444
	public class CompPawnSpawnOnWakeup : ThingComp
	{
		// Token: 0x17001BEF RID: 7151
		// (get) Token: 0x0600A1A9 RID: 41385 RVA: 0x0039C76F File Offset: 0x0039A96F
		private CompProperties_PawnSpawnOnWakeup Props
		{
			get
			{
				return (CompProperties_PawnSpawnOnWakeup)this.props;
			}
		}

		// Token: 0x17001BF0 RID: 7152
		// (get) Token: 0x0600A1AA RID: 41386 RVA: 0x0039C77C File Offset: 0x0039A97C
		public bool CanSpawn
		{
			get
			{
				return this.points > 0f;
			}
		}

		// Token: 0x0600A1AB RID: 41387 RVA: 0x0039C78B File Offset: 0x0039A98B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.points = this.Props.points.RandomInRange;
		}

		// Token: 0x0600A1AC RID: 41388 RVA: 0x0039C7AC File Offset: 0x0039A9AC
		public override void CompTick()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
			if (this.points == 0f)
			{
				return;
			}
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			bool flag = comp == null || comp.Awake;
			if (this.points > 0f && flag && this.parent.Spawned)
			{
				this.Spawn();
			}
		}

		// Token: 0x0600A1AD RID: 41389 RVA: 0x0039C83C File Offset: 0x0039AA3C
		private IntVec3 GetSpawnPosition()
		{
			if (!this.Props.dropInPods)
			{
				return this.parent.Position;
			}
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!DropCellFinder.IsGoodDropSpot(c, this.parent.MapHeld, false, true, true))
				{
					return false;
				}
				float num = c.DistanceTo(this.parent.Position);
				return num >= (float)this.Props.pawnSpawnRadius.min && num <= (float)this.Props.pawnSpawnRadius.max;
			};
			IntVec3 result;
			if (CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.MapHeld, this.Props.pawnSpawnRadius.max, validator, out result, -1))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600A1AE RID: 41390 RVA: 0x0039C8A8 File Offset: 0x0039AAA8
		private List<Thing> GeneratePawns()
		{
			List<Thing> list = new List<Thing>();
			float pointsLeft;
			Func<PawnKindDef, bool> <>9__0;
			PawnKindDef pawnKindDef;
			for (pointsLeft = this.points; pointsLeft > 0f; pointsLeft -= pawnKindDef.combatPower)
			{
				IEnumerable<PawnKindDef> spawnablePawnKinds = this.Props.spawnablePawnKinds;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((PawnKindDef p) => p.combatPower <= pointsLeft));
				}
				if (!spawnablePawnKinds.Where(predicate).TryRandomElement(out pawnKindDef))
				{
					break;
				}
				int index = pawnKindDef.lifeStages.Count - 1;
				List<Thing> list2 = list;
				PawnKindDef kind = pawnKindDef;
				Faction faction = this.parent.Faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(pawnKindDef.race.race.lifeStageAges[index].minAge);
				list2.Add(PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)));
			}
			this.points = 0f;
			return list;
		}

		// Token: 0x0600A1AF RID: 41391 RVA: 0x0039CA0C File Offset: 0x0039AC0C
		private void Spawn()
		{
			Lord lord = CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, delegate(Thing spawner)
			{
				CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = spawner.TryGetComp<CompPawnSpawnOnWakeup>();
				if (compPawnSpawnOnWakeup != null)
				{
					return compPawnSpawnOnWakeup.spawnedPawns;
				}
				return null;
			}, 2.1474836E+09f);
			if (lord == null)
			{
				lord = CompSpawnerPawn.CreateNewLord(this.parent, this.Props.aggressive, this.Props.defendRadius, this.Props.lordJob);
			}
			IntVec3 spawnPosition = this.GetSpawnPosition();
			if (!spawnPosition.IsValid)
			{
				return;
			}
			List<Thing> list = this.GeneratePawns();
			if (this.Props.dropInPods)
			{
				DropPodUtility.DropThingsNear(spawnPosition, this.parent.MapHeld, list, 110, false, false, true, true, true, this.parent.Faction);
			}
			List<IntVec3> occupiedCells = new List<IntVec3>();
			Predicate<IntVec3> <>9__1;
			foreach (Thing thing in list)
			{
				if (!this.Props.dropInPods)
				{
					IntVec3 root = spawnPosition;
					Map map = this.parent.Map;
					int randomInRange = this.Props.pawnSpawnRadius.RandomInRange;
					Predicate<IntVec3> extraValidator;
					if ((extraValidator = <>9__1) == null)
					{
						extraValidator = (<>9__1 = ((IntVec3 c) => !occupiedCells.Contains(c)));
					}
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(root, map, randomInRange, extraValidator);
					if (!intVec.IsValid)
					{
						intVec = CellFinder.RandomClosewalkCellNear(spawnPosition, this.parent.Map, this.Props.pawnSpawnRadius.RandomInRange, null);
					}
					GenSpawn.Spawn(thing, intVec, this.parent.Map, WipeMode.Vanish);
					occupiedCells.Add(intVec);
				}
				lord.AddPawn((Pawn)thing);
				this.spawnedPawns.Add((Pawn)thing);
				CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.WakeUp();
				}
				if (this.Props.mentalState != null)
				{
					((Pawn)thing).mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.CocoonDisturbed, null, false, false, false, null, false, false, false);
				}
			}
			if (this.Props.spawnEffecter != null)
			{
				Effecter effecter = new Effecter(this.Props.spawnEffecter);
				effecter.Trigger(this.parent, TargetInfo.Invalid, -1);
				effecter.Cleanup();
			}
			if (this.Props.spawnSound != null)
			{
				this.Props.spawnSound.PlayOneShot(this.parent);
			}
			if (this.Props.activatedMessageKey != null)
			{
				Messages.Message(this.Props.activatedMessageKey.Translate(), this.spawnedPawns, MessageTypeDefOf.ThreatBig, true);
			}
			if (this.Props.destroyAfterSpawn && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600A1B0 RID: 41392 RVA: 0x0039CD00 File Offset: 0x0039AF00
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPawnSpawnOnWakeup.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompPawnSpawnOnWakeup.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A1B1 RID: 41393 RVA: 0x0039CD10 File Offset: 0x0039AF10
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04005F8D RID: 24461
		public float points;

		// Token: 0x04005F8E RID: 24462
		public List<Pawn> spawnedPawns = new List<Pawn>();
	}
}
