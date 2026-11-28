using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001924 RID: 6436
	[StaticConstructorOnStartup]
	public class CompOrbitalBeam : ThingComp
	{
		// Token: 0x17001BDF RID: 7135
		// (get) Token: 0x0600A174 RID: 41332 RVA: 0x0039B77D File Offset: 0x0039997D
		public CompProperties_OrbitalBeam Props
		{
			get
			{
				return (CompProperties_OrbitalBeam)this.props;
			}
		}

		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x0600A175 RID: 41333 RVA: 0x0039B78A File Offset: 0x0039998A
		private int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x0600A176 RID: 41334 RVA: 0x0039B79D File Offset: 0x0039999D
		private int TicksLeft
		{
			get
			{
				return this.totalDuration - this.TicksPassed;
			}
		}

		// Token: 0x17001BE2 RID: 7138
		// (get) Token: 0x0600A177 RID: 41335 RVA: 0x0039B7AC File Offset: 0x003999AC
		private float BeamEndHeight
		{
			get
			{
				return this.Props.width * 0.5f;
			}
		}

		// Token: 0x0600A178 RID: 41336 RVA: 0x0039B7C0 File Offset: 0x003999C0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.totalDuration, "totalDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
		}

		// Token: 0x0600A179 RID: 41337 RVA: 0x0039B81F File Offset: 0x00399A1F
		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			this.startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			this.CheckSpawnSustainer();
		}

		// Token: 0x0600A17A RID: 41338 RVA: 0x0039B84C File Offset: 0x00399A4C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSpawnSustainer();
		}

		// Token: 0x0600A17B RID: 41339 RVA: 0x0039B85B File Offset: 0x00399A5B
		public override void CompTick()
		{
			base.CompTick();
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
				if (this.TicksLeft < this.fadeOutDuration)
				{
					this.sustainer.End();
					this.sustainer = null;
				}
			}
		}

		// Token: 0x0600A17C RID: 41340 RVA: 0x0039B898 File Offset: 0x00399A98
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.TicksLeft <= 0)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			float num = ((float)this.parent.Map.Size.z - drawPos.z) * 1.4142135f;
			Vector3 a = Vector3Utility.FromAngleFlat(this.angle - 90f);
			Vector3 a2 = drawPos + a * num * 0.5f;
			a2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			float num2 = Mathf.Min((float)this.TicksPassed / 10f, 1f);
			Vector3 b = a * ((1f - num2) * num);
			float num3 = 0.975f + Mathf.Sin((float)this.TicksPassed * 0.3f) * 0.025f;
			if (this.TicksLeft < this.fadeOutDuration)
			{
				num3 *= (float)this.TicksLeft / (float)this.fadeOutDuration;
			}
			Color color = this.Props.color;
			color.a *= num3;
			CompOrbitalBeam.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(a2 + a * this.BeamEndHeight * 0.5f + b, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompOrbitalBeam.BeamMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
			Vector3 pos = drawPos + b;
			pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Matrix4x4 matrix2 = default(Matrix4x4);
			matrix2.SetTRS(pos, Quaternion.Euler(0f, this.angle, 0f), new Vector3(this.Props.width, 1f, this.BeamEndHeight));
			Graphics.DrawMesh(MeshPool.plane10, matrix2, CompOrbitalBeam.BeamEndMat, 0, null, 0, CompOrbitalBeam.MatPropertyBlock);
		}

		// Token: 0x0600A17D RID: 41341 RVA: 0x0039BA9D File Offset: 0x00399C9D
		private void CheckSpawnSustainer()
		{
			if (this.TicksLeft >= this.fadeOutDuration && this.Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
				});
			}
		}

		// Token: 0x04005F60 RID: 24416
		private int startTick;

		// Token: 0x04005F61 RID: 24417
		private int totalDuration;

		// Token: 0x04005F62 RID: 24418
		private int fadeOutDuration;

		// Token: 0x04005F63 RID: 24419
		private float angle;

		// Token: 0x04005F64 RID: 24420
		private Sustainer sustainer;

		// Token: 0x04005F65 RID: 24421
		private const float AlphaAnimationSpeed = 0.3f;

		// Token: 0x04005F66 RID: 24422
		private const float AlphaAnimationStrength = 0.025f;

		// Token: 0x04005F67 RID: 24423
		private const float BeamEndHeightRatio = 0.5f;

		// Token: 0x04005F68 RID: 24424
		private static readonly Material BeamMat = MaterialPool.MatFrom("Other/OrbitalBeam", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04005F69 RID: 24425
		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Other/OrbitalBeamEnd", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		// Token: 0x04005F6A RID: 24426
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
	}
}
