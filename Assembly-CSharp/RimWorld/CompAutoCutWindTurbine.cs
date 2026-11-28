using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F5 RID: 6133
	public class CompAutoCutWindTurbine : CompAutoCut
	{
		// Token: 0x0600990E RID: 39182 RVA: 0x00376830 File Offset: 0x00374A30
		public override IEnumerable<IntVec3> GetAutoCutCells()
		{
			return WindTurbineUtility.CalculateWindCells(this.parent.Position, this.parent.Rotation, this.parent.def.size);
		}

		// Token: 0x0600990F RID: 39183 RVA: 0x00376860 File Offset: 0x00374A60
		public override ThingFilter GetDefaultAutoCutFilter()
		{
			if (this.defaultAutoCutFilter == null)
			{
				this.defaultAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.GetFixedAutoCutFilter().AllowedThingDefs)
				{
					if (thingDef.blockWind)
					{
						this.defaultAutoCutFilter.SetAllow(thingDef, true);
					}
				}
			}
			return this.defaultAutoCutFilter;
		}

		// Token: 0x06009910 RID: 39184 RVA: 0x003768DC File Offset: 0x00374ADC
		public override ThingFilter GetFixedAutoCutFilter()
		{
			if (this.fixedAutoCutFilter == null)
			{
				this.fixedAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.parent.Map.wildPlantSpawner.AllWildPlants)
				{
					if (thingDef.plant.allowAutoCut)
					{
						this.fixedAutoCutFilter.SetAllow(thingDef, true);
					}
				}
				this.fixedAutoCutFilter.SetAllow(ThingCategoryDefOf.Stumps, true, null, null);
			}
			return this.fixedAutoCutFilter;
		}

		// Token: 0x06009911 RID: 39185 RVA: 0x00376980 File Offset: 0x00374B80
		public override void CompTick()
		{
			base.CompTick();
			if (this.autoCut && this.parent.IsHashIntervalTick(2000))
			{
				base.DesignatePlantsToCut();
			}
		}

		// Token: 0x04005B70 RID: 23408
		private ThingFilter fixedAutoCutFilter;

		// Token: 0x04005B71 RID: 23409
		public ThingFilter defaultAutoCutFilter;
	}
}
