using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000583 RID: 1411
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		// Token: 0x060029B9 RID: 10681 RVA: 0x000FF5B9 File Offset: 0x000FD7B9
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x04001CEA RID: 7402
		public MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
