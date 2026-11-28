using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002268 RID: 8808
	public class QuestNode_Root_Asteroid : QuestNode
	{
		// Token: 0x0600D999 RID: 55705 RVA: 0x0050A1E8 File Offset: 0x005083E8
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return this.TryFindSiteTile(out planetTile);
		}

		// Token: 0x0600D99A RID: 55706 RVA: 0x0050A200 File Offset: 0x00508400
		protected override void RunInt()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			PlanetTile tile;
			this.TryFindSiteTile(out tile);
			SpaceMapParent spaceMapParent = (SpaceMapParent)WorldObjectMaker.MakeWorldObject(this.worldObjectDef);
			spaceMapParent.Tile = tile;
			if (this.mineables != null)
			{
				ThingDef thingDef = this.mineables.RandomElement<ThingDef>();
				slate.Set<string>("resource", thingDef.label, false);
				spaceMapParent.nameInt = "AsteroidName".Translate(thingDef.label.Named("RESOURCE"));
				spaceMapParent.preciousResource = thingDef;
			}
			slate.Set<SpaceMapParent>("worldObject", spaceMapParent, false);
			quest.SpawnWorldObject(spaceMapParent, null, null);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("worldObject.MapRemoved");
			int delayTicks = QuestNode_Root_Asteroid.TimeoutDays.RandomInRange * 60000;
			quest.WorldObjectTimeout(spaceMapParent, delayTicks, null, null, false, null, true);
			quest.Delay(delayTicks, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0600D99B RID: 55707 RVA: 0x0050A330 File Offset: 0x00508530
		private bool TryFindSiteTile(out PlanetTile tile)
		{
			tile = PlanetTile.Invalid;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			PlanetTile planetTile;
			PlanetTile origin;
			if (!TileFinder.TryFindRandomPlayerTile(out planetTile, false, null, true, null))
			{
				origin = new PlanetTile(0, Find.WorldGrid.Surface);
			}
			else
			{
				origin = planetTile;
			}
			PlanetLayer planetLayer;
			if (!Find.WorldGrid.TryGetFirstAdjacentLayerOfDef(origin, this.layerDef, out planetLayer))
			{
				return false;
			}
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(origin, 1f, 3f, FastTileFinder.LandmarkMode.Any, true, Hilliness.Undefined, Hilliness.Undefined, true, true, true);
			return planetLayer.FastTileFinder.Query(query, null, null, default(FastTileFinder.TileQueryParams)).TryRandomElement(out tile);
		}

		// Token: 0x04008B0A RID: 35594
		public PlanetLayerDef layerDef;

		// Token: 0x04008B0B RID: 35595
		public WorldObjectDef worldObjectDef;

		// Token: 0x04008B0C RID: 35596
		public List<ThingDef> mineables;

		// Token: 0x04008B0D RID: 35597
		private const int MaxDistanceFromColony = 3;

		// Token: 0x04008B0E RID: 35598
		private const int MinDistanceFromColony = 1;

		// Token: 0x04008B0F RID: 35599
		private static readonly IntRange TimeoutDays = new IntRange(45, 60);
	}
}
