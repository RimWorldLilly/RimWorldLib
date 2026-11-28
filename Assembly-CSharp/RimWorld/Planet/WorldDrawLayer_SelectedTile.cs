using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002323 RID: 8995
	public class WorldDrawLayer_SelectedTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x170026F0 RID: 9968
		// (get) Token: 0x0600DE2C RID: 56876 RVA: 0x0052A5EB File Offset: 0x005287EB
		protected override PlanetTile Tile
		{
			get
			{
				return Find.WorldSelector.SelectedTile;
			}
		}

		// Token: 0x170026F1 RID: 9969
		// (get) Token: 0x0600DE2D RID: 56877 RVA: 0x0052A5F7 File Offset: 0x005287F7
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}

		// Token: 0x170026F2 RID: 9970
		// (get) Token: 0x0600DE2E RID: 56878 RVA: 0x0052A5FE File Offset: 0x005287FE
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026F3 RID: 9971
		// (get) Token: 0x0600DE2F RID: 56879 RVA: 0x0052A601 File Offset: 0x00528801
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
