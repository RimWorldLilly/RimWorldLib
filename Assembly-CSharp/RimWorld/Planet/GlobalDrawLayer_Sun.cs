using System;
using System.Collections;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200230C RID: 8972
	public class GlobalDrawLayer_Sun : WorldDrawLayerBase
	{
		// Token: 0x170026BC RID: 9916
		// (get) Token: 0x0600DD95 RID: 56725 RVA: 0x00528352 File Offset: 0x00526552
		protected override int RenderLayer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x170026BD RID: 9917
		// (get) Token: 0x0600DD96 RID: 56726 RVA: 0x00528359 File Offset: 0x00526559
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x0600DD97 RID: 56727 RVA: 0x00528365 File Offset: 0x00526565
		public override IEnumerable Regenerate()
		{
			GlobalDrawLayer_Sun.<Regenerate>d__5 <Regenerate>d__ = new GlobalDrawLayer_Sun.<Regenerate>d__5(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008E53 RID: 36435
		private const float SunDrawSize = 15f;
	}
}
