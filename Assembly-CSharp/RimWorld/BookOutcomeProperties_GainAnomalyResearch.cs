using System;

namespace RimWorld
{
	// Token: 0x02000EAB RID: 3755
	public class BookOutcomeProperties_GainAnomalyResearch : BookOutcomeProperties_GainResearch
	{
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060062DD RID: 25309 RVA: 0x002393FF File Offset: 0x002375FF
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerGainAnomalyResearch);
			}
		}
	}
}
