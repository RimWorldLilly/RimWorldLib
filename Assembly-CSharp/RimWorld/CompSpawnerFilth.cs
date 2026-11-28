using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197F RID: 6527
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x17001C77 RID: 7287
		// (get) Token: 0x0600A3ED RID: 41965 RVA: 0x003A9277 File Offset: 0x003A7477
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x17001C78 RID: 7288
		// (get) Token: 0x0600A3EE RID: 41966 RVA: 0x003A9284 File Offset: 0x003A7484
		private bool CanSpawnFilth
		{
			get
			{
				Hive hive = this.parent as Hive;
				if (hive != null && !hive.CompDormant.Awake)
				{
					return false;
				}
				if (this.Props.requiredRotStage != null)
				{
					RotStage rotStage = this.parent.GetRotStage();
					RotStage? requiredRotStage = this.Props.requiredRotStage;
					if (!(rotStage == requiredRotStage.GetValueOrDefault() & requiredRotStage != null))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600A3EF RID: 41967 RVA: 0x003A92EE File Offset: 0x003A74EE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x0600A3F0 RID: 41968 RVA: 0x003A9308 File Offset: 0x003A7508
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				for (int i = 0; i < this.Props.spawnCountOnSpawn; i++)
				{
					this.TrySpawnFilth();
				}
			}
		}

		// Token: 0x0600A3F1 RID: 41969 RVA: 0x003A9334 File Offset: 0x003A7534
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			this.TickIntervalDelta(delta);
		}

		// Token: 0x0600A3F2 RID: 41970 RVA: 0x003A9344 File Offset: 0x003A7544
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickIntervalDelta(250);
		}

		// Token: 0x0600A3F3 RID: 41971 RVA: 0x003A9358 File Offset: 0x003A7558
		private void TickIntervalDelta(int interval)
		{
			if (!this.CanSpawnFilth)
			{
				return;
			}
			if (this.Props.spawnMtbHours > 0f && Rand.MTBEventOccurs(this.Props.spawnMtbHours, 2500f, (float)interval))
			{
				this.TrySpawnFilth();
			}
			if (this.Props.spawnEveryDays >= 0f && Find.TickManager.TicksGame >= this.nextSpawnTimestamp)
			{
				if (this.nextSpawnTimestamp != -1)
				{
					this.TrySpawnFilth();
				}
				this.nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(this.Props.spawnEveryDays * 60000f);
			}
		}

		// Token: 0x0600A3F4 RID: 41972 RVA: 0x003A93F8 File Offset: 0x003A75F8
		public void TrySpawnFilth()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableNearbyCell(this.parent.Position, this.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 x) => x.Standable(this.parent.Map), (Region x) => true, out c, 999999))
			{
				return;
			}
			FilthMaker.TryMakeFilth(c, this.parent.Map, this.Props.filthDef, 1, FilthSourceFlags.None, true);
		}

		// Token: 0x040060D8 RID: 24792
		private int nextSpawnTimestamp = -1;
	}
}
