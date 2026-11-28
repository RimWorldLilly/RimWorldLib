using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002324 RID: 8996
	public class WorldDrawLayer_SettleTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x170026F4 RID: 9972
		// (get) Token: 0x0600DE31 RID: 56881 RVA: 0x0052A60C File Offset: 0x0052880C
		protected override PlanetTile Tile
		{
			get
			{
				if (!(WorldGizmoUtility.LastMouseOverGizmo is Command_Settle))
				{
					return PlanetTile.Invalid;
				}
				Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
				if (caravan == null)
				{
					return PlanetTile.Invalid;
				}
				return caravan.Tile;
			}
		}

		// Token: 0x170026F5 RID: 9973
		// (get) Token: 0x0600DE32 RID: 56882 RVA: 0x0052A64A File Offset: 0x0052884A
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170026F6 RID: 9974
		// (get) Token: 0x0600DE33 RID: 56883 RVA: 0x0052A651 File Offset: 0x00528851
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
