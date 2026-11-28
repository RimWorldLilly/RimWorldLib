using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029B RID: 667
	public class CellFXSpawner
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x00075437 File Offset: 0x00073637
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x0007543F File Offset: 0x0007363F
		public List<IntVec3> Cells { get; private set; }

		// Token: 0x06001475 RID: 5237 RVA: 0x00075448 File Offset: 0x00073648
		public CellFXSpawner(float fxPerTilePerSec, Action<Vector3> emitterFunc)
		{
			this.Cells = new List<IntVec3>();
			this.emitterFunc = emitterFunc;
			this.fxPerTilePerTick = fxPerTilePerSec / 60f;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00075470 File Offset: 0x00073670
		public void Tick()
		{
			this.queue += this.fxPerTilePerTick * (float)this.Cells.Count;
			while (this.queue >= 1f)
			{
				this.emitterFunc(this.Cells.RandomElement<IntVec3>().ToVector3Shifted());
				this.queue -= 1f;
			}
		}

		// Token: 0x04001217 RID: 4631
		private float fxPerTilePerTick;

		// Token: 0x04001218 RID: 4632
		private Action<Vector3> emitterFunc;

		// Token: 0x04001219 RID: 4633
		private float queue;
	}
}
