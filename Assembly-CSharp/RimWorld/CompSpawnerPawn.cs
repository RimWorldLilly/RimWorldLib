using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001989 RID: 6537
	public class CompSpawnerPawn : ThingComp
	{
		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x0600A41A RID: 42010 RVA: 0x003A9F9B File Offset: 0x003A819B
		private CompProperties_SpawnerPawn Props
		{
			get
			{
				return (CompProperties_SpawnerPawn)this.props;
			}
		}

		// Token: 0x17001C80 RID: 7296
		// (get) Token: 0x0600A41B RID: 42011 RVA: 0x003A9FA8 File Offset: 0x003A81A8
		public Lord Lord
		{
			get
			{
				return CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, null, this.Props.lordJoinRadius);
			}
		}

		// Token: 0x17001C81 RID: 7297
		// (get) Token: 0x0600A41C RID: 42012 RVA: 0x003A9FD8 File Offset: 0x003A81D8
		private float SpawnedPawnsPoints
		{
			get
			{
				this.FilterOutUnspawnedPawns();
				float num = 0f;
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					num += this.spawnedPawns[i].kindDef.combatPower;
				}
				return num;
			}
		}

		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x0600A41D RID: 42013 RVA: 0x003AA021 File Offset: 0x003A8221
		public bool Active
		{
			get
			{
				return this.pawnsLeftToSpawn != 0 && !this.Dormant;
			}
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x0600A41E RID: 42014 RVA: 0x003AA038 File Offset: 0x003A8238
		public CompCanBeDormant DormancyComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormancyCompCached) == null)
				{
					result = (this.dormancyCompCached = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x0600A41F RID: 42015 RVA: 0x003AA063 File Offset: 0x003A8263
		public bool Dormant
		{
			get
			{
				return this.DormancyComp != null && !this.DormancyComp.Awake;
			}
		}

		// Token: 0x0600A420 RID: 42016 RVA: 0x003AA080 File Offset: 0x003A8280
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.chosenKind == null)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.Props.maxPawnsToSpawn != IntRange.Zero)
			{
				this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
			}
		}

		// Token: 0x0600A421 RID: 42017 RVA: 0x003AA0D8 File Offset: 0x003A82D8
		public static Lord FindLordToJoin(Thing spawner, Type lordJobType, bool shouldTryJoinParentLord, Func<Thing, List<Pawn>> spawnedPawnSelector = null, float lordJoinRadius = 2.1474836E+09f)
		{
			if (spawner.Spawned)
			{
				if (shouldTryJoinParentLord)
				{
					Building building = spawner as Building;
					Lord lord = (building != null) ? building.GetLord() : null;
					if (lord != null)
					{
						return lord;
					}
				}
				if (spawnedPawnSelector == null)
				{
					spawnedPawnSelector = delegate(Thing s)
					{
						CompSpawnerPawn compSpawnerPawn = s.TryGetComp<CompSpawnerPawn>();
						if (compSpawnerPawn == null)
						{
							return null;
						}
						return compSpawnerPawn.spawnedPawns;
					};
				}
				Predicate<Pawn> hasJob = delegate(Pawn x)
				{
					Lord lord2 = x.GetLord();
					return lord2 != null && lord2.LordJob.GetType() == lordJobType;
				};
				Pawn foundPawn = null;
				RegionTraverser.BreadthFirstTraverse(spawner.GetRegion(RegionType.Set_Passable), (Region from, Region to) => true, delegate(Region r)
				{
					List<Thing> list = r.ListerThings.ThingsOfDef(spawner.def);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Faction == spawner.Faction)
						{
							List<Pawn> list2 = spawnedPawnSelector(list[i]);
							if (list2 != null)
							{
								foundPawn = list2.Find(hasJob);
							}
							if (foundPawn != null && foundPawn.Position.InHorDistOf(spawner.Position, lordJoinRadius))
							{
								return true;
							}
						}
					}
					return false;
				}, 40, RegionType.Set_Passable);
				if (foundPawn != null)
				{
					return foundPawn.GetLord();
				}
			}
			return null;
		}

		// Token: 0x0600A422 RID: 42018 RVA: 0x003AA1FC File Offset: 0x003A83FC
		public static Lord CreateNewLord(Thing byThing, bool aggressive, float defendRadius, Type lordJobType)
		{
			IntVec3 invalid;
			if (!CellFinder.TryFindRandomCellNear(byThing.Position, byThing.Map, 5, (IntVec3 c) => c.Standable(byThing.Map) && byThing.Map.reachability.CanReach(c, byThing, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)), out invalid, -1))
			{
				string str = "Found no place for pawns to defend ";
				Thing byThing2 = byThing;
				Log.Error(str + ((byThing2 != null) ? byThing2.ToString() : null));
				invalid = IntVec3.Invalid;
			}
			return LordMaker.MakeNewLord(byThing.Faction, Activator.CreateInstance(lordJobType, new object[]
			{
				new SpawnedPawnParams
				{
					aggressive = aggressive,
					defendRadius = defendRadius,
					defSpot = invalid,
					spawnerThing = byThing
				}
			}) as LordJob, byThing.Map, null);
		}

		// Token: 0x0600A423 RID: 42019 RVA: 0x003AA2C0 File Offset: 0x003A84C0
		private void SpawnInitialPawns()
		{
			int num = 0;
			Pawn pawn;
			while (num < this.Props.initialPawnsCount && this.TrySpawnPawn(out pawn))
			{
				num++;
			}
			this.SpawnPawnsUntilPoints(this.Props.initialPawnsPoints);
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x0600A424 RID: 42020 RVA: 0x003AA304 File Offset: 0x003A8504
		public void SpawnPawnsUntilPoints(float points)
		{
			int num = 0;
			while (this.SpawnedPawnsPoints < points)
			{
				num++;
				if (num > 1000)
				{
					Log.Error("Too many iterations.");
					break;
				}
				Pawn pawn;
				if (!this.TrySpawnPawn(out pawn))
				{
					break;
				}
			}
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x0600A425 RID: 42021 RVA: 0x003AA346 File Offset: 0x003A8546
		private void CalculateNextPawnSpawnTick()
		{
			this.CalculateNextPawnSpawnTick(this.Props.pawnSpawnIntervalDays.RandomInRange * 60000f);
		}

		// Token: 0x0600A426 RID: 42022 RVA: 0x003AA364 File Offset: 0x003A8564
		public void CalculateNextPawnSpawnTick(float delayTicks)
		{
			if (Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f)
			{
				this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(delayTicks / (CompSpawnerPawn.RateFactorForBugCount.Evaluate((float)this.spawnedPawns.Count) * Find.Storyteller.difficulty.enemyReproductionRateFactor));
				return;
			}
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)delayTicks;
		}

		// Token: 0x0600A427 RID: 42023 RVA: 0x003AA3D8 File Offset: 0x003A85D8
		private void FilterOutUnspawnedPawns()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600A428 RID: 42024 RVA: 0x003AA41C File Offset: 0x003A861C
		private PawnKindDef RandomPawnKindDef()
		{
			float curPoints = this.SpawnedPawnsPoints;
			IEnumerable<PawnKindDef> source = this.Props.spawnablePawnKinds;
			if (this.Props.maxSpawnedPawnsPoints > -1f)
			{
				source = from x in source
				where curPoints + x.combatPower <= this.Props.maxSpawnedPawnsPoints
				select x;
			}
			PawnKindDef result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600A429 RID: 42025 RVA: 0x003AA480 File Offset: 0x003A8680
		private bool TrySpawnPawn(out Pawn pawn)
		{
			if (!this.canSpawnPawns)
			{
				pawn = null;
				return false;
			}
			if (!this.Props.chooseSingleTypeToSpawn)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.chosenKind == null)
			{
				pawn = null;
				return false;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(this.chosenKind, this.parent.Faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			if (this.chosenKind.RaceProps.IsMechanoid)
			{
				request.AllowedDevelopmentalStages = DevelopmentalStage.Newborn;
			}
			else
			{
				int index = this.chosenKind.lifeStages.Count - 1;
				request.FixedBiologicalAge = new float?(this.chosenKind.race.race.lifeStageAges[index].minAge);
			}
			pawn = PawnGenerator.GeneratePawn(request);
			this.spawnedPawns.Add(pawn);
			GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.parent.Position, this.parent.Map, this.Props.pawnSpawnRadius, null), this.parent.Map, WipeMode.Vanish);
			(this.Lord ?? CompSpawnerPawn.CreateNewLord(this.parent, this.aggressive, this.Props.defendRadius, this.Props.lordJob)).AddPawn(pawn);
			SoundDef spawnSound = this.Props.spawnSound;
			if (spawnSound != null)
			{
				spawnSound.PlayOneShot(this.parent);
			}
			if (this.pawnsLeftToSpawn > 0)
			{
				this.pawnsLeftToSpawn--;
			}
			this.SendMessage();
			return true;
		}

		// Token: 0x0600A42A RID: 42026 RVA: 0x003AA67F File Offset: 0x003A887F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.Active && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
		}

		// Token: 0x0600A42B RID: 42027 RVA: 0x003AA6A4 File Offset: 0x003A88A4
		public override void CompTick()
		{
			if (this.Active && this.parent.Spawned && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
			if (this.parent.Spawned)
			{
				this.FilterOutUnspawnedPawns();
				if (this.Active && Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
				{
					Pawn pawn;
					if ((this.Props.maxSpawnedPawnsPoints < 0f || this.SpawnedPawnsPoints < this.Props.maxSpawnedPawnsPoints) && Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f && this.TrySpawnPawn(out pawn) && pawn.caller != null)
					{
						pawn.caller.DoCall(false);
					}
					this.CalculateNextPawnSpawnTick();
				}
			}
		}

		// Token: 0x0600A42C RID: 42028 RVA: 0x003AA764 File Offset: 0x003A8964
		public void SendMessage()
		{
			if (!this.Props.spawnMessageKey.NullOrEmpty() && MessagesRepeatAvoider.MessageShowAllowed(this.Props.spawnMessageKey, 0.1f))
			{
				Messages.Message(this.Props.spawnMessageKey.Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600A42D RID: 42029 RVA: 0x003AA7C5 File Offset: 0x003A89C5
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSpawnerPawn.<CompGetGizmosExtra>d__33 <CompGetGizmosExtra>d__ = new CompSpawnerPawn.<CompGetGizmosExtra>d__33(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A42E RID: 42030 RVA: 0x003AA7D8 File Offset: 0x003A89D8
		public override string CompInspectStringExtra()
		{
			if (!this.Props.showNextSpawnInInspect || this.nextPawnSpawnTick <= 0 || this.chosenKind == null)
			{
				return null;
			}
			if (this.pawnsLeftToSpawn == 0 && !this.Props.noPawnsLeftToSpawnKey.NullOrEmpty())
			{
				return this.Props.noPawnsLeftToSpawnKey.Translate();
			}
			string text;
			if (!this.Dormant)
			{
				text = (this.Props.nextSpawnInspectStringKey ?? "SpawningNextPawnIn").Translate(this.chosenKind.LabelCap, (this.nextPawnSpawnTick - Find.TickManager.TicksGame).ToStringTicksToDays("F1"));
			}
			else
			{
				if (this.Props.nextSpawnInspectStringKeyDormant == null)
				{
					return null;
				}
				text = this.Props.nextSpawnInspectStringKeyDormant.Translate() + ": " + this.chosenKind.LabelCap;
			}
			if (this.pawnsLeftToSpawn > 0 && !this.Props.pawnsLeftToSpawnKey.NullOrEmpty())
			{
				text = text + ("\n" + this.Props.pawnsLeftToSpawnKey.Translate() + ": ") + this.pawnsLeftToSpawn.ToString();
			}
			return text;
		}

		// Token: 0x0600A42F RID: 42031 RVA: 0x003AA928 File Offset: 0x003A8B28
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.pawnsLeftToSpawn, "pawnsLeftToSpawn", -1, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.chosenKind, "chosenKind");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
				if (this.pawnsLeftToSpawn == -1 && this.Props.maxPawnsToSpawn != IntRange.Zero)
				{
					this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
				}
			}
		}

		// Token: 0x04006106 RID: 24838
		public int nextPawnSpawnTick = -1;

		// Token: 0x04006107 RID: 24839
		public int pawnsLeftToSpawn = -1;

		// Token: 0x04006108 RID: 24840
		public List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x04006109 RID: 24841
		public bool aggressive = true;

		// Token: 0x0400610A RID: 24842
		public bool canSpawnPawns = true;

		// Token: 0x0400610B RID: 24843
		private PawnKindDef chosenKind;

		// Token: 0x0400610C RID: 24844
		private static readonly SimpleCurve RateFactorForBugCount = new SimpleCurve
		{
			{
				0f,
				1f,
				true
			},
			{
				5f,
				0.5f,
				true
			}
		};

		// Token: 0x0400610D RID: 24845
		private CompCanBeDormant dormancyCompCached;
	}
}
