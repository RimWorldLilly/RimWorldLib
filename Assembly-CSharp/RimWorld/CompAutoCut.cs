using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F4 RID: 6132
	public abstract class CompAutoCut : ThingComp
	{
		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x06009903 RID: 39171 RVA: 0x00376683 File Offset: 0x00374883
		public ThingFilter AutoCutFilter
		{
			get
			{
				return this.autoCutFilter;
			}
		}

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x06009904 RID: 39172 RVA: 0x0037668B File Offset: 0x0037488B
		public virtual bool CanDesignatePlants
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009905 RID: 39173 RVA: 0x0037668E File Offset: 0x0037488E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.autoCutFilter == null)
			{
				this.autoCutFilter = new ThingFilter();
				this.autoCutFilter.CopyAllowancesFrom(this.GetDefaultAutoCutFilter());
			}
		}

		// Token: 0x06009906 RID: 39174
		public abstract ThingFilter GetDefaultAutoCutFilter();

		// Token: 0x06009907 RID: 39175
		public abstract ThingFilter GetFixedAutoCutFilter();

		// Token: 0x06009908 RID: 39176 RVA: 0x003766BB File Offset: 0x003748BB
		public override void CompTickLong()
		{
			base.CompTickLong();
			if (!this.autoCut)
			{
				return;
			}
			this.DesignatePlantsToCut();
		}

		// Token: 0x06009909 RID: 39177
		public abstract IEnumerable<IntVec3> GetAutoCutCells();

		// Token: 0x0600990A RID: 39178 RVA: 0x003766D4 File Offset: 0x003748D4
		public void DesignatePlantsToCut()
		{
			if (!this.CanDesignatePlants)
			{
				return;
			}
			Map map = this.parent.Map;
			foreach (IntVec3 c in this.GetAutoCutCells())
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null && this.CanDesignatePlant(plant))
					{
						map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.CutPlant, null));
					}
				}
			}
		}

		// Token: 0x0600990B RID: 39179 RVA: 0x00376780 File Offset: 0x00374980
		protected virtual bool CanDesignatePlant(Plant plant)
		{
			return plant.def.plant.allowAutoCut && !plant.DeliberatelyCultivated() && this.autoCutFilter.Allows(plant) && !plant.Map.designationManager.HasMapDesignationOn(plant);
		}

		// Token: 0x0600990C RID: 39180 RVA: 0x003767D4 File Offset: 0x003749D4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.autoCut, "autoCut", false, false);
			Scribe_Values.Look<int?>(ref this.lastCheckedAutoCutTick, "lastCheckedAutoCutTick", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.autoCutFilter, "autoCutFilter", Array.Empty<object>());
		}

		// Token: 0x04005B6D RID: 23405
		public bool autoCut;

		// Token: 0x04005B6E RID: 23406
		private int? lastCheckedAutoCutTick;

		// Token: 0x04005B6F RID: 23407
		private ThingFilter autoCutFilter;
	}
}
