using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BA RID: 5562
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x060089DC RID: 35292 RVA: 0x0031E996 File Offset: 0x0031CB96
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x060089DD RID: 35293 RVA: 0x0031E9A6 File Offset: 0x0031CBA6
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x060089DE RID: 35294 RVA: 0x0031E9AE File Offset: 0x0031CBAE
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x060089DF RID: 35295 RVA: 0x0031E9BB File Offset: 0x0031CBBB
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x060089E0 RID: 35296 RVA: 0x0031E9CC File Offset: 0x0031CBCC
		public void SetForced(Apparel ap, bool forced)
		{
			if (forced)
			{
				if (!this.forcedAps.Contains(ap))
				{
					this.forcedAps.Add(ap);
					return;
				}
			}
			else if (this.forcedAps.Contains(ap))
			{
				this.forcedAps.Remove(ap);
			}
		}

		// Token: 0x060089E1 RID: 35297 RVA: 0x0031EA07 File Offset: 0x0031CC07
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x060089E2 RID: 35298 RVA: 0x0031EA20 File Offset: 0x0031CC20
		public bool IsForced(Apparel ap)
		{
			if (ap.Destroyed)
			{
				Log.Error("Apparel was forced while Destroyed: " + ((ap != null) ? ap.ToString() : null));
				if (this.forcedAps.Contains(ap))
				{
					this.forcedAps.Remove(ap);
				}
				return false;
			}
			return this.forcedAps.Contains(ap);
		}

		// Token: 0x040054E0 RID: 21728
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
