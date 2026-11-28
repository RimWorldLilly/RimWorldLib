using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147C RID: 5244
	public class LifeStageWorker
	{
		// Token: 0x060080B3 RID: 32947 RVA: 0x002DE73D File Offset: 0x002DC93D
		public virtual void Notify_LifeStageStarted(Pawn pawn, LifeStageDef previousLifeStage)
		{
			pawn.health.capacities.Notify_CapacityLevelsDirty();
		}

		// Token: 0x0400502F RID: 20527
		public LifeStageDef def;
	}
}
