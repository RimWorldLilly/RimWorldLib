using System;

namespace RimWorld
{
	// Token: 0x02000EA6 RID: 3750
	public abstract class BookOutcomeProperties : ReadingOutcomeProperties
	{
		// Token: 0x060062CB RID: 25291 RVA: 0x002390D0 File Offset: 0x002372D0
		public BookOutcomeProperties()
		{
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x002390D8 File Offset: 0x002372D8
		public BookOutcomeProperties(Type doerClass)
		{
			this.doerClass = doerClass;
		}
	}
}
