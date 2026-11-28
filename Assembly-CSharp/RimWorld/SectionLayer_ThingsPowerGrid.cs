using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128F RID: 4751
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x060075D3 RID: 30163 RVA: 0x002973AC File Offset: 0x002955AC
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlagDefOf.PowerGrid;
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x002973CC File Offset: 0x002955CC
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x060075D5 RID: 30165 RVA: 0x002973DC File Offset: 0x002955DC
		protected override void TakePrintFrom(Thing t)
		{
			if (t.Faction != null && t.Faction != Faction.OfPlayer)
			{
				return;
			}
			Building building = t as Building;
			if (building != null)
			{
				building.PrintForPowerGrid(this);
			}
		}
	}
}
