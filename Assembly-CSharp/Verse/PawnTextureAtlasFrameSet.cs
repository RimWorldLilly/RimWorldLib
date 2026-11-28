using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F3 RID: 1267
	public class PawnTextureAtlasFrameSet
	{
		// Token: 0x0600259B RID: 9627 RVA: 0x000E4D4C File Offset: 0x000E2F4C
		public int GetIndex(Rot4 rotation, PawnDrawMode drawMode)
		{
			if (drawMode == PawnDrawMode.BodyAndHead)
			{
				return rotation.AsInt;
			}
			return 4 + rotation.AsInt;
		}

		// Token: 0x04001B1E RID: 6942
		public RenderTexture atlas;

		// Token: 0x04001B1F RID: 6943
		public Rect[] uvRects = new Rect[8];

		// Token: 0x04001B20 RID: 6944
		public Mesh[] meshes = new Mesh[8];

		// Token: 0x04001B21 RID: 6945
		public bool[] isDirty = new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};
	}
}
