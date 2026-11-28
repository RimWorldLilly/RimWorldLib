using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCE RID: 3278
	public class LordToilData_DefendFleshmassHeart : LordToilData
	{
		// Token: 0x06005CE1 RID: 23777 RVA: 0x0022157D File Offset: 0x0021F77D
		public override void ExposeData()
		{
			Scribe_References.Look<Building_FleshmassHeart>(ref this.heart, "heart", false);
		}

		// Token: 0x040034E9 RID: 13545
		public Building_FleshmassHeart heart;
	}
}
