using System;

namespace RimWorld
{
	// Token: 0x02001DFE RID: 7678
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x0600C78E RID: 51086 RVA: 0x0049468C File Offset: 0x0049288C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x0600C78F RID: 51087 RVA: 0x004946B0 File Offset: 0x004928B0
		private int GetValueFor(Transferable t)
		{
			QualityCategory result;
			if (!t.AnyThing.TryGetQuality(out result))
			{
				return -1;
			}
			return (int)result;
		}
	}
}
