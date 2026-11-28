using System;

namespace RimWorld
{
	// Token: 0x02000EB3 RID: 3763
	public class BookOutcomeProperties_JoyFactorModifier : BookOutcomeProperties
	{
		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06006319 RID: 25369 RVA: 0x0023A56E File Offset: 0x0023876E
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerJoyFactorModifier);
			}
		}
	}
}
