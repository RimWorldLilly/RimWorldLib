using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF4 RID: 6900
	public class IncineratorSpray : Thing
	{
		// Token: 0x17001FCD RID: 8141
		// (get) Token: 0x0600B05C RID: 45148 RVA: 0x003E2006 File Offset: 0x003E0206
		private int NumAlive
		{
			get
			{
				return this.projectiles.Count;
			}
		}

		// Token: 0x17001FCE RID: 8142
		// (get) Token: 0x0600B05D RID: 45149 RVA: 0x003E2013 File Offset: 0x003E0213
		private bool AllComplete
		{
			get
			{
				return this.numComplete >= this.NumAlive;
			}
		}

		// Token: 0x0600B05E RID: 45150 RVA: 0x003E2028 File Offset: 0x003E0228
		protected override void Tick()
		{
			int i = 0;
			int count = this.projectiles.Count;
			while (i < count)
			{
				IncineratorProjectileMotion value = this.projectiles[i];
				int num = Mathf.Approximately(value.Alpha, 1f) ? 1 : 0;
				value.Tick(base.MapHeld);
				bool flag = Mathf.Approximately(value.Alpha, 1f);
				this.projectiles[i] = value;
				if (num == 0 && flag)
				{
					this.numComplete++;
				}
				i++;
			}
			if (this.AllComplete)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B05F RID: 45151 RVA: 0x003E20BB File Offset: 0x003E02BB
		public void Add(IncineratorProjectileMotion proj)
		{
			this.projectiles.Add(proj);
		}

		// Token: 0x040065C5 RID: 26053
		private List<IncineratorProjectileMotion> projectiles = new List<IncineratorProjectileMotion>();

		// Token: 0x040065C6 RID: 26054
		private int numComplete;
	}
}
