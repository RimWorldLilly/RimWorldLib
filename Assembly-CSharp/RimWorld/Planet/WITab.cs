using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023ED RID: 9197
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x1700288A RID: 10378
		// (get) Token: 0x0600E5BA RID: 58810 RVA: 0x00555B0F File Offset: 0x00553D0F
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x1700288B RID: 10379
		// (get) Token: 0x0600E5BB RID: 58811 RVA: 0x00555B1B File Offset: 0x00553D1B
		protected PlanetTile SelPlanetTile
		{
			get
			{
				WorldObject selObject = this.SelObject;
				if (selObject == null)
				{
					return Find.WorldSelector.SelectedTile;
				}
				return selObject.Tile;
			}
		}

		// Token: 0x1700288C RID: 10380
		// (get) Token: 0x0600E5BC RID: 58812 RVA: 0x00555B37 File Offset: 0x00553D37
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelPlanetTile];
			}
		}

		// Token: 0x1700288D RID: 10381
		// (get) Token: 0x0600E5BD RID: 58813 RVA: 0x00555B49 File Offset: 0x00553D49
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x1700288E RID: 10382
		// (get) Token: 0x0600E5BE RID: 58814 RVA: 0x00555B56 File Offset: 0x00553D56
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x1700288F RID: 10383
		// (get) Token: 0x0600E5BF RID: 58815 RVA: 0x00555B67 File Offset: 0x00553D67
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldSelected && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs != null && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x17002890 RID: 10384
		// (get) Token: 0x0600E5C0 RID: 58816 RVA: 0x00555BA0 File Offset: 0x00553DA0
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x0600E5C1 RID: 58817 RVA: 0x00555BAD File Offset: 0x00553DAD
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
