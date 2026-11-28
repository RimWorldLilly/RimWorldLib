using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C11 RID: 3089
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005832 RID: 22578 RVA: 0x0020A0E4 File Offset: 0x002082E4
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x0020A0E7 File Offset: 0x002082E7
		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x0020A0EA File Offset: 0x002082EA
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			WorkGiver_Grower.<PotentialWorkCellsGlobal>d__4 <PotentialWorkCellsGlobal>d__ = new WorkGiver_Grower.<PotentialWorkCellsGlobal>d__4(-2);
			<PotentialWorkCellsGlobal>d__.<>4__this = this;
			<PotentialWorkCellsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkCellsGlobal>d__;
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x0020A104 File Offset: 0x00208304
		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			if (plantToGrowSettable == null)
			{
				return null;
			}
			return plantToGrowSettable.GetPlantDefToGrow();
		}

		// Token: 0x0400334B RID: 13131
		protected static ThingDef wantedPlantDef;
	}
}
