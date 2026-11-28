using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200148E RID: 5262
	public class PawnFirefoamDrawer : PawnOverlayDrawer
	{
		// Token: 0x0600810A RID: 33034 RVA: 0x002E02E5 File Offset: 0x002DE4E5
		public PawnFirefoamDrawer(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600810B RID: 33035 RVA: 0x002E02F0 File Offset: 0x002DE4F0
		protected override void WriteCache(PawnOverlayDrawer.CacheKey key, PawnDrawParms parms, List<PawnOverlayDrawer.DrawCall> writeTarget)
		{
			Rot4 pawnRot = key.pawnRot;
			Mesh bodyMesh = key.bodyMesh;
			PawnOverlayDrawer.OverlayLayer layer = key.layer;
			Graphic graphic = (layer == PawnOverlayDrawer.OverlayLayer.Body) ? this.pawn.Drawer.renderer.BodyGraphic : this.pawn.Drawer.renderer.HeadGraphic;
			Rand.PushState(this.pawn.thingIDNumber * (int)(layer + 1));
			try
			{
				bool flag = (graphic.EastFlipped && pawnRot == Rot4.East) || (graphic.WestFlipped && pawnRot == Rot4.West);
				int num = (Rand.Range(0, PawnFirefoamDrawer.FoamTexturePaths.Length) + pawnRot.AsInt) % PawnFirefoamDrawer.FoamTexturePaths.Length;
				Material material = MaterialPool.MatFrom(PawnFirefoamDrawer.FoamTexturePaths[num], ShaderDatabase.FirefoamOverlay, Color.white);
				Mesh mesh = flag ? MeshPool.GridPlaneFlip(Vector2.one * 0.25f) : MeshPool.GridPlane(Vector2.one * 0.25f);
				Vector3 size = bodyMesh.bounds.size;
				float d = size.magnitude * 2.8f;
				material = MaterialPool.MatFrom(new MaterialRequest
				{
					maskTex = (Texture2D)graphic.MatAt(pawnRot, null).mainTexture,
					mainTex = material.mainTexture,
					color = material.color,
					shader = material.shader
				});
				Vector3 vector = Rand.InsideUnitCircleVec3 * 2f;
				Vector3 vector2 = mesh.bounds.size * d;
				Vector4 value = new Vector4(vector2.x / size.x, vector2.z / size.z);
				Vector4 value2 = new Vector4(vector.x, vector.z);
				Vector4 value3 = new Vector4(1.4f, 1.4f, 1f, 1f);
				writeTarget.Add(new PawnOverlayDrawer.DrawCall
				{
					overlayMat = material,
					matrix = Matrix4x4.Scale(Vector3.one * d),
					overlayMesh = mesh,
					displayOverApparel = true,
					maskTexScale = new Vector4?(value),
					mainTexScale = new Vector4?(value3),
					mainTexOffset = new Vector4?(value2)
				});
			}
			finally
			{
				Rand.PopState();
			}
		}

		// Token: 0x0400503D RID: 20541
		public bool coveredInFoam;

		// Token: 0x0400503E RID: 20542
		public const float TextureScaleFactor = 2.8f;

		// Token: 0x0400503F RID: 20543
		public const float TextureTiles = 1.4f;

		// Token: 0x04005040 RID: 20544
		public const float TextureOffsetVecMagnitude = 2f;

		// Token: 0x04005041 RID: 20545
		private static readonly string[] FoamTexturePaths = new string[]
		{
			"Things/Pawn/Overlays/Firefoam/FireFoamOverlayA",
			"Things/Pawn/Overlays/Firefoam/FireFoamOverlayB",
			"Things/Pawn/Overlays/Firefoam/FireFoamOverlayC",
			"Things/Pawn/Overlays/Firefoam/FireFoamOverlayD"
		};
	}
}
