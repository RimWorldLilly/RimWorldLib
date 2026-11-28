using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001362 RID: 4962
	[StaticConstructorOnStartup]
	public class CompPowerPlantWind : CompPowerPlant
	{
		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x06007AF1 RID: 31473 RVA: 0x002BC220 File Offset: 0x002BA420
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.cachedPowerOutput;
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x06007AF2 RID: 31474 RVA: 0x002BC228 File Offset: 0x002BA428
		private float PowerPercent
		{
			get
			{
				return base.PowerOutput / (-base.Props.PowerConsumption * 1.5f);
			}
		}

		// Token: 0x06007AF3 RID: 31475 RVA: 0x002BC244 File Offset: 0x002BA444
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CompPowerPlantWind.BarSize = new Vector2((float)this.parent.def.size.z - 0.95f, 0.14f);
			this.RecalculateBlockages();
			this.spinPosition = Rand.Range(0f, 15f);
		}

		// Token: 0x06007AF4 RID: 31476 RVA: 0x002BC2A0 File Offset: 0x002BA4A0
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.windPathCells.Clear();
			Sustainer sustainer = this.sustainer;
			if (sustainer != null && !sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06007AF5 RID: 31477 RVA: 0x002BC2DD File Offset: 0x002BA4DD
		public override void PostSwapMap()
		{
			this.RecalculateBlockages();
		}

		// Token: 0x06007AF6 RID: 31478 RVA: 0x002BC2E5 File Offset: 0x002BA4E5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceWeatherUpdate, "updateCounter", 0, false);
			Scribe_Values.Look<float>(ref this.cachedPowerOutput, "cachedPowerOutput", 0f, false);
		}

		// Token: 0x06007AF7 RID: 31479 RVA: 0x002BC318 File Offset: 0x002BA518
		public override void CompTick()
		{
			base.CompTick();
			if (!base.PowerOn)
			{
				this.cachedPowerOutput = 0f;
				return;
			}
			this.ticksSinceWeatherUpdate++;
			if (this.ticksSinceWeatherUpdate >= this.updateWeatherEveryXTicks)
			{
				float num = Mathf.Min(this.parent.Map.windManager.WindSpeed, 1.5f);
				this.ticksSinceWeatherUpdate = 0;
				this.cachedPowerOutput = -(base.Props.PowerConsumption * num);
				this.RecalculateBlockages();
				if (this.blockedDueToInOrbit)
				{
					this.cachedPowerOutput = 0f;
				}
				else if (this.windPathBlockedCells.Count > 0)
				{
					float num2 = 0f;
					for (int i = 0; i < this.windPathBlockedCells.Count; i++)
					{
						num2 += this.cachedPowerOutput * 0.2f;
					}
					this.cachedPowerOutput -= num2;
					if (this.cachedPowerOutput < 0f)
					{
						this.cachedPowerOutput = 0f;
					}
				}
			}
			if (this.cachedPowerOutput > 0.01f)
			{
				this.spinPosition += this.PowerPercent * CompPowerPlantWind.SpinRateFactor;
			}
			if (this.sustainer == null || this.sustainer.Ended)
			{
				this.sustainer = SoundDefOf.WindTurbine_Ambience.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
			this.sustainer.Maintain();
			this.sustainer.externalParams["PowerOutput"] = this.PowerPercent;
		}

		// Token: 0x06007AF8 RID: 31480 RVA: 0x002BC498 File Offset: 0x002BA698
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = this.parent.DrawPos + Vector3.up * 0.1f,
				size = CompPowerPlantWind.BarSize,
				fillPercent = this.PowerPercent,
				filledMat = CompPowerPlantWind.WindTurbineBarFilledMat,
				unfilledMat = CompPowerPlantWind.WindTurbineBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			Vector3 vector = this.parent.TrueCenter();
			vector += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWind.VerticalBladeOffset;
			vector += this.parent.Rotation.RighthandCell.ToVector3() * CompPowerPlantWind.HorizontalBladeOffset;
			vector.y += 0.03658537f;
			float num = CompPowerPlantWind.BladeWidth * Mathf.Sin(this.spinPosition);
			if (num < 0f)
			{
				num *= -1f;
			}
			bool flag = this.spinPosition % 3.1415927f * 2f < 3.1415927f;
			Vector2 vector2 = new Vector2(num, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
			vector.y -= 0.07317074f;
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
		}

		// Token: 0x06007AF9 RID: 31481 RVA: 0x002BC6A0 File Offset: 0x002BA8A0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (this.windPathBlockedCells.Count > 0)
			{
				stringBuilder.AppendLine();
				Thing thing = null;
				if (this.windPathBlockedByThings != null)
				{
					thing = this.windPathBlockedByThings[0];
				}
				if (thing != null)
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedBy".Translate() + " " + thing.Label);
				}
				else
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedByRoof".Translate());
				}
			}
			if (this.blockedDueToInOrbit)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("CannotFunctionOnLayer".Translate(this.parent.Map.Tile.LayerDef.label).CapitalizeFirst().Colorize(ColoredText.WarningColor));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x002BC798 File Offset: 0x002BA998
		private void RecalculateBlockages()
		{
			if (this.windPathCells.Count == 0)
			{
				IEnumerable<IntVec3> collection = WindTurbineUtility.CalculateWindCells(this.parent.Position, this.parent.Rotation, this.parent.def.size);
				this.windPathCells.AddRange(collection);
			}
			this.windPathBlockedCells.Clear();
			this.windPathBlockedByThings.Clear();
			this.blockedDueToInOrbit = this.parent.Map.Biome.inVacuum;
			for (int i = 0; i < this.windPathCells.Count; i++)
			{
				IntVec3 intVec = this.windPathCells[i];
				if (this.parent.Map.roofGrid.Roofed(intVec))
				{
					this.windPathBlockedByThings.Add(null);
					this.windPathBlockedCells.Add(intVec);
				}
				else
				{
					List<Thing> list = this.parent.Map.thingGrid.ThingsListAt(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.blockWind)
						{
							this.windPathBlockedByThings.Add(thing);
							this.windPathBlockedCells.Add(intVec);
							break;
						}
					}
				}
			}
		}

		// Token: 0x04004D96 RID: 19862
		public int updateWeatherEveryXTicks = 250;

		// Token: 0x04004D97 RID: 19863
		private int ticksSinceWeatherUpdate;

		// Token: 0x04004D98 RID: 19864
		private float cachedPowerOutput;

		// Token: 0x04004D99 RID: 19865
		private List<IntVec3> windPathCells = new List<IntVec3>();

		// Token: 0x04004D9A RID: 19866
		private List<Thing> windPathBlockedByThings = new List<Thing>();

		// Token: 0x04004D9B RID: 19867
		private List<IntVec3> windPathBlockedCells = new List<IntVec3>();

		// Token: 0x04004D9C RID: 19868
		private bool blockedDueToInOrbit;

		// Token: 0x04004D9D RID: 19869
		private float spinPosition;

		// Token: 0x04004D9E RID: 19870
		private Sustainer sustainer;

		// Token: 0x04004D9F RID: 19871
		private const float MaxUsableWindIntensity = 1.5f;

		// Token: 0x04004DA0 RID: 19872
		[TweakValue("Graphics", 0f, 0.1f)]
		private static float SpinRateFactor = 0.035f;

		// Token: 0x04004DA1 RID: 19873
		[TweakValue("Graphics", -1f, 1f)]
		private static float HorizontalBladeOffset = -0.02f;

		// Token: 0x04004DA2 RID: 19874
		[TweakValue("Graphics", 0f, 1f)]
		private static float VerticalBladeOffset = 0.7f;

		// Token: 0x04004DA3 RID: 19875
		[TweakValue("Graphics", 4f, 8f)]
		private static float BladeWidth = 6.6f;

		// Token: 0x04004DA4 RID: 19876
		private const float PowerReductionPercentPerObstacle = 0.2f;

		// Token: 0x04004DA5 RID: 19877
		private const string TranslateWindPathIsBlockedBy = "WindTurbine_WindPathIsBlockedBy";

		// Token: 0x04004DA6 RID: 19878
		private const string TranslateWindPathIsBlockedByRoof = "WindTurbine_WindPathIsBlockedByRoof";

		// Token: 0x04004DA7 RID: 19879
		private static Vector2 BarSize;

		// Token: 0x04004DA8 RID: 19880
		private static readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04004DA9 RID: 19881
		private static readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		// Token: 0x04004DAA RID: 19882
		private static readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Power/WindTurbine/WindTurbineBlades");
	}
}
