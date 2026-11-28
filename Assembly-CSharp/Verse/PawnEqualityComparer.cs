using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004D5 RID: 1237
	public class PawnEqualityComparer : IEqualityComparer<Pawn>
	{
		// Token: 0x06002293 RID: 8851 RVA: 0x000CCE0B File Offset: 0x000CB00B
		public bool Equals(Pawn x, Pawn y)
		{
			return x.Equals(y);
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000CCE14 File Offset: 0x000CB014
		public int GetHashCode(Pawn obj)
		{
			return obj.GetHashCode();
		}
	}
}
