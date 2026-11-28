using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200100C RID: 4108
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x06006795 RID: 26517
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
