using System;

namespace RimWorld
{
	// Token: 0x02001DFC RID: 7676
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x0600C78A RID: 51082 RVA: 0x00494665 File Offset: 0x00492865
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
