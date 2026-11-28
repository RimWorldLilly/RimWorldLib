using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D2 RID: 5586
	public class Thought_AttendedRitual : Thought_Memory
	{
		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x06008AD1 RID: 35537 RVA: 0x00325CDA File Offset: 0x00323EDA
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.sourcePrecept.Named("RITUAL"));
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x06008AD2 RID: 35538 RVA: 0x00325D01 File Offset: 0x00323F01
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.sourcePrecept.Named("RITUAL"));
			}
		}
	}
}
