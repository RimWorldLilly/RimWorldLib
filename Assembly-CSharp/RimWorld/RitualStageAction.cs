using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171A RID: 5914
	public abstract class RitualStageAction : IExposable
	{
		// Token: 0x060093AA RID: 37802 RVA: 0x0035E24A File Offset: 0x0035C44A
		public virtual void Apply(LordJob_Ritual ritual)
		{
		}

		// Token: 0x060093AB RID: 37803 RVA: 0x0035E24C File Offset: 0x0035C44C
		public virtual void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
		}

		// Token: 0x060093AC RID: 37804
		public abstract void ExposeData();
	}
}
