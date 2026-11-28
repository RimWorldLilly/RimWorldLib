using System;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000E9 RID: 233
	public static class ToilMaker
	{
		// Token: 0x0600087C RID: 2172 RVA: 0x0003069A File Offset: 0x0002E89A
		public static Toil MakeToil([CallerMemberName] string debugName = null)
		{
			Toil toil = SimplePool<Toil>.Get();
			if (!toil.inPool)
			{
				Log.Error("Toil was marked as not in pool while coming from pool");
			}
			toil.inPool = false;
			toil.debugName = debugName;
			return toil;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000306C1 File Offset: 0x0002E8C1
		public static void ReturnToPool(this Toil toil)
		{
			toil.Clear();
			if (toil.inPool)
			{
				Log.Error("Toil was marked as already being in the pool while being returned to pool");
				return;
			}
			if (SimplePool<Toil>.FreeItemsCount >= 5000)
			{
				return;
			}
			toil.inPool = true;
			SimplePool<Toil>.Return(toil);
		}

		// Token: 0x040003CA RID: 970
		private const int MaxToilPoolSize = 5000;
	}
}
