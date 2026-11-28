using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200022E RID: 558
	public abstract class AnimalPenEnclosureCalculator
	{
		// Token: 0x06001030 RID: 4144 RVA: 0x0005D707 File Offset: 0x0005B907
		protected AnimalPenEnclosureCalculator()
		{
			this.regionProcessor = new RegionProcessor(this.ProcessRegion);
			this.regionEntryPredicate = new RegionEntryPredicate(this.EnterRegion);
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0005D733 File Offset: 0x0005B933
		protected virtual void VisitDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0005D735 File Offset: 0x0005B935
		protected virtual void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0005D737 File Offset: 0x0005B937
		protected virtual void VisitPassableDoorway(Region r)
		{
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0005D739 File Offset: 0x0005B939
		protected virtual void VisitImpassableDoorway(Region r)
		{
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0005D73C File Offset: 0x0005B93C
		protected bool VisitPen(IntVec3 position, Map map)
		{
			this.rootDistrict = position.GetDistrict(map, RegionType.Set_Passable);
			if (this.rootDistrict == null || this.rootDistrict.TouchesMapEdge)
			{
				return false;
			}
			this.isEnclosed = true;
			RegionTraverser.BreadthFirstTraverse(position, map, this.regionEntryPredicate, this.regionProcessor, 999999, RegionType.Set_Passable);
			return this.isEnclosed;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0005D796 File Offset: 0x0005B996
		public static bool RoamerCanPass(Building_Door door)
		{
			return door.FreePassage || AnimalPenEnclosureCalculator.RoamerCanPass(door.def);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0005D7AD File Offset: 0x0005B9AD
		public static bool RoamerCanPass(ThingDef thingDef)
		{
			return thingDef.building.roamerCanOpen;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0005D7BA File Offset: 0x0005B9BA
		private bool EnterRegion(Region from, Region to)
		{
			return (!from.IsDoorway || AnimalPenEnclosureCalculator.RoamerCanPass(from.door)) && (to.type == RegionType.Normal || to.IsDoorway);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0005D7E4 File Offset: 0x0005B9E4
		private bool ProcessRegion(Region reg)
		{
			if (reg.touchesMapEdge)
			{
				this.isEnclosed = false;
				return true;
			}
			if (reg.type == RegionType.Normal)
			{
				if (reg.District == this.rootDistrict)
				{
					this.VisitDirectlyConnectedRegion(reg);
				}
				else
				{
					this.VisitIndirectlyDirectlyConnectedRegion(reg);
				}
			}
			else if (reg.IsDoorway)
			{
				if (AnimalPenEnclosureCalculator.RoamerCanPass(reg.door))
				{
					this.VisitPassableDoorway(reg);
				}
				else
				{
					this.VisitImpassableDoorway(reg);
				}
			}
			return false;
		}

		// Token: 0x04000F34 RID: 3892
		private District rootDistrict;

		// Token: 0x04000F35 RID: 3893
		private bool isEnclosed;

		// Token: 0x04000F36 RID: 3894
		private readonly RegionProcessor regionProcessor;

		// Token: 0x04000F37 RID: 3895
		private readonly RegionEntryPredicate regionEntryPredicate;
	}
}
