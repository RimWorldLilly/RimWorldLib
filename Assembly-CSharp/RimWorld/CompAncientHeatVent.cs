using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001804 RID: 6148
	public class CompAncientHeatVent : CompAncientVent
	{
		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x0600998B RID: 39307 RVA: 0x0037894D File Offset: 0x00376B4D
		protected override bool AppliesEffectsToPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x0600998C RID: 39308 RVA: 0x00378950 File Offset: 0x00376B50
		public new CompProperties_AncientHeatVent Props
		{
			get
			{
				return (CompProperties_AncientHeatVent)this.props;
			}
		}

		// Token: 0x0600998D RID: 39309 RVA: 0x0037895D File Offset: 0x00376B5D
		protected override void ToggleIndividualVent(bool on)
		{
		}

		// Token: 0x0600998E RID: 39310 RVA: 0x00378960 File Offset: 0x00376B60
		private void DrawFleckQuad(FleckDef def, MaterialPropertyBlock props)
		{
			GraphicData graphicData = def.graphicData;
			Vector3 pos = this.parent.Position.ToVector3Shifted() + graphicData.drawOffset;
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.identity, (this.parent.DrawSize * graphicData.drawSize).ToVector3());
			Graphics.DrawMesh(MeshPool.plane10, matrix, graphicData.Graphic.MatSingle, 0, null, 0, props);
		}

		// Token: 0x0600998F RID: 39311 RVA: 0x003789E4 File Offset: 0x00376BE4
		private void DrawGlow()
		{
			GraphicData graphicData = FleckDefOf.AncientVentHeatGlow.graphicData;
			this.glowProps = new MaterialPropertyBlock();
			this.glowProps.SetColor(ShaderPropertyIDs.Color, graphicData.color.WithAlpha(this.intensity * this.targetGlow * graphicData.color.a));
			this.DrawFleckQuad(FleckDefOf.AncientVentHeatGlow, this.glowProps);
		}

		// Token: 0x06009990 RID: 39312 RVA: 0x00378A4C File Offset: 0x00376C4C
		private void DrawHeatShimmer()
		{
			if (this.heatShimmerProps == null)
			{
				this.heatShimmerProps = new MaterialPropertyBlock();
			}
			this.heatShimmerProps.SetFloat(CompAncientHeatVent.shaderPropertyIDIntensity, this.intensity);
			this.DrawFleckQuad(FleckDefOf.AncientVentHeatShimmer, this.glowProps);
		}

		// Token: 0x06009991 RID: 39313 RVA: 0x00378A88 File Offset: 0x00376C88
		public override void PostDraw()
		{
			if (!base.VentOn)
			{
				return;
			}
			this.DrawGlow();
			this.DrawHeatShimmer();
		}

		// Token: 0x06009992 RID: 39314 RVA: 0x00378A9F File Offset: 0x00376C9F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.intensity = (base.VentOn ? 1f : 0f);
			CompAncientHeatVent.shaderPropertyIDIntensity = Shader.PropertyToID("_Intensity");
		}

		// Token: 0x06009993 RID: 39315 RVA: 0x00378AD4 File Offset: 0x00376CD4
		public override void CompTickInterval(int delta)
		{
			if (base.VentOn && this.intensity < 1f)
			{
				this.intensity += (float)delta * 0.016666668f / this.Props.rampUpTime;
				if (this.intensity > 1f)
				{
					this.intensity = 1f;
					return;
				}
			}
			else if (this.intensity > 0f)
			{
				this.targetGlow = Mathf.Lerp(this.Props.minGlowBrightness, this.Props.maxGlowBrightness, 0.5f + 0.5f * Mathf.Sin((float)GenTicks.TicksGame * 0.016666668f * this.Props.pulseSpeed));
				if (!base.VentOn)
				{
					this.intensity -= (float)delta * 0.016666668f / this.Props.rampDownTime;
					if (this.intensity < 0f)
					{
						this.intensity = 0f;
					}
				}
			}
		}

		// Token: 0x04005B96 RID: 23446
		private float targetGlow = 1f;

		// Token: 0x04005B97 RID: 23447
		private float intensity;

		// Token: 0x04005B98 RID: 23448
		private MaterialPropertyBlock glowProps;

		// Token: 0x04005B99 RID: 23449
		private MaterialPropertyBlock heatShimmerProps;

		// Token: 0x04005B9A RID: 23450
		private static int shaderPropertyIDIntensity;
	}
}
