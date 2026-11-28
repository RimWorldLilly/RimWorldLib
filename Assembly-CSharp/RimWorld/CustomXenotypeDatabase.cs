using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101F RID: 4127
	public sealed class CustomXenotypeDatabase : IExposable
	{
		// Token: 0x060067EC RID: 26604 RVA: 0x0024AEE4 File Offset: 0x002490E4
		public void ExposeData()
		{
			Scribe_Collections.Look<CustomXenotype>(ref this.customXenotypes, "customXenotypes", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.customXenotypes == null)
			{
				this.customXenotypes = new List<CustomXenotype>();
			}
		}

		// Token: 0x04004249 RID: 16969
		public List<CustomXenotype> customXenotypes = new List<CustomXenotype>();
	}
}
