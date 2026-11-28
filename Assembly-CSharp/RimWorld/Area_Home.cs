using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001203 RID: 4611
	public class Area_Home : Area
	{
		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06007292 RID: 29330 RVA: 0x00281920 File Offset: 0x0027FB20
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06007293 RID: 29331 RVA: 0x00281931 File Offset: 0x0027FB31
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06007294 RID: 29332 RVA: 0x00281947 File Offset: 0x0027FB47
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06007295 RID: 29333 RVA: 0x0028194E File Offset: 0x0027FB4E
		public Area_Home()
		{
		}

		// Token: 0x06007296 RID: 29334 RVA: 0x00281956 File Offset: 0x0027FB56
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06007297 RID: 29335 RVA: 0x0028195F File Offset: 0x0027FB5F
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06007298 RID: 29336 RVA: 0x00281962 File Offset: 0x0027FB62
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_Home";
		}

		// Token: 0x06007299 RID: 29337 RVA: 0x0028197E File Offset: 0x0027FB7E
		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] == val)
			{
				return;
			}
			base.Set(c, val);
			base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
		}
	}
}
