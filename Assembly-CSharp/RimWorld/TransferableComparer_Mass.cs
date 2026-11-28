using System;

namespace RimWorld
{
	// Token: 0x02001DFB RID: 7675
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x0600C788 RID: 51080 RVA: 0x00494624 File Offset: 0x00492824
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true, -1).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true, -1));
		}
	}
}
