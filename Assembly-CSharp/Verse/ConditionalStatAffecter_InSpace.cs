using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020000DE RID: 222
	public class ConditionalStatAffecter_InSpace : ConditionalStatAffecter
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0002FFC8 File Offset: 0x0002E1C8
		public override string Label
		{
			get
			{
				return "StatsReport_InSpace".Translate();
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002FFDC File Offset: 0x0002E1DC
		public override bool Applies(StatRequest req)
		{
			if (req.HasThing && req.Thing.SpawnedOrAnyParentSpawned)
			{
				PlanetTile tile = req.Thing.MapHeld.Tile;
				return tile.Valid && tile.LayerDef.isSpace;
			}
			return false;
		}
	}
}
