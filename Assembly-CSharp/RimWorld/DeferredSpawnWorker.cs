using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120E RID: 4622
	public abstract class DeferredSpawnWorker : IExposable
	{
		// Token: 0x060072CB RID: 29387
		public abstract void OnSpawn(Thing thing);

		// Token: 0x060072CC RID: 29388 RVA: 0x002823DD File Offset: 0x002805DD
		public void ExposeData()
		{
		}
	}
}
