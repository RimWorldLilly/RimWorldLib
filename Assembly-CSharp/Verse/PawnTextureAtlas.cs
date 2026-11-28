using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F4 RID: 1268
	public class PawnTextureAtlas
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x000E4D99 File Offset: 0x000E2F99
		public RenderTexture RawTexture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000E4DA1 File Offset: 0x000E2FA1
		public int FreeCount
		{
			get
			{
				return this.freeFrameSets.Count;
			}
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x000E4DB0 File Offset: 0x000E2FB0
		public PawnTextureAtlas()
		{
			this.texture = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32, 0);
			this.texture.name = "PawnTextureAtlas_" + 2048.ToString();
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < 2048; i += 128)
			{
				for (int j = 0; j < 2048; j += 128)
				{
					list.Add(new Rect((float)i / 2048f, (float)j / 2048f, 0.0625f, 0.0625f));
				}
			}
			while (list.Count >= 8)
			{
				PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = new PawnTextureAtlasFrameSet();
				pawnTextureAtlasFrameSet.uvRects = new Rect[]
				{
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>()
				};
				pawnTextureAtlasFrameSet.meshes = (from u in pawnTextureAtlasFrameSet.uvRects
				select TextureAtlasHelper.CreateMeshForUV(u, 1f)).ToArray<Mesh>();
				pawnTextureAtlasFrameSet.atlas = this.texture;
				this.freeFrameSets.Add(pawnTextureAtlasFrameSet);
			}
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x000E4F45 File Offset: 0x000E3145
		public bool ContainsFrameSet(Pawn pawn)
		{
			return this.frameAssignments.ContainsKey(pawn);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000E4F54 File Offset: 0x000E3154
		public bool TryGetFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew)
		{
			createdNew = false;
			if (this.frameAssignments.TryGetValue(pawn, out frameSet))
			{
				return true;
			}
			if (this.FreeCount == 0)
			{
				return false;
			}
			createdNew = true;
			frameSet = this.freeFrameSets.Pop<PawnTextureAtlasFrameSet>();
			for (int i = 0; i < frameSet.isDirty.Length; i++)
			{
				frameSet.isDirty[i] = true;
			}
			this.frameAssignments.Add(pawn, frameSet);
			return true;
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000E4FBC File Offset: 0x000E31BC
		public void GC()
		{
			try
			{
				foreach (Pawn pawn in this.frameAssignments.Keys)
				{
					if (!pawn.SpawnedOrAnyParentSpawned)
					{
						PawnTextureAtlas.tmpPawnsToFree.Add(pawn);
					}
				}
				foreach (Pawn key in PawnTextureAtlas.tmpPawnsToFree)
				{
					this.freeFrameSets.Add(this.frameAssignments[key]);
					this.frameAssignments.Remove(key);
				}
			}
			finally
			{
				PawnTextureAtlas.tmpPawnsToFree.Clear();
			}
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000E5098 File Offset: 0x000E3298
		public void Destroy()
		{
			foreach (PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet in this.frameAssignments.Values.Concat(this.freeFrameSets))
			{
				Mesh[] meshes = pawnTextureAtlasFrameSet.meshes;
				for (int i = 0; i < meshes.Length; i++)
				{
					Object.Destroy(meshes[i]);
				}
			}
			this.frameAssignments.Clear();
			this.freeFrameSets.Clear();
			Object.Destroy(this.texture);
		}

		// Token: 0x04001B22 RID: 6946
		private RenderTexture texture;

		// Token: 0x04001B23 RID: 6947
		private Dictionary<Pawn, PawnTextureAtlasFrameSet> frameAssignments = new Dictionary<Pawn, PawnTextureAtlasFrameSet>();

		// Token: 0x04001B24 RID: 6948
		private List<PawnTextureAtlasFrameSet> freeFrameSets = new List<PawnTextureAtlasFrameSet>();

		// Token: 0x04001B25 RID: 6949
		private static List<Pawn> tmpPawnsToFree = new List<Pawn>();

		// Token: 0x04001B26 RID: 6950
		private const int AtlasSize = 2048;

		// Token: 0x04001B27 RID: 6951
		public const int FrameSize = 128;

		// Token: 0x04001B28 RID: 6952
		private const int PawnsHeldPerAtlas = 32;

		// Token: 0x04001B29 RID: 6953
		private const int FramesPerPawn = 8;
	}
}
