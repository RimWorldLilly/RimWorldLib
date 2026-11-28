using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001207 RID: 4615
	public class Area_PollutionClear : Area
	{
		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x060072AC RID: 29356 RVA: 0x00281AC3 File Offset: 0x0027FCC3
		public override string Label
		{
			get
			{
				return "PollutionClear".Translate();
			}
		}

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x060072AD RID: 29357 RVA: 0x00281AD4 File Offset: 0x0027FCD4
		public override Color Color
		{
			get
			{
				return new Color(0.1f, 0.8f, 0.1f);
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x060072AE RID: 29358 RVA: 0x00281AEA File Offset: 0x0027FCEA
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x060072AF RID: 29359 RVA: 0x00281AF1 File Offset: 0x0027FCF1
		public Area_PollutionClear()
		{
		}

		// Token: 0x060072B0 RID: 29360 RVA: 0x00281AF9 File Offset: 0x0027FCF9
		public Area_PollutionClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x060072B1 RID: 29361 RVA: 0x00281B02 File Offset: 0x0027FD02
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_PollutionClear";
		}
	}
}
