using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001204 RID: 4612
	public class Area_BuildRoof : Area
	{
		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x0600729A RID: 29338 RVA: 0x002819A4 File Offset: 0x0027FBA4
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x0600729B RID: 29339 RVA: 0x002819B5 File Offset: 0x0027FBB5
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x0600729C RID: 29340 RVA: 0x002819CB File Offset: 0x0027FBCB
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x002819D2 File Offset: 0x0027FBD2
		public Area_BuildRoof()
		{
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x002819DA File Offset: 0x0027FBDA
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x002819E3 File Offset: 0x0027FBE3
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_BuildRoof";
		}
	}
}
