using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA0 RID: 7072
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x170020D3 RID: 8403
		// (get) Token: 0x0600B596 RID: 46486 RVA: 0x003FF6FE File Offset: 0x003FD8FE
		protected override Color BGColor
		{
			get
			{
				return Alert_Critical.BgColor();
			}
		}

		// Token: 0x0600B597 RID: 46487 RVA: 0x003FF705 File Offset: 0x003FD905
		public static Color BgColor()
		{
			float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
			return new Color(num, num, num) * Color.red;
		}

		// Token: 0x170020D4 RID: 8404
		// (get) Token: 0x0600B598 RID: 46488 RVA: 0x003FF731 File Offset: 0x003FD931
		protected virtual bool DoMessage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B599 RID: 46489 RVA: 0x003FF734 File Offset: 0x003FD934
		public override void AlertActiveUpdate()
		{
			if (this.DoMessage && this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(base.Label.CapitalizeFirst()), new LookTargets(this.GetReport().AllCulprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}

		// Token: 0x0600B59A RID: 46490 RVA: 0x003FF7A0 File Offset: 0x003FD9A0
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x0400687E RID: 26750
		private int lastActiveFrame = -1;

		// Token: 0x0400687F RID: 26751
		private const float PulseFreq = 0.5f;

		// Token: 0x04006880 RID: 26752
		private const float PulseAmpCritical = 0.6f;
	}
}
