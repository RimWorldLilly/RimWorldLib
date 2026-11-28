using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB8 RID: 3768
	public abstract class ReadingOutcomeProperties
	{
		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06006330 RID: 25392
		public abstract Type DoerClass { get; }

		// Token: 0x06006331 RID: 25393 RVA: 0x0023A6F8 File Offset: 0x002388F8
		public ReadingOutcomeProperties()
		{
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x0023A710 File Offset: 0x00238910
		public ReadingOutcomeProperties(Type doerClass)
		{
			this.doerClass = doerClass;
		}

		// Token: 0x040036A5 RID: 13989
		[TranslationHandle]
		public Type doerClass = typeof(ReadingOutcomeDoer);
	}
}
