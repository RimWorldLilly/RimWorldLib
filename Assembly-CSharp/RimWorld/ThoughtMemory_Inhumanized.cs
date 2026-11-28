using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E91 RID: 3729
	public class ThoughtMemory_Inhumanized : Thought_Memory
	{
		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x0600626E RID: 25198 RVA: 0x00235EA4 File Offset: 0x002340A4
		public override int CurStageIndex
		{
			get
			{
				return (ModsConfig.AnomalyActive && this.pawn.Inhumanized() > false) ? 1 : 0;
			}
		}
	}
}
