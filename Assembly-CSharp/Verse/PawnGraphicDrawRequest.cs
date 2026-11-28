using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038D RID: 909
	public struct PawnGraphicDrawRequest
	{
		// Token: 0x06001BA3 RID: 7075 RVA: 0x000AA838 File Offset: 0x000A8A38
		public PawnGraphicDrawRequest(PawnRenderNode node, Mesh mesh = null, Material material = null)
		{
			this.node = node;
			this.mesh = mesh;
			this.material = material;
			this.preDrawnComputedMatrix = default(Matrix4x4);
		}

		// Token: 0x040016BB RID: 5819
		public readonly PawnRenderNode node;

		// Token: 0x040016BC RID: 5820
		public readonly Mesh mesh;

		// Token: 0x040016BD RID: 5821
		public Material material;

		// Token: 0x040016BE RID: 5822
		public Matrix4x4 preDrawnComputedMatrix;
	}
}
