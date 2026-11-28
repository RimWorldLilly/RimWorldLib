using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000584 RID: 1412
	public abstract class Graphic_WithPropertyBlockRandom : Graphic_Random
	{
		// Token: 0x060029BB RID: 10683 RVA: 0x000FF60A File Offset: 0x000FD80A
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x04001CEB RID: 7403
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
