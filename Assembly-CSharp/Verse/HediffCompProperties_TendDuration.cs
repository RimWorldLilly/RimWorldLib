using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000482 RID: 1154
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x000C18FF File Offset: 0x000BFAFF
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000C190E File Offset: 0x000BFB0E
		public int TendTicksFull
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksFull on permanent-tend Hediff.", 6163263);
				}
				return Mathf.RoundToInt((this.baseTendDurationHours + this.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x000C193F File Offset: 0x000BFB3F
		public int TendTicksBase
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksBase on permanent-tend Hediff.", 61621263);
				}
				return Mathf.RoundToInt(this.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x000C1969 File Offset: 0x000BFB69
		public int TendTicksOverlap
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksOverlap on permanent-tend Hediff.", 1963263);
				}
				return Mathf.RoundToInt(this.tendOverlapHours * 2500f);
			}
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x000C1993 File Offset: 0x000BFB93
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x040018F5 RID: 6389
		private float baseTendDurationHours = -1f;

		// Token: 0x040018F6 RID: 6390
		private float tendOverlapHours = 3f;

		// Token: 0x040018F7 RID: 6391
		public bool tendAllAtOnce;

		// Token: 0x040018F8 RID: 6392
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x040018F9 RID: 6393
		public float severityPerDayTended;

		// Token: 0x040018FA RID: 6394
		public bool showTendQuality = true;

		// Token: 0x040018FB RID: 6395
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		// Token: 0x040018FC RID: 6396
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		// Token: 0x040018FD RID: 6397
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
