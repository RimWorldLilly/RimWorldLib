using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF5 RID: 6901
	public struct IncineratorProjectileMotion
	{
		// Token: 0x17001FCF RID: 8143
		// (get) Token: 0x0600B061 RID: 45153 RVA: 0x003E20DC File Offset: 0x003E02DC
		public float Alpha
		{
			get
			{
				return Mathf.Clamp01((float)this.ticks / (float)this.lifespanTicks);
			}
		}

		// Token: 0x17001FD0 RID: 8144
		// (get) Token: 0x0600B062 RID: 45154 RVA: 0x003E20F2 File Offset: 0x003E02F2
		public Vector3 LerpdPos
		{
			get
			{
				return Vector3.Lerp(this.worldSource, this.worldTarget, this.Alpha);
			}
		}

		// Token: 0x0600B063 RID: 45155 RVA: 0x003E210C File Offset: 0x003E030C
		public void Tick(Map map)
		{
			this.ticks++;
			Vector3 lerpdPos = this.LerpdPos;
			IntVec3 cell = lerpdPos.ToIntVec3();
			Vector3 offsetA = lerpdPos - cell.ToVector3Shifted();
			Vector3 vector = this.LerpdPos - this.moveVector * 2f;
			IntVec3 cell2 = vector.ToIntVec3();
			Vector3 offsetB = vector - cell2.ToVector3Shifted();
			this.mote.Scale = Mathf.Lerp(this.startScale, this.endScale, this.Alpha);
			this.mote.UpdateTargets(new TargetInfo(cell, map, false), new TargetInfo(cell2, map, false), offsetA, offsetB);
			if (this.Alpha < 1f)
			{
				this.mote.Maintain();
			}
		}

		// Token: 0x040065C7 RID: 26055
		public MoteDualAttached mote;

		// Token: 0x040065C8 RID: 26056
		public Vector3 worldSource;

		// Token: 0x040065C9 RID: 26057
		public Vector3 worldTarget;

		// Token: 0x040065CA RID: 26058
		public IntVec3 targetDest;

		// Token: 0x040065CB RID: 26059
		public Vector3 moveVector;

		// Token: 0x040065CC RID: 26060
		public float startScale;

		// Token: 0x040065CD RID: 26061
		public float endScale;

		// Token: 0x040065CE RID: 26062
		public int ticks;

		// Token: 0x040065CF RID: 26063
		public int lifespanTicks;
	}
}
