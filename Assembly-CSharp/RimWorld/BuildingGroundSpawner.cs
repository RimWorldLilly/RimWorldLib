using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE8 RID: 6888
	public class BuildingGroundSpawner : GroundSpawner
	{
		// Token: 0x17001FB5 RID: 8117
		// (get) Token: 0x0600AFF3 RID: 45043 RVA: 0x003E0330 File Offset: 0x003DE530
		protected override IntRange ResultSpawnDelay
		{
			get
			{
				IntRange? intRange = this.emergeDelay;
				if (intRange == null)
				{
					return this.def.building.groundSpawnerSpawnDelay;
				}
				return intRange.GetValueOrDefault();
			}
		}

		// Token: 0x17001FB6 RID: 8118
		// (get) Token: 0x0600AFF4 RID: 45044 RVA: 0x003E0365 File Offset: 0x003DE565
		protected override SoundDef SustainerSound
		{
			get
			{
				return this.def.building.groundSpawnerSustainerSound ?? SoundDefOf.Tunnel;
			}
		}

		// Token: 0x17001FB7 RID: 8119
		// (get) Token: 0x0600AFF5 RID: 45045 RVA: 0x003E0380 File Offset: 0x003DE580
		protected virtual ThingDef ThingDefToSpawn
		{
			get
			{
				return this.def.building.groundSpawnerThingToSpawn;
			}
		}

		// Token: 0x17001FB8 RID: 8120
		// (get) Token: 0x0600AFF6 RID: 45046 RVA: 0x003E0392 File Offset: 0x003DE592
		public Thing ThingToSpawn
		{
			get
			{
				return this.thingToSpawn;
			}
		}

		// Token: 0x0600AFF7 RID: 45047 RVA: 0x003E039A File Offset: 0x003DE59A
		public override void PostMake()
		{
			base.PostMake();
			this.PostMakeInt();
		}

		// Token: 0x0600AFF8 RID: 45048 RVA: 0x003E03A8 File Offset: 0x003DE5A8
		protected virtual void PostMakeInt()
		{
			this.thingToSpawn = ThingMaker.MakeThing(this.ThingDefToSpawn, null);
		}

		// Token: 0x0600AFF9 RID: 45049 RVA: 0x003E03BC File Offset: 0x003DE5BC
		protected override void Spawn(Map map, IntVec3 pos)
		{
			TerrainDef newTerr = map.Biome.TerrainForAffordance(this.ThingDefToSpawn.terrainAffordanceNeeded);
			foreach (IntVec3 c in GenAdj.OccupiedRect(pos, Rot4.North, this.ThingDefToSpawn.Size))
			{
				map.terrainGrid.RemoveTopLayer(c, false);
				if (!c.GetAffordances(map).Contains(this.ThingDefToSpawn.terrainAffordanceNeeded))
				{
					map.terrainGrid.SetTerrain(c, newTerr);
				}
			}
			GenSpawn.Spawn(this.thingToSpawn, pos, map, Rot4.North, WipeMode.FullRefund, false, true);
			this.thingToSpawn.questTags = this.questTagsToForward;
			BuildingProperties building = this.def.building;
			if (building != null && building.groundSpawnerDestroyAdjacent)
			{
				foreach (IntVec3 c2 in GenAdj.CellsAdjacentCardinal(this.thingToSpawn))
				{
					Building edifice = c2.GetEdifice(map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Refund);
					}
				}
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			BuildingProperties building2 = this.def.building;
			if (((building2 != null) ? building2.groundSpawnerLetterLabel : null) != null)
			{
				BuildingProperties building3 = this.def.building;
				if (((building3 != null) ? building3.groundSpawnerLetterText : null) != null)
				{
					Find.LetterStack.ReceiveLetter(this.def.building.groundSpawnerLetterLabel, this.def.building.groundSpawnerLetterText, LetterDefOf.NegativeEvent, new TargetInfo(this.thingToSpawn), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x0600AFFA RID: 45050 RVA: 0x003E0598 File Offset: 0x003DE798
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange?>(ref this.emergeDelay, "emergeDelay", null, false);
			Scribe_Deep.Look<Thing>(ref this.thingToSpawn, "thingToSpawn", Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.questTagsToForward, "questTagsToForward", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04006593 RID: 26003
		protected Thing thingToSpawn;

		// Token: 0x04006594 RID: 26004
		public IntRange? emergeDelay;

		// Token: 0x04006595 RID: 26005
		public List<string> questTagsToForward;
	}
}
