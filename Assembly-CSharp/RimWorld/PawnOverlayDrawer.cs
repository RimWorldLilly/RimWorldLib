using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200148F RID: 5263
	[StaticConstructorOnStartup]
	public abstract class PawnOverlayDrawer
	{
		// Token: 0x0600810D RID: 33037 RVA: 0x002E059D File Offset: 0x002DE79D
		public PawnOverlayDrawer(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600810E RID: 33038 RVA: 0x002E05B7 File Offset: 0x002DE7B7
		private static List<PawnOverlayDrawer.DrawCall> GetDrawCallList()
		{
			if (PawnOverlayDrawer.drawCallListPool.Count == 0)
			{
				return new List<PawnOverlayDrawer.DrawCall>();
			}
			List<PawnOverlayDrawer.DrawCall> result = PawnOverlayDrawer.drawCallListPool[PawnOverlayDrawer.drawCallListPool.Count - 1];
			PawnOverlayDrawer.drawCallListPool.RemoveAt(PawnOverlayDrawer.drawCallListPool.Count - 1);
			return result;
		}

		// Token: 0x0600810F RID: 33039 RVA: 0x002E05F7 File Offset: 0x002DE7F7
		private static void ReturnDrawCallList(List<PawnOverlayDrawer.DrawCall> lst)
		{
			lst.Clear();
			PawnOverlayDrawer.drawCallListPool.Add(lst);
		}

		// Token: 0x06008110 RID: 33040 RVA: 0x002E060C File Offset: 0x002DE80C
		public void ClearCache()
		{
			foreach (List<PawnOverlayDrawer.DrawCall> lst in this.drawCallCache.Values)
			{
				PawnOverlayDrawer.ReturnDrawCallList(lst);
			}
			this.drawCallCache.Clear();
		}

		// Token: 0x06008111 RID: 33041
		protected abstract void WriteCache(PawnOverlayDrawer.CacheKey key, PawnDrawParms parms, List<PawnOverlayDrawer.DrawCall> writeTarget);

		// Token: 0x06008112 RID: 33042 RVA: 0x002E066C File Offset: 0x002DE86C
		public void RenderPawnOverlay(Matrix4x4 matrix, Mesh bodyMesh, PawnOverlayDrawer.OverlayLayer layer, PawnDrawParms parms, bool? overApparel = null)
		{
			PawnOverlayDrawer.CacheKey key = new PawnOverlayDrawer.CacheKey(bodyMesh, parms.facing, layer);
			List<PawnOverlayDrawer.DrawCall> drawCallList;
			if (!this.drawCallCache.TryGetValue(key, out drawCallList))
			{
				drawCallList = PawnOverlayDrawer.GetDrawCallList();
				this.WriteCache(key, parms, drawCallList);
				this.drawCallCache.Add(key, drawCallList);
			}
			foreach (PawnOverlayDrawer.DrawCall drawCall in drawCallList)
			{
				if (overApparel != null)
				{
					bool? flag = overApparel;
					bool displayOverApparel = drawCall.displayOverApparel;
					if (!(flag.GetValueOrDefault() == displayOverApparel & flag != null))
					{
						continue;
					}
				}
				this.DoDrawCall(drawCall, matrix, parms.DrawNow);
			}
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x002E072C File Offset: 0x002DE92C
		private void DoDrawCall(PawnOverlayDrawer.DrawCall call, Matrix4x4 matrix, bool drawNow)
		{
			if (drawNow)
			{
				if (call.maskTexOffset != null)
				{
					call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureOffset, call.maskTexOffset.Value);
					call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureScale, call.maskTexScale.Value);
				}
				if (call.mainTexOffset != null)
				{
					call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureOffset, call.mainTexOffset.Value);
				}
				if (call.mainTexScale != null)
				{
					call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureScale, call.mainTexScale.Value);
				}
				if (call.colorOverride != null)
				{
					call.overlayMat.SetColor(ShaderPropertyIDs.Color, call.colorOverride.Value);
				}
				call.overlayMat.SetInt(ShaderPropertyIDs.Rotation, call.rotation);
				call.overlayMat.SetPass(0);
				Graphics.DrawMeshNow(call.overlayMesh, matrix * call.matrix);
				call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureOffset, Vector4.zero);
				call.overlayMat.SetVector(ShaderPropertyIDs.MaskTextureScale, Vector4.one);
				call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureOffset, Vector4.zero);
				call.overlayMat.SetVector(ShaderPropertyIDs.MainTextureScale, Vector4.one);
				if (call.colorOverride != null)
				{
					call.overlayMat.SetColor(ShaderPropertyIDs.Color, Color.white);
					return;
				}
			}
			else
			{
				PawnOverlayDrawer.propBlock.Clear();
				if (call.maskTexOffset != null)
				{
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MaskTextureOffset, call.maskTexOffset.Value);
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MaskTextureScale, call.maskTexScale.Value);
				}
				if (call.mainTexOffset != null)
				{
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MainTextureOffset, call.mainTexOffset.Value);
				}
				if (call.mainTexScale != null)
				{
					PawnOverlayDrawer.propBlock.SetVector(ShaderPropertyIDs.MainTextureScale, call.mainTexScale.Value);
				}
				if (call.colorOverride != null)
				{
					PawnOverlayDrawer.propBlock.SetColor(ShaderPropertyIDs.Color, call.colorOverride.Value);
				}
				PawnOverlayDrawer.propBlock.SetInt(ShaderPropertyIDs.Rotation, call.rotation);
				Graphics.DrawMesh(call.overlayMesh, matrix * call.matrix, call.overlayMat, 0, null, 0, PawnOverlayDrawer.propBlock);
			}
		}

		// Token: 0x04005042 RID: 20546
		protected Pawn pawn;

		// Token: 0x04005043 RID: 20547
		private static MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

		// Token: 0x04005044 RID: 20548
		protected Dictionary<PawnOverlayDrawer.CacheKey, List<PawnOverlayDrawer.DrawCall>> drawCallCache = new Dictionary<PawnOverlayDrawer.CacheKey, List<PawnOverlayDrawer.DrawCall>>();

		// Token: 0x04005045 RID: 20549
		protected static List<List<PawnOverlayDrawer.DrawCall>> drawCallListPool = new List<List<PawnOverlayDrawer.DrawCall>>();

		// Token: 0x020032D3 RID: 13011
		public enum OverlayLayer
		{
			// Token: 0x0400CC20 RID: 52256
			Body,
			// Token: 0x0400CC21 RID: 52257
			Head
		}

		// Token: 0x020032D4 RID: 13012
		protected struct DrawCall
		{
			// Token: 0x0400CC22 RID: 52258
			public Mesh overlayMesh;

			// Token: 0x0400CC23 RID: 52259
			public Matrix4x4 matrix;

			// Token: 0x0400CC24 RID: 52260
			public Material overlayMat;

			// Token: 0x0400CC25 RID: 52261
			public bool displayOverApparel;

			// Token: 0x0400CC26 RID: 52262
			public Color? colorOverride;

			// Token: 0x0400CC27 RID: 52263
			public Vector4? maskTexScale;

			// Token: 0x0400CC28 RID: 52264
			public Vector4? maskTexOffset;

			// Token: 0x0400CC29 RID: 52265
			public Vector4? mainTexScale;

			// Token: 0x0400CC2A RID: 52266
			public Vector4? mainTexOffset;

			// Token: 0x0400CC2B RID: 52267
			public int rotation;
		}

		// Token: 0x020032D5 RID: 13013
		protected struct CacheKey : IEquatable<PawnOverlayDrawer.CacheKey>
		{
			// Token: 0x06012D84 RID: 77188 RVA: 0x00644ED4 File Offset: 0x006430D4
			public CacheKey(Mesh bodyMesh, Rot4 pawnRot, PawnOverlayDrawer.OverlayLayer layer)
			{
				this.bodyMesh = bodyMesh;
				this.pawnRot = pawnRot;
				this.layer = layer;
				this.hash = ((bodyMesh == null) ? 0 : bodyMesh.GetHashCode());
				this.hash = Gen.HashCombineInt(pawnRot.GetHashCode(), this.hash);
				this.hash = Gen.HashCombineInt(layer.GetHashCode(), this.hash);
			}

			// Token: 0x06012D85 RID: 77189 RVA: 0x00644F4A File Offset: 0x0064314A
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x06012D86 RID: 77190 RVA: 0x00644F52 File Offset: 0x00643152
			public bool Equals(PawnOverlayDrawer.CacheKey other)
			{
				return other.bodyMesh == this.bodyMesh && other.pawnRot == this.pawnRot && other.layer == this.layer;
			}

			// Token: 0x06012D87 RID: 77191 RVA: 0x00644F8C File Offset: 0x0064318C
			public override bool Equals(object other)
			{
				if (other is PawnOverlayDrawer.CacheKey)
				{
					PawnOverlayDrawer.CacheKey other2 = (PawnOverlayDrawer.CacheKey)other;
					return this.Equals(other2);
				}
				return false;
			}

			// Token: 0x0400CC2C RID: 52268
			public Mesh bodyMesh;

			// Token: 0x0400CC2D RID: 52269
			public Rot4 pawnRot;

			// Token: 0x0400CC2E RID: 52270
			public PawnOverlayDrawer.OverlayLayer layer;

			// Token: 0x0400CC2F RID: 52271
			private int hash;
		}
	}
}
