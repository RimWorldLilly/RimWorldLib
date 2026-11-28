using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200231C RID: 8988
	public class WorldDrawLayer_MouseTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x170026DF RID: 9951
		// (get) Token: 0x0600DDFA RID: 56826 RVA: 0x00529578 File Offset: 0x00527778
		protected override PlanetTile Tile
		{
			get
			{
				if (Find.World.UI.selector.dragBox.IsValidAndActive)
				{
					return PlanetTile.Invalid;
				}
				if (Find.WorldTargeter.IsTargeting)
				{
					return PlanetTile.Invalid;
				}
				if (Find.ScreenshotModeHandler.Active)
				{
					return PlanetTile.Invalid;
				}
				return GenWorld.MouseTile(false);
			}
		}

		// Token: 0x170026E0 RID: 9952
		// (get) Token: 0x0600DDFB RID: 56827 RVA: 0x005295D0 File Offset: 0x005277D0
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}

		// Token: 0x170026E1 RID: 9953
		// (get) Token: 0x0600DDFC RID: 56828 RVA: 0x005295D7 File Offset: 0x005277D7
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026E2 RID: 9954
		// (get) Token: 0x0600DDFD RID: 56829 RVA: 0x005295DA File Offset: 0x005277DA
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
