using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002E5 RID: 741
	public static class PocketMapUtility
	{
		// Token: 0x0600169A RID: 5786 RVA: 0x00087540 File Offset: 0x00085740
		public static Map GeneratePocketMap(IntVec3 size, MapGeneratorDef generatorDef, IEnumerable<GenStepWithParams> extraGenStepDefs, Map sourceMap)
		{
			PocketMapParent pocketMapParent = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.PocketMap) as PocketMapParent;
			pocketMapParent.sourceMap = sourceMap;
			pocketMapParent.mapGenerator = generatorDef;
			Map result = MapGenerator.GenerateMap(size, pocketMapParent, generatorDef, extraGenStepDefs, null, true, false);
			Find.World.pocketMaps.Add(pocketMapParent);
			return result;
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00087588 File Offset: 0x00085788
		public static void DestroyPocketMap(Map map)
		{
			PocketMapParent pocketMapParent = ((map != null) ? map.Parent : null) as PocketMapParent;
			if (map == null || pocketMapParent == null)
			{
				return;
			}
			Find.World.pocketMaps.Remove(pocketMapParent);
			Current.Game.DeinitAndRemoveMap(map, true);
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x04001363 RID: 4963
		public static MapPortal currentlyGeneratingPortal;
	}
}
