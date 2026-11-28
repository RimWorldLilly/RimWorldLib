using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ACD RID: 6861
	internal class Building_SunLamp : Building
	{
		// Token: 0x17001F5B RID: 8027
		// (get) Token: 0x0600AEA9 RID: 44713 RVA: 0x003D840B File Offset: 0x003D660B
		public IEnumerable<IntVec3> GrowableCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, this.def.specialDisplayRadius, true);
			}
		}

		// Token: 0x0600AEAA RID: 44714 RVA: 0x003D8424 File Offset: 0x003D6624
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_SunLamp.<GetGizmos>d__2 <GetGizmos>d__ = new Building_SunLamp.<GetGizmos>d__2(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AEAB RID: 44715 RVA: 0x003D8434 File Offset: 0x003D6634
		private void MakeMatchingGrowZone()
		{
			Designator_ZoneAdd_Growing designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
			designator.DesignateMultiCell(from tempCell in this.GrowableCells
			where designator.CanDesignateCell(tempCell).Accepted
			select tempCell);
		}
	}
}
