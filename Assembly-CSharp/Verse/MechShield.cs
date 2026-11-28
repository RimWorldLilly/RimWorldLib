using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200054F RID: 1359
	public class MechShield : ThingWithComps
	{
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x000F9F20 File Offset: 0x000F8120
		public override Vector3 DrawPos
		{
			get
			{
				if (this.target == null)
				{
					return base.DrawPos;
				}
				return this.target.DrawPos;
			}
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000F9F3C File Offset: 0x000F813C
		public void SetTarget(Thing target)
		{
			this.target = target;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000F9F45 File Offset: 0x000F8145
		public bool IsTargeting(Thing target)
		{
			return this.target == target;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000F9F50 File Offset: 0x000F8150
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000F9F58 File Offset: 0x000F8158
		protected override void Tick()
		{
			base.Tick();
			if (this.target != null)
			{
				base.Position = this.target.Position;
			}
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000F9F79 File Offset: 0x000F8179
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x04001C93 RID: 7315
		private Thing target;
	}
}
