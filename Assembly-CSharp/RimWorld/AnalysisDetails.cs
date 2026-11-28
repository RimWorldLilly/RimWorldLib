using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101B RID: 4123
	public class AnalysisDetails : IExposable
	{
		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x060067D7 RID: 26583 RVA: 0x0024ABE6 File Offset: 0x00248DE6
		public bool Satisfied
		{
			get
			{
				return this.timesDone >= this.required;
			}
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x0024ABF9 File Offset: 0x00248DF9
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.required, "required", 0, false);
			Scribe_Values.Look<int>(ref this.timesDone, "timesDone", 0, false);
		}

		// Token: 0x04004241 RID: 16961
		public int id;

		// Token: 0x04004242 RID: 16962
		public int required;

		// Token: 0x04004243 RID: 16963
		public int timesDone;
	}
}
