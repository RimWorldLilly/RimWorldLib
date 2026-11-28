using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200230B RID: 8971
	public class GlobalDrawLayer_Stars : WorldDrawLayerBase
	{
		// Token: 0x170026B8 RID: 9912
		// (get) Token: 0x0600DD8D RID: 56717 RVA: 0x005282AB File Offset: 0x005264AB
		protected override int RenderLayer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x170026B9 RID: 9913
		// (get) Token: 0x0600DD8E RID: 56718 RVA: 0x005282B2 File Offset: 0x005264B2
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		// Token: 0x170026BA RID: 9914
		// (get) Token: 0x0600DD8F RID: 56719 RVA: 0x005282ED File Offset: 0x005264ED
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		// Token: 0x170026BB RID: 9915
		// (get) Token: 0x0600DD90 RID: 56720 RVA: 0x005282F7 File Offset: 0x005264F7
		protected override Quaternion Rotation
		{
			get
			{
				if (this.UseStaticRotation)
				{
					return Quaternion.identity;
				}
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x0600DD91 RID: 56721 RVA: 0x00528311 File Offset: 0x00526511
		public override IEnumerable Regenerate()
		{
			GlobalDrawLayer_Stars.<Regenerate>d__14 <Regenerate>d__ = new GlobalDrawLayer_Stars.<Regenerate>d__14(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008E4D RID: 36429
		private bool calculatedForStaticRotation;

		// Token: 0x04008E4E RID: 36430
		private PlanetTile calculatedForStartingTile = PlanetTile.Invalid;

		// Token: 0x04008E4F RID: 36431
		public const float DistanceToStars = 20f;

		// Token: 0x04008E50 RID: 36432
		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		// Token: 0x04008E51 RID: 36433
		private const int StarsCount = 1500;

		// Token: 0x04008E52 RID: 36434
		private const float DistToSunToReduceStarSize = 0.8f;
	}
}
