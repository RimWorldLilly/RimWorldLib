using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001206 RID: 4614
	public class Area_SnowOrSandClear : Area
	{
		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x060072A6 RID: 29350 RVA: 0x00281A5A File Offset: 0x0027FC5A
		public override string Label
		{
			get
			{
				return (ModsConfig.OdysseyActive ? "SnowAndSandClear" : "SnowClear").Translate();
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x060072A7 RID: 29351 RVA: 0x00281A79 File Offset: 0x0027FC79
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x060072A8 RID: 29352 RVA: 0x00281A8F File Offset: 0x0027FC8F
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x00281A96 File Offset: 0x0027FC96
		public Area_SnowOrSandClear()
		{
		}

		// Token: 0x060072AA RID: 29354 RVA: 0x00281A9E File Offset: 0x0027FC9E
		public Area_SnowOrSandClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x060072AB RID: 29355 RVA: 0x00281AA7 File Offset: 0x0027FCA7
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_SnowClear";
		}
	}
}
