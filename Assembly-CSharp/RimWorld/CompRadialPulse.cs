using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194C RID: 6476
	[StaticConstructorOnStartup]
	public class CompRadialPulse : ThingComp
	{
		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x0600A270 RID: 41584 RVA: 0x003A0DE7 File Offset: 0x0039EFE7
		private CompProperties_RadialPulse Props
		{
			get
			{
				return (CompProperties_RadialPulse)this.props;
			}
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x0600A271 RID: 41585 RVA: 0x003A0DF4 File Offset: 0x0039EFF4
		private float RingLerpFactor
		{
			get
			{
				return (float)(Find.TickManager.TicksGame % this.Props.ticksBetweenPulses) / (float)this.Props.ticksPerPulse;
			}
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x0600A272 RID: 41586 RVA: 0x003A0E1A File Offset: 0x0039F01A
		private float RingScale
		{
			get
			{
				return this.Props.radius * Mathf.Lerp(0f, 2f, this.RingLerpFactor) * 1.1601562f;
			}
		}

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x0600A273 RID: 41587 RVA: 0x003A0E44 File Offset: 0x0039F044
		private bool ParentIsActive
		{
			get
			{
				CompSendSignalOnMotion comp = this.parent.GetComp<CompSendSignalOnMotion>();
				return comp != null && comp.Sent;
			}
		}

		// Token: 0x0600A274 RID: 41588 RVA: 0x003A0E68 File Offset: 0x0039F068
		public override void PostDraw()
		{
			if (this.ParentIsActive)
			{
				return;
			}
			Vector3 pos = this.parent.Position.ToVector3Shifted();
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Color color = this.Props.color;
			color.a = Mathf.Lerp(this.Props.color.a, 0f, this.RingLerpFactor);
			CompRadialPulse.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, Quaternion.identity, new Vector3(this.RingScale, 1f, this.RingScale));
			Graphics.DrawMesh(MeshPool.plane10, matrix, CompRadialPulse.RingMat, 0, null, 0, CompRadialPulse.MatPropertyBlock);
		}

		// Token: 0x04005FF9 RID: 24569
		private static readonly Material RingMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x04005FFA RID: 24570
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04005FFB RID: 24571
		private const float TextureActualRingSizeFactor = 1.1601562f;
	}
}
