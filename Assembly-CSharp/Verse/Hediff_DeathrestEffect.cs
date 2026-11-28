using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004AC RID: 1196
	public class Hediff_DeathrestEffect : Hediff
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x000C5D58 File Offset: 0x000C3F58
		public override string LabelBase
		{
			get
			{
				return base.LabelBase + " x" + Mathf.FloorToInt(this.Severity).ToString();
			}
		}
	}
}
