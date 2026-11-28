using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006C8 RID: 1736
	public abstract class SubEffecter_CameraShake : SubEffecter
	{
		// Token: 0x06003703 RID: 14083 RVA: 0x00160A4C File Offset: 0x0015EC4C
		public SubEffecter_CameraShake(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x00160A56 File Offset: 0x0015EC56
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			if (force)
			{
				this.DoShake(A);
			}
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x00160A64 File Offset: 0x0015EC64
		public void DoShake(TargetInfo tgt)
		{
			float num = 1f - Find.CameraDriver.ZoomRootSize.Remap(11f, 60f, 0f, 1f);
			IntVec3 mapPosition = Find.CameraDriver.MapPosition;
			float lengthHorizontal = (tgt.Cell - mapPosition).LengthHorizontal;
			float num2 = 1f - Mathf.Clamp01(lengthHorizontal / this.def.distanceAttenuationMax);
			num2 *= num;
			float randomInRange = this.def.cameraShake.RandomInRange;
			Find.CameraDriver.shaker.DoShake(randomInRange * Mathf.Lerp(1f, num2, this.def.distanceAttenuationScale));
			if (this.def.soundDef != null)
			{
				this.def.soundDef.PlayOneShot(tgt);
			}
		}
	}
}
