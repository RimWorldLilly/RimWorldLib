using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC9 RID: 2761
	public class BreachingTargetData : IExposable
	{
		// Token: 0x060052D1 RID: 21201 RVA: 0x001EDE45 File Offset: 0x001EC045
		public BreachingTargetData()
		{
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x001EDE4D File Offset: 0x001EC04D
		public BreachingTargetData(Thing target, IntVec3 firingPosition)
		{
			this.target = target;
			this.firingPosition = firingPosition;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x001EDE63 File Offset: 0x001EC063
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<IntVec3>(ref this.firingPosition, "firingPosition", IntVec3.Invalid, false);
		}

		// Token: 0x040031E7 RID: 12775
		public Thing target;

		// Token: 0x040031E8 RID: 12776
		public IntVec3 firingPosition;
	}
}
