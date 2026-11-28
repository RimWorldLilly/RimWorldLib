using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC4 RID: 6852
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x17001F4E RID: 8014
		// (get) Token: 0x0600AE5B RID: 44635 RVA: 0x003D6BEA File Offset: 0x003D4DEA
		public IEnumerable<Plant> PlantsOnMe
		{
			get
			{
				Building_PlantGrower.<get_PlantsOnMe>d__3 <get_PlantsOnMe>d__ = new Building_PlantGrower.<get_PlantsOnMe>d__3(-2);
				<get_PlantsOnMe>d__.<>4__this = this;
				return <get_PlantsOnMe>d__;
			}
		}

		// Token: 0x17001F4F RID: 8015
		// (get) Token: 0x0600AE5C RID: 44636 RVA: 0x003D6BFC File Offset: 0x003D4DFC
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x0600AE5D RID: 44637 RVA: 0x003D6C17 File Offset: 0x003D4E17
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_PlantGrower.<GetGizmos>d__6 <GetGizmos>d__ = new Building_PlantGrower.<GetGizmos>d__6(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE5E RID: 44638 RVA: 0x003D6C27 File Offset: 0x003D4E27
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x0600AE5F RID: 44639 RVA: 0x003D6C45 File Offset: 0x003D4E45
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x0600AE60 RID: 44640 RVA: 0x003D6C66 File Offset: 0x003D4E66
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x0600AE61 RID: 44641 RVA: 0x003D6C80 File Offset: 0x003D4E80
		public override void TickRare()
		{
			base.TickRare();
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Thing thing in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					thing.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x0600AE62 RID: 44642 RVA: 0x003D6D0C File Offset: 0x003D4F0C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
				{
					plant.Destroy(DestroyMode.Vanish);
				}
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600AE63 RID: 44643 RVA: 0x003D6D70 File Offset: 0x003D4F70
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.plantDefToGrow))
				{
					text += "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x0600AE64 RID: 44644 RVA: 0x003D6DE8 File Offset: 0x003D4FE8
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x0600AE65 RID: 44645 RVA: 0x003D6DF0 File Offset: 0x003D4FF0
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x0600AE66 RID: 44646 RVA: 0x003D6DF9 File Offset: 0x003D4FF9
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x040064E6 RID: 25830
		private ThingDef plantDefToGrow;

		// Token: 0x040064E7 RID: 25831
		private CompPowerTrader compPower;
	}
}
