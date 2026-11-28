using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001294 RID: 4756
	public class GenStep_AnimaTrees : GenStep_SpecialTrees
	{
		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x060075F7 RID: 30199 RVA: 0x00297CF0 File Offset: 0x00295EF0
		public override int SeedPart
		{
			get
			{
				return 647816171;
			}
		}

		// Token: 0x060075F8 RID: 30200 RVA: 0x00297CF7 File Offset: 0x00295EF7
		public override int DesiredTreeCountForMap(Map map)
		{
			return Mathf.Max(Mathf.RoundToInt(GenStep_AnimaTrees.Density * (float)map.Area), 1);
		}

		// Token: 0x060075F9 RID: 30201 RVA: 0x00297D14 File Offset: 0x00295F14
		protected override float GetGrowth()
		{
			return GenStep_AnimaTrees.GrowthRange.RandomInRange;
		}

		// Token: 0x040049F2 RID: 18930
		public static readonly float Density = 1.25E-05f;

		// Token: 0x040049F3 RID: 18931
		private static readonly FloatRange GrowthRange = new FloatRange(0.5f, 0.75f);
	}
}
