using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101E RID: 4126
	public sealed class CustomXenogermDatabase : IExposable
	{
		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x060067E7 RID: 26599 RVA: 0x0024AE36 File Offset: 0x00249036
		public List<CustomXenogerm> CustomXenogermsForReading
		{
			get
			{
				return this.customXenogerms;
			}
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x0024AE40 File Offset: 0x00249040
		public void Add(CustomXenogerm customXenogerm)
		{
			if (!ModLister.CheckBiotech("Custom xenogerm database"))
			{
				return;
			}
			this.customXenogerms.RemoveAll((CustomXenogerm c) => c.name == customXenogerm.name);
			this.customXenogerms.Add(customXenogerm);
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x0024AE90 File Offset: 0x00249090
		public bool Remove(CustomXenogerm customXenogerm)
		{
			return this.customXenogerms.Remove(customXenogerm);
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x0024AE9E File Offset: 0x0024909E
		public void ExposeData()
		{
			Scribe_Collections.Look<CustomXenogerm>(ref this.customXenogerms, "customXenogerms", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.customXenogerms == null)
			{
				this.customXenogerms = new List<CustomXenogerm>();
			}
		}

		// Token: 0x04004248 RID: 16968
		private List<CustomXenogerm> customXenogerms = new List<CustomXenogerm>();
	}
}
