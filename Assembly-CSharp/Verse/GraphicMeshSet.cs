using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000506 RID: 1286
	public class GraphicMeshSet
	{
		// Token: 0x06002616 RID: 9750 RVA: 0x000E82FC File Offset: 0x000E64FC
		public GraphicMeshSet(Mesh normalMesh, Mesh leftMesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			this.meshes[2] = normalMesh;
			array[num] = (array2[num2] = normalMesh);
			this.meshes[3] = leftMesh;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000E8344 File Offset: 0x000E6544
		public GraphicMeshSet(Mesh mesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			Mesh[] array3 = this.meshes;
			int num3 = 2;
			this.meshes[3] = mesh;
			array[num] = (array2[num2] = (array3[num3] = mesh));
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000E8390 File Offset: 0x000E6590
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000E83E8 File Offset: 0x000E65E8
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000E8448 File Offset: 0x000E6648
		public GraphicMeshSet(Mesh northMesh, Mesh eastMesh, Mesh southMesh, Mesh westMesh)
		{
			this.meshes[0] = northMesh;
			this.meshes[1] = eastMesh;
			this.meshes[2] = southMesh;
			this.meshes[3] = westMesh;
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000E8481 File Offset: 0x000E6681
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x04001B6D RID: 7021
		private Mesh[] meshes = new Mesh[4];
	}
}
