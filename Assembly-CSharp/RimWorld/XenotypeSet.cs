using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001018 RID: 4120
	public class XenotypeSet
	{
		// Token: 0x170011D1 RID: 4561
		public XenotypeChance this[int index]
		{
			get
			{
				return this.xenotypeChances[index];
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x060067CD RID: 26573 RVA: 0x0024AA59 File Offset: 0x00248C59
		public int Count
		{
			get
			{
				return this.xenotypeChances.Count;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x060067CE RID: 26574 RVA: 0x0024AA68 File Offset: 0x00248C68
		public float BaselinerChance
		{
			get
			{
				if (!ModsConfig.BiotechActive || this.xenotypeChances.NullOrEmpty<XenotypeChance>())
				{
					return 1f;
				}
				float num = 0f;
				for (int i = 0; i < this.xenotypeChances.Count; i++)
				{
					if (this.xenotypeChances[i].xenotype != XenotypeDefOf.Baseliner)
					{
						num += this.xenotypeChances[i].chance;
					}
				}
				return 1f - num;
			}
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x0024AAE0 File Offset: 0x00248CE0
		public bool Contains(XenotypeDef xenotype)
		{
			if (xenotype == XenotypeDefOf.Baseliner && this.BaselinerChance > 0f)
			{
				return true;
			}
			for (int i = 0; i < this.xenotypeChances.Count; i++)
			{
				if (this.xenotypeChances[i].xenotype == xenotype)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004237 RID: 16951
		private List<XenotypeChance> xenotypeChances = new List<XenotypeChance>();
	}
}
