using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058E RID: 1422
	public class MoteLeaf : Mote
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x001007F7 File Offset: 0x000FE9F7
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x00100829 File Offset: 0x000FEA29
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x0010083C File Offset: 0x000FEA3C
		public override float Alpha
		{
			get
			{
				float num = base.AgeSecs;
				if (num <= this.spawnDelay)
				{
					return 0f;
				}
				num -= this.spawnDelay;
				if (num <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return num / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.FallTime + base.SolidTime)
					{
						return 1f;
					}
					num -= this.FallTime + base.SolidTime;
					if (num <= this.def.mote.fadeOutTime)
					{
						return 1f - Mathf.InverseLerp(0f, this.def.mote.fadeOutTime, num);
					}
					num -= this.def.mote.fadeOutTime;
					return 0f;
				}
			}
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0010091E File Offset: 0x000FEB1E
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x00100948 File Offset: 0x000FEB48
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float ageSecs = base.AgeSecs;
			this.exactPosition = this.startSpatialPosition;
			if (ageSecs > this.spawnDelay)
			{
				this.exactPosition.y = this.exactPosition.y - MoteLeaf.FallSpeed * (ageSecs - this.spawnDelay);
			}
			this.exactPosition.y = Mathf.Max(this.exactPosition.y, 0f);
			this.currentSpatialPosition = this.exactPosition;
			this.exactPosition.z = this.exactPosition.z + this.exactPosition.y;
			this.exactPosition.y = 0f;
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x001009F4 File Offset: 0x000FEBF4
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawMote(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x04001D11 RID: 7441
		private Vector3 startSpatialPosition;

		// Token: 0x04001D12 RID: 7442
		private Vector3 currentSpatialPosition;

		// Token: 0x04001D13 RID: 7443
		private float spawnDelay;

		// Token: 0x04001D14 RID: 7444
		private bool front;

		// Token: 0x04001D15 RID: 7445
		private float treeHeight;

		// Token: 0x04001D16 RID: 7446
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
