using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004BB RID: 1211
	public class ImplantSource : IExposable
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x000C81F5 File Offset: 0x000C63F5
		public bool WasPawn
		{
			get
			{
				return this.sourcePawn != null;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x000C8200 File Offset: 0x000C6400
		public Pawn Pawn
		{
			get
			{
				return this.sourcePawn;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000C8208 File Offset: 0x000C6408
		public int Biosignature
		{
			get
			{
				return this.biosignature;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x000C8210 File Offset: 0x000C6410
		public string BiosignatureName
		{
			get
			{
				string result;
				if ((result = this.cachedName) == null)
				{
					result = (this.cachedName = AnomalyUtility.GetBiosignatureName(this.biosignature));
				}
				return result;
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000C823B File Offset: 0x000C643B
		public ImplantSource()
		{
			this.biosignature = Rand.Int;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000C8250 File Offset: 0x000C6450
		public ImplantSource(Pawn pawn, string descKey = null, string descResolved = null)
		{
			this.sourcePawn = pawn;
			this.descKey = descKey;
			this.descResolved = descResolved;
			Hediff_MetalhorrorImplant hediff_MetalhorrorImplant;
			if (pawn != null && pawn.health.hediffSet.TryGetHediff<Hediff_MetalhorrorImplant>(out hediff_MetalhorrorImplant))
			{
				this.biosignature = hediff_MetalhorrorImplant.Biosignature;
				return;
			}
			this.biosignature = Rand.Int;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000C82A8 File Offset: 0x000C64A8
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.descKey, "descKeyDesc", null, false);
			Scribe_Values.Look<string>(ref this.descResolved, "descResolved", null, false);
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
			Scribe_References.Look<Pawn>(ref this.sourcePawn, "sourcePawn", true);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000C82FC File Offset: 0x000C64FC
		public void DebugSetBiosignature(int newBiosignature)
		{
			this.biosignature = newBiosignature;
			this.cachedName = null;
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000C830C File Offset: 0x000C650C
		public string GetSourceDesc()
		{
			if (!string.IsNullOrEmpty(this.descResolved))
			{
				return this.descResolved;
			}
			if (string.IsNullOrEmpty(this.descKey))
			{
				return "ImplantSourceUnknown".Translate();
			}
			if (this.sourcePawn != null)
			{
				return this.descKey.Translate(this.sourcePawn.Named("SOURCE"));
			}
			return this.descKey.Translate();
		}

		// Token: 0x0400198C RID: 6540
		private Pawn sourcePawn;

		// Token: 0x0400198D RID: 6541
		private string descKey;

		// Token: 0x0400198E RID: 6542
		private string descResolved;

		// Token: 0x0400198F RID: 6543
		private int biosignature;

		// Token: 0x04001990 RID: 6544
		private string cachedName;
	}
}
