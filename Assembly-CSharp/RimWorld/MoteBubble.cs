using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2F RID: 6959
	[StaticConstructorOnStartup]
	public class MoteBubble : MoteDualAttached
	{
		// Token: 0x0600B27C RID: 45692 RVA: 0x003ED668 File Offset: 0x003EB868
		public void SetupMoteBubble(Texture2D icon, Pawn target, Color? iconColor = null)
		{
			this.iconMat = MaterialPool.MatFrom(icon, ShaderDatabase.TransparentPostLight, Color.white);
			this.iconMatPropertyBlock = new MaterialPropertyBlock();
			this.arrowTarget = target;
			if (iconColor != null)
			{
				this.iconMatPropertyBlock.SetColor(ShaderPropertyIDs.Color, iconColor.Value);
			}
		}

		// Token: 0x0600B27D RID: 45693 RVA: 0x003ED6C0 File Offset: 0x003EB8C0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (Find.UIRoot.HideMotes)
			{
				return;
			}
			if (this.iconMat != null)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.01f;
				float alpha = this.Alpha;
				if (alpha <= 0f)
				{
					return;
				}
				Color instanceColor = this.instanceColor;
				instanceColor.a *= alpha;
				Material material = this.iconMat;
				if (instanceColor != material.color)
				{
					material = MaterialPool.MatFrom((Texture2D)material.mainTexture, material.shader, instanceColor);
				}
				Vector3 s = new Vector3(this.def.graphicData.drawSize.x * 0.64f, 1f, this.def.graphicData.drawSize.y * 0.64f);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(drawPos, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0, null, 0, this.iconMatPropertyBlock);
			}
			if (this.arrowTarget != null)
			{
				Quaternion rotation = Quaternion.AngleAxis(((this.arrowTarget.Spawned ? this.arrowTarget.TrueCenter() : this.arrowTarget.PositionHeld.ToVector3Shifted()) - this.DrawPos).AngleFlat(), Vector3.up);
				Vector3 vector = this.DrawPos;
				vector.y -= 0.01f;
				vector += 0.6f * (rotation * Vector3.forward);
				Graphics.DrawMesh(MeshPool.plane05, vector, rotation, MoteBubble.InteractionArrowTex, 0);
			}
		}

		// Token: 0x04006702 RID: 26370
		public Material iconMat;

		// Token: 0x04006703 RID: 26371
		public Pawn arrowTarget;

		// Token: 0x04006704 RID: 26372
		public MaterialPropertyBlock iconMatPropertyBlock;

		// Token: 0x04006705 RID: 26373
		private static readonly Material InteractionArrowTex = MaterialPool.MatFrom("Things/Mote/InteractionArrow");
	}
}
