using System;

namespace RimWorld
{
	// Token: 0x02000EB1 RID: 3761
	public class BookOutcomeProperties_GiveQuest : BookOutcomeProperties
	{
		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06006307 RID: 25351 RVA: 0x0023A215 File Offset: 0x00238415
		public override Type DoerClass
		{
			get
			{
				return typeof(BookOutcomeDoer_GiveQuest);
			}
		}

		// Token: 0x04003699 RID: 13977
		public float questChance;
	}
}
