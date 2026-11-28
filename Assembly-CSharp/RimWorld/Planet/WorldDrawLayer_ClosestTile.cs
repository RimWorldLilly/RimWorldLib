using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002315 RID: 8981
	public class WorldDrawLayer_ClosestTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x170026D1 RID: 9937
		// (get) Token: 0x0600DDD5 RID: 56789 RVA: 0x00529178 File Offset: 0x00527378
		protected override PlanetTile Tile
		{
			get
			{
				if (Find.WorldTargeter.IsTargeting)
				{
					return Find.WorldTargeter.ClosestLayerTile;
				}
				return Find.TilePicker.ClosestLayerTile;
			}
		}

		// Token: 0x170026D2 RID: 9938
		// (get) Token: 0x0600DDD6 RID: 56790 RVA: 0x0052919B File Offset: 0x0052739B
		protected override Material Material
		{
			get
			{
				return WorldMaterials.ClosestTile;
			}
		}

		// Token: 0x170026D3 RID: 9939
		// (get) Token: 0x0600DDD7 RID: 56791 RVA: 0x005291A2 File Offset: 0x005273A2
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026D4 RID: 9940
		// (get) Token: 0x0600DDD8 RID: 56792 RVA: 0x005291A5 File Offset: 0x005273A5
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
