using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB5 RID: 3765
	public class BookOutcomeProperties_MentalBreak : BookOutcomeProperties
	{
		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06006320 RID: 25376 RVA: 0x0023A611 File Offset: 0x00238811
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerMentalBreak);
			}
		}

		// Token: 0x040036A0 RID: 13984
		public FloatRange chancePerHourRange = new FloatRange(0f, 0.2f);

		// Token: 0x040036A1 RID: 13985
		public FloatRange chanceMinMaxRange = new FloatRange(0.01f, 0.9f);
	}
}
