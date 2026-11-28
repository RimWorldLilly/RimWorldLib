using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135F RID: 4959
	[StaticConstructorOnStartup]
	public class CompPowerPlantSolar : CompPowerPlant
	{
		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x06007AD5 RID: 31445 RVA: 0x002BB966 File Offset: 0x002B9B66
		protected override float DesiredPowerOutput
		{
			get
			{
				return Mathf.Lerp(0f, -base.Props.PowerConsumption, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x06007AD6 RID: 31446 RVA: 0x002BB99C File Offset: 0x002B9B9C
		private float RoofedPowerOutputFactor
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.parent.OccupiedRect())
				{
					num++;
					if (this.parent.Map.roofGrid.Roofed(c))
					{
						num2++;
					}
				}
				return (float)(num - num2) / (float)num;
			}
		}

		// Token: 0x06007AD7 RID: 31447 RVA: 0x002BBA20 File Offset: 0x002B9C20
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.parent.DrawPos + Vector3.up * 0.1f;
			r.size = CompPowerPlantSolar.BarSize;
			r.fillPercent = base.PowerOutput / -base.Props.PowerConsumption;
			r.filledMat = CompPowerPlantSolar.PowerPlantSolarBarFilledMat;
			r.unfilledMat = CompPowerPlantSolar.PowerPlantSolarBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}

		// Token: 0x04004D85 RID: 19845
		private const float NightPower = 0f;

		// Token: 0x04004D86 RID: 19846
		private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

		// Token: 0x04004D87 RID: 19847
		private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04004D88 RID: 19848
		private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
	}
}
