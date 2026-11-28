using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001490 RID: 5264
	public abstract class PawnScarDrawer : PawnOverlayDrawer
	{
		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06008115 RID: 33045
		protected abstract string ScarTexturePath { get; }

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06008116 RID: 33046 RVA: 0x002E09D1 File Offset: 0x002DEBD1
		protected virtual float ScaleFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06008117 RID: 33047 RVA: 0x002E09D8 File Offset: 0x002DEBD8
		public PawnScarDrawer(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06008118 RID: 33048 RVA: 0x002E09E4 File Offset: 0x002DEBE4
		protected override void WriteCache(PawnOverlayDrawer.CacheKey key, PawnDrawParms parms, List<PawnOverlayDrawer.DrawCall> writeTarget)
		{
			Rot4 pawnRot = key.pawnRot;
			Mesh bodyMesh = key.bodyMesh;
			PawnOverlayDrawer.OverlayLayer layer = key.layer;
			Graphic graphic = (layer == PawnOverlayDrawer.OverlayLayer.Body) ? this.pawn.Drawer.renderer.BodyGraphic : this.pawn.Drawer.renderer.HeadGraphic;
			if (graphic == null)
			{
				return;
			}
			Rand.PushState(this.pawn.thingIDNumber * (int)(layer + 1));
			try
			{
				Mesh mesh = ((graphic.EastFlipped && pawnRot == Rot4.East) || (graphic.WestFlipped && pawnRot == Rot4.West)) ? MeshPool.GridPlaneFlip(Vector2.one) : MeshPool.GridPlane(Vector2.one);
				Vector3 size = bodyMesh.bounds.size;
				float d = size.magnitude * this.ScaleFactor;
				Vector3 vector = mesh.bounds.size * d;
				Vector4 value = new Vector4(vector.x / size.x, vector.z / size.z);
				Material material = MaterialPool.MatFrom(this.ScarTexturePath, ShaderDatabase.Wound, Color.white);
				material = MaterialPool.MatFrom(new MaterialRequest
				{
					maskTex = (Texture2D)graphic.MatAt(pawnRot, null).mainTexture,
					mainTex = material.mainTexture,
					color = material.color,
					shader = material.shader
				});
				Vector3 vector2 = Rand.InsideUnitCircleVec3 / 2f;
				int rotation = Rand.Range(0, 360);
				writeTarget.Add(new PawnOverlayDrawer.DrawCall
				{
					overlayMat = material,
					matrix = Matrix4x4.Scale(size),
					overlayMesh = mesh,
					mainTexScale = new Vector4?(value),
					mainTexOffset = new Vector4?(new Vector4(vector2.x, vector2.z)),
					rotation = rotation
				});
			}
			finally
			{
				Rand.PopState();
			}
		}
	}
}
