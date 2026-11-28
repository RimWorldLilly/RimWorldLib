using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFC RID: 6908
	public class OrbitalDebrisSpawner : Thing
	{
		// Token: 0x0600B0A9 RID: 45225 RVA: 0x003E4384 File Offset: 0x003E2584
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
			Scribe_Values.Look<int>(ref this.meteoriteCount, "meteoriteCount", 0, false);
			Scribe_Values.Look<int>(ref this.meteoritesSpawned, "meteoritesSpawned", 0, false);
		}

		// Token: 0x0600B0AA RID: 45226 RVA: 0x003E43E0 File Offset: 0x003E25E0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.startTick = GenTicks.TicksGame + OrbitalDebrisSpawner.DelaySecondsRange.RandomInRange.SecondsToTicks();
			this.endTick = this.startTick + OrbitalDebrisSpawner.DurationSecondsRange.RandomInRange.SecondsToTicks();
			this.meteoriteCount = OrbitalDebrisSpawner.MeteoriteRange.RandomInRange;
		}

		// Token: 0x0600B0AB RID: 45227 RVA: 0x003E444C File Offset: 0x003E264C
		protected override void Tick()
		{
			if (GenTicks.TicksGame > this.endTick)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (GenTicks.TicksGame < this.startTick || this.meteoritesSpawned >= this.meteoriteCount)
			{
				return;
			}
			int positionsRemaining = this.endTick - GenTicks.TicksGame;
			if (Rand.DynamicChance(this.meteoritesSpawned, this.meteoriteCount, positionsRemaining))
			{
				this.meteoritesSpawned++;
				this.SpawnMeteoriteIncoming();
			}
		}

		// Token: 0x0600B0AC RID: 45228 RVA: 0x003E44C0 File Offset: 0x003E26C0
		private void SpawnMeteoriteIncoming()
		{
			IntVec3 pos;
			if (this.TryFindCell(out pos))
			{
				SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteCraterIncoming, pos, base.Map);
			}
		}

		// Token: 0x0600B0AD RID: 45229 RVA: 0x003E44EC File Offset: 0x003E26EC
		private bool TryFindCell(out IntVec3 cell)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteCraterIncoming, base.Map, TerrainAffordanceDefOf.Light, out cell, 10, default(IntVec3), -1, false, false, false, false, false, false, null);
		}

		// Token: 0x040065EB RID: 26091
		private static readonly FloatRange DelaySecondsRange = new FloatRange(3f, 12f);

		// Token: 0x040065EC RID: 26092
		private static readonly FloatRange DurationSecondsRange = new FloatRange(5f, 5f);

		// Token: 0x040065ED RID: 26093
		private static readonly IntRange MeteoriteRange = new IntRange(12, 16);

		// Token: 0x040065EE RID: 26094
		private int startTick;

		// Token: 0x040065EF RID: 26095
		private int endTick;

		// Token: 0x040065F0 RID: 26096
		private int meteoriteCount;

		// Token: 0x040065F1 RID: 26097
		private int meteoritesSpawned;
	}
}
