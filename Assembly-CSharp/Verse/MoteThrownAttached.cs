using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000591 RID: 1425
	internal class MoteThrownAttached : MoteThrown
	{
		// Token: 0x06002A11 RID: 10769 RVA: 0x00100EAC File Offset: 0x000FF0AC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.link1.Linked)
			{
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x00100F00 File Offset: 0x000FF100
		protected override Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = base.NextExactPosition(deltaTime);
			if (this.link1.Linked)
			{
				bool flag = this.detachAfterTicks == -1 || Find.TickManager.TicksGame - this.spawnTick < this.detachAfterTicks;
				if (!this.link1.Target.ThingDestroyed && flag)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.link1.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			return vector;
		}

		// Token: 0x04001D1C RID: 7452
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
	}
}
