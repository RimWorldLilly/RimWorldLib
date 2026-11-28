using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F7 RID: 8951
	public class MoveableGrowZone : MoveableArea
	{
		// Token: 0x0600DD41 RID: 56641 RVA: 0x00526110 File Offset: 0x00524310
		public MoveableGrowZone(Gravship gravship, Zone_Growing growZone) : base(gravship, growZone.label, growZone.RenamableLabel, growZone.color, growZone.ID)
		{
			this.plantDef = growZone.PlantDefToGrow;
			this.allowSow = growZone.allowSow;
			this.allowCut = growZone.allowCut;
		}

		// Token: 0x0600DD42 RID: 56642 RVA: 0x00526160 File Offset: 0x00524360
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDef, "plantDef");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", false, false);
			Scribe_Values.Look<bool>(ref this.allowCut, "allowCut", false, false);
		}

		// Token: 0x0600DD43 RID: 56643 RVA: 0x0052619C File Offset: 0x0052439C
		public void TryCreateGrowZone(ZoneManager zoneManager, IntVec3 newOrigin)
		{
			Zone_Growing zone_Growing = new Zone_Growing(zoneManager)
			{
				label = this.label,
				color = this.color,
				ID = this.id
			};
			zone_Growing.SetPlantDefToGrow(this.plantDef);
			zone_Growing.allowSow = this.allowSow;
			zone_Growing.allowCut = this.allowCut;
			zoneManager.RegisterZone(zone_Growing);
			foreach (IntVec3 b in base.RelativeCells)
			{
				zone_Growing.AddCell(newOrigin + b);
			}
		}

		// Token: 0x04008DD0 RID: 36304
		public ThingDef plantDef;

		// Token: 0x04008DD1 RID: 36305
		public bool allowSow;

		// Token: 0x04008DD2 RID: 36306
		public bool allowCut;
	}
}
