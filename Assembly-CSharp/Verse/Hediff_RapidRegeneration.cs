using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C5 RID: 1221
	public class Hediff_RapidRegeneration : Hediff
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x000C9F2C File Offset: 0x000C812C
		public override string SeverityLabel
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.hpRemaining, "0");
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.hpCapacity, "0");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("HP".Translate());
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x000C9F8A File Offset: 0x000C818A
		public override bool ShouldRemove
		{
			get
			{
				return this.hpRemaining <= 0f;
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000C9F9C File Offset: 0x000C819C
		public void SetHpCapacity(float amount)
		{
			this.hpCapacity = amount;
			this.hpRemaining = amount;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000C9FB9 File Offset: 0x000C81B9
		public override void Notify_Regenerated(float hp)
		{
			this.hpRemaining = Mathf.Max(this.hpRemaining - hp, 0f);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000C9FD3 File Offset: 0x000C81D3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.hpRemaining, "hpRemaining", 0f, false);
			Scribe_Values.Look<float>(ref this.hpCapacity, "hpCapacity", 0f, false);
		}

		// Token: 0x040019A2 RID: 6562
		private float hpRemaining = 100f;

		// Token: 0x040019A3 RID: 6563
		private float hpCapacity = 100f;
	}
}
