using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001817 RID: 6167
	public class CompProperties_AssignableToPawn : CompProperties
	{
		// Token: 0x06009A26 RID: 39462 RVA: 0x0037AB80 File Offset: 0x00378D80
		public CompProperties_AssignableToPawn()
		{
			this.compClass = typeof(CompAssignableToPawn);
		}

		// Token: 0x06009A27 RID: 39463 RVA: 0x0037ABAD File Offset: 0x00378DAD
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		// Token: 0x06009A28 RID: 39464 RVA: 0x0037ABB6 File Offset: 0x00378DB6
		public override void PostLoadSpecial(ThingDef parent)
		{
			if (parent.thingClass == typeof(Building_Bed))
			{
				this.maxAssignedPawnsCount = BedUtility.GetSleepingSlotsCount(parent.size);
			}
		}

		// Token: 0x04005BC7 RID: 23495
		public int maxAssignedPawnsCount = 1;

		// Token: 0x04005BC8 RID: 23496
		public bool drawAssignmentOverlay = true;

		// Token: 0x04005BC9 RID: 23497
		public bool drawUnownedAssignmentOverlay = true;

		// Token: 0x04005BCA RID: 23498
		[MustTranslate]
		public string noAssignablePawnsDesc;
	}
}
