using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002317 RID: 8983
	public class WorldDrawLayer_CurrentMapTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x170026D5 RID: 9941
		// (get) Token: 0x0600DDDF RID: 56799 RVA: 0x00529261 File Offset: 0x00527461
		protected override PlanetTile Tile
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return PlanetTile.Invalid;
				}
				if (Find.CurrentMap == null)
				{
					return PlanetTile.Invalid;
				}
				return Find.CurrentMap.Tile;
			}
		}

		// Token: 0x170026D6 RID: 9942
		// (get) Token: 0x0600DDE0 RID: 56800 RVA: 0x00529288 File Offset: 0x00527488
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170026D7 RID: 9943
		// (get) Token: 0x0600DDE1 RID: 56801 RVA: 0x0052928F File Offset: 0x0052748F
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026D8 RID: 9944
		// (get) Token: 0x0600DDE2 RID: 56802 RVA: 0x00529292 File Offset: 0x00527492
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
