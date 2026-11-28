using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001205 RID: 4613
	public class Area_NoRoof : Area
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x060072A0 RID: 29344 RVA: 0x002819FF File Offset: 0x0027FBFF
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x060072A1 RID: 29345 RVA: 0x00281A10 File Offset: 0x0027FC10
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x060072A2 RID: 29346 RVA: 0x00281A26 File Offset: 0x0027FC26
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x060072A3 RID: 29347 RVA: 0x00281A2D File Offset: 0x0027FC2D
		public Area_NoRoof()
		{
		}

		// Token: 0x060072A4 RID: 29348 RVA: 0x00281A35 File Offset: 0x0027FC35
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x060072A5 RID: 29349 RVA: 0x00281A3E File Offset: 0x0027FC3E
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_NoRoof";
		}
	}
}
