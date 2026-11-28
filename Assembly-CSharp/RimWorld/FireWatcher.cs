using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001211 RID: 4625
	public class FireWatcher
	{
		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x060072DC RID: 29404 RVA: 0x00282750 File Offset: 0x00280950
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x060072DD RID: 29405 RVA: 0x00282758 File Offset: 0x00280958
		public bool LargeFireDangerPresent
		{
			get
			{
				if (this.fireDanger < 0f)
				{
					this.UpdateObservations();
				}
				return this.fireDanger > 90f;
			}
		}

		// Token: 0x060072DE RID: 29406 RVA: 0x0028277A File Offset: 0x0028097A
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x060072DF RID: 29407 RVA: 0x00282794 File Offset: 0x00280994
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x060072E0 RID: 29408 RVA: 0x002827B0 File Offset: 0x002809B0
		private void UpdateObservations()
		{
			this.fireDanger = 0f;
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.Fire);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				this.fireDanger += 0.5f + fire.fireSize;
			}
		}

		// Token: 0x040048A7 RID: 18599
		private Map map;

		// Token: 0x040048A8 RID: 18600
		private float fireDanger = -1f;

		// Token: 0x040048A9 RID: 18601
		private const int UpdateObservationsInterval = 426;

		// Token: 0x040048AA RID: 18602
		private const float BaseDangerPerFire = 0.5f;
	}
}
