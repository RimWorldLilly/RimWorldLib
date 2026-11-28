using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000271 RID: 625
	public static class GetOrGenerateMapUtility
	{
		// Token: 0x0600122C RID: 4652 RVA: 0x000684C8 File Offset: 0x000666C8
		public static Map GetOrGenerateMap(PlanetTile tile, IntVec3 size, WorldObjectDef suggestedMapParentDef, IEnumerable<GenStepWithParams> extraGenStepDefs = null, bool stepDebugger = false)
		{
			Map map = Current.Game.FindMap(tile);
			if (map == null)
			{
				MapParent mapParent = Find.WorldObjects.MapParentAt(tile);
				if (mapParent == null)
				{
					if (suggestedMapParentDef == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(118, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Tried to get or generate map at ");
						defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(tile);
						defaultInterpolatedStringHandler.AppendLiteral(", but there isn't any MapParent world object here and map parent def argument is null.");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						return null;
					}
					mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(suggestedMapParentDef);
					mapParent.Tile = tile;
					Find.WorldObjects.Add(mapParent);
				}
				if (!tile.Valid)
				{
					map = PocketMapUtility.GeneratePocketMap(size, mapParent.MapGeneratorDef, extraGenStepDefs, Find.AnyPlayerHomeMap);
				}
				else
				{
					map = MapGenerator.GenerateMap(size, mapParent, mapParent.MapGeneratorDef, mapParent.ExtraGenStepDefs.ConcatIfNotNull(extraGenStepDefs), null, false, stepDebugger);
				}
			}
			return map;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0006858E File Offset: 0x0006678E
		public static Map GetOrGenerateMap(PlanetTile tile, WorldObjectDef suggestedMapParentDef, IEnumerable<GenStepWithParams> extraGenStepDefs = null)
		{
			return GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, suggestedMapParentDef, extraGenStepDefs, false);
		}
	}
}
