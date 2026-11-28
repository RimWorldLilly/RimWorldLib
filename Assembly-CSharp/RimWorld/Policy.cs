using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BC RID: 5564
	public abstract class Policy : IExposable, ILoadReferenceable, IRenameable
	{
		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x060089E9 RID: 35305 RVA: 0x0031EB19 File Offset: 0x0031CD19
		// (set) Token: 0x060089EA RID: 35306 RVA: 0x0031EB21 File Offset: 0x0031CD21
		public string RenamableLabel
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x060089EB RID: 35307 RVA: 0x0031EB2A File Offset: 0x0031CD2A
		public string BaseLabel
		{
			get
			{
				return "UnnamedPolicy".Translate();
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x060089EC RID: 35308 RVA: 0x0031EB3B File Offset: 0x0031CD3B
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x060089ED RID: 35309
		protected abstract string LoadKey { get; }

		// Token: 0x060089EE RID: 35310 RVA: 0x0031EB43 File Offset: 0x0031CD43
		protected Policy()
		{
		}

		// Token: 0x060089EF RID: 35311 RVA: 0x0031EB4B File Offset: 0x0031CD4B
		protected Policy(int id, string label)
		{
			this.id = id;
			this.label = label;
		}

		// Token: 0x060089F0 RID: 35312
		public abstract void CopyFrom(Policy other);

		// Token: 0x060089F1 RID: 35313 RVA: 0x0031EB61 File Offset: 0x0031CD61
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
		}

		// Token: 0x060089F2 RID: 35314 RVA: 0x0031EB88 File Offset: 0x0031CD88
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted(this.LoadKey);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted(this.label);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040054E3 RID: 21731
		public int id;

		// Token: 0x040054E4 RID: 21732
		public string label;
	}
}
