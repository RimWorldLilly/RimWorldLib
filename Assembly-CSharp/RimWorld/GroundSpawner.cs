using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AF3 RID: 6899
	[StaticConstructorOnStartup]
	public class GroundSpawner : ThingWithComps
	{
		// Token: 0x17001FC9 RID: 8137
		// (get) Token: 0x0600B04A RID: 45130 RVA: 0x003E1BB7 File Offset: 0x003DFDB7
		protected virtual IntRange ResultSpawnDelay
		{
			get
			{
				return GroundSpawner.DefaultSpawnDelay;
			}
		}

		// Token: 0x17001FCA RID: 8138
		// (get) Token: 0x0600B04B RID: 45131 RVA: 0x003E1BBE File Offset: 0x003DFDBE
		protected virtual SoundDef SustainerSound
		{
			get
			{
				return SoundDefOf.Tunnel;
			}
		}

		// Token: 0x17001FCB RID: 8139
		// (get) Token: 0x0600B04C RID: 45132 RVA: 0x003E1BC5 File Offset: 0x003DFDC5
		public int TicksUntilSpawn
		{
			get
			{
				return this.secondarySpawnTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001FCC RID: 8140
		// (get) Token: 0x0600B04D RID: 45133 RVA: 0x003E1BD8 File Offset: 0x003DFDD8
		protected virtual bool SpawnRubble
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B04E RID: 45134 RVA: 0x003E1BDC File Offset: 0x003DFDDC
		public static void ResetStaticData()
		{
			GroundSpawner.filthTypes.Clear();
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			GroundSpawner.filthTypes.Add(ThingDefOf.Filth_RubbleRock);
		}

		// Token: 0x0600B04F RID: 45135 RVA: 0x003E1C30 File Offset: 0x003DFE30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.secondarySpawnTick, "secondarySpawnTick", 0, false);
			Scribe_Values.Look<float>(ref this.dustMoteSpawnMTB, "dustMoteSpawnMTB", 0.2f, false);
			Scribe_Values.Look<float>(ref this.filthSpawnMTB, "filthSpawnMTB", 0.3f, false);
			Scribe_Values.Look<float>(ref this.filthSpawnRadius, "filthSpawnRadius", 3f, false);
		}

		// Token: 0x0600B050 RID: 45136 RVA: 0x003E1C98 File Offset: 0x003DFE98
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.secondarySpawnTick = Find.TickManager.TicksGame + this.ResultSpawnDelay.RandomInRange;
			}
			this.CreateFX();
		}

		// Token: 0x0600B051 RID: 45137 RVA: 0x003E1CD8 File Offset: 0x003DFED8
		protected override void Tick()
		{
			base.Tick();
			if (!base.Spawned)
			{
				return;
			}
			Sustainer sustainer = this.sustainer;
			if (sustainer != null && !sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			Effecter effecter = this.sustainedFx;
			if (effecter != null)
			{
				effecter.EffectTick(this, this);
			}
			if (this.SpawnRubble && Rand.MTBEventOccurs(this.filthSpawnMTB, 60f, 1f))
			{
				IntVec3 c;
				if (CellFinder.TryFindRandomReachableNearbyCell(this.OccupiedRect().RandomCell, base.Map, this.filthSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), null, null, out c, 999999))
				{
					FilthMaker.TryMakeFilth(c, base.Map, GroundSpawner.filthTypes.RandomElement<ThingDef>(), 1, FilthSourceFlags.None, true);
				}
				Plant plant = this.OccupiedRect().RandomCell.GetPlant(base.Map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
			}
			if (this.secondarySpawnTick <= Find.TickManager.TicksGame)
			{
				Sustainer sustainer2 = this.sustainer;
				if (sustainer2 != null)
				{
					sustainer2.End();
				}
				Map map = base.Map;
				IntVec3 position = base.Position;
				BuildingProperties building = this.def.building;
				if (building != null)
				{
					EffecterDef groundSpawnerCompleteEffecter = building.groundSpawnerCompleteEffecter;
					if (groundSpawnerCompleteEffecter != null)
					{
						groundSpawnerCompleteEffecter.SpawnMaintained(position, map, 1f);
					}
				}
				Effecter effecter2 = this.sustainedFx;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				Thing.allowDestroyNonDestroyable = true;
				this.Destroy(DestroyMode.Vanish);
				Thing.allowDestroyNonDestroyable = false;
				this.Spawn(map, position);
			}
		}

		// Token: 0x0600B052 RID: 45138 RVA: 0x003E1E4E File Offset: 0x003E004E
		protected virtual void Spawn(Map map, IntVec3 loc)
		{
		}

		// Token: 0x0600B053 RID: 45139 RVA: 0x003E1E50 File Offset: 0x003E0050
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
		}

		// Token: 0x0600B054 RID: 45140 RVA: 0x003E1E52 File Offset: 0x003E0052
		private void CreateFX()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef sustainerSound = this.SustainerSound;
				this.sustainer = ((sustainerSound != null) ? sustainerSound.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick)) : null);
				BuildingProperties building = this.def.building;
				Effecter effecter;
				if (building == null)
				{
					effecter = null;
				}
				else
				{
					EffecterDef groundSpawnerSustainedEffecter = building.groundSpawnerSustainedEffecter;
					effecter = ((groundSpawnerSustainedEffecter != null) ? groundSpawnerSustainedEffecter.Spawn(this, base.Map, 1f) : null);
				}
				this.sustainedFx = effecter;
			});
		}

		// Token: 0x0600B055 RID: 45141 RVA: 0x003E1E65 File Offset: 0x003E0065
		public override IEnumerable<Gizmo> GetGizmos()
		{
			GroundSpawner.<GetGizmos>d__23 <GetGizmos>d__ = new GroundSpawner.<GetGizmos>d__23(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B056 RID: 45142 RVA: 0x003E1E78 File Offset: 0x003E0078
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("Emergence".Translate() + ": " + this.TicksUntilSpawn.ToStringTicksToPeriod(true, false, true, true, false));
			return stringBuilder.ToString();
		}

		// Token: 0x040065BD RID: 26045
		protected int secondarySpawnTick;

		// Token: 0x040065BE RID: 26046
		protected float dustMoteSpawnMTB = 0.2f;

		// Token: 0x040065BF RID: 26047
		protected float filthSpawnMTB = 0.3f;

		// Token: 0x040065C0 RID: 26048
		protected float filthSpawnRadius = 3f;

		// Token: 0x040065C1 RID: 26049
		private Sustainer sustainer;

		// Token: 0x040065C2 RID: 26050
		private Effecter sustainedFx;

		// Token: 0x040065C3 RID: 26051
		private static readonly IntRange DefaultSpawnDelay = new IntRange(1560, 1800);

		// Token: 0x040065C4 RID: 26052
		private static List<ThingDef> filthTypes = new List<ThingDef>();
	}
}
