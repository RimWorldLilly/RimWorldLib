using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000508 RID: 1288
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		// Token: 0x06002620 RID: 9760 RVA: 0x000E84C0 File Offset: 0x000E66C0
		static MeshPool()
		{
			MeshMakerCircles.MakePieMeshes(MeshPool.pies);
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000E85BC File Offset: 0x000E67BC
		public static Mesh GridPlane(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planes.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, false, false, false);
				MeshPool.planes.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000E85F0 File Offset: 0x000E67F0
		public static Mesh GridPlaneFlip(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planesFlip.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, true, false, false);
				MeshPool.planesFlip.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000E8624 File Offset: 0x000E6824
		public static Mesh GridPlaneFlip(Mesh mesh)
		{
			MeshPool.MeshMetaData metaData = MeshPool.GetMetaData(mesh);
			if (!metaData.flipped)
			{
				return MeshPool.GridPlaneFlip(metaData.size);
			}
			return MeshPool.GridPlane(metaData.size);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000E8657 File Offset: 0x000E6857
		public static Mesh GridPlane(Vector2 size, bool flip)
		{
			if (!flip)
			{
				return MeshPool.GridPlane(size);
			}
			return MeshPool.GridPlaneFlip(size);
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000E866C File Offset: 0x000E686C
		[DebugOutput("System", false)]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count.ToString());
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count.ToString());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000E86DA File Offset: 0x000E68DA
		public static GraphicMeshSet GetMeshSetForWidth(float width)
		{
			return MeshPool.GetMeshSetForSize(width, width);
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000E86E3 File Offset: 0x000E68E3
		public static GraphicMeshSet GetMeshSetForSize(Vector2 size)
		{
			if (!MeshPool.humanlikeMeshSet_Custom.ContainsKey(size))
			{
				MeshPool.humanlikeMeshSet_Custom[size] = new GraphicMeshSet(size.x, size.y);
			}
			return MeshPool.humanlikeMeshSet_Custom[size];
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000E8719 File Offset: 0x000E6919
		public static GraphicMeshSet GetMeshSetForSize(float width, float height)
		{
			return MeshPool.GetMeshSetForSize(new Vector2(width, height));
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000E8727 File Offset: 0x000E6927
		public static MeshPool.MeshMetaData GetMetaData(Mesh mesh)
		{
			return MeshPool.meshMetaData[mesh];
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000E8734 File Offset: 0x000E6934
		public static void EnsureMetaDataCached(Mesh mesh, Vector2 size, bool flipped)
		{
			if (MeshPool.meshMetaData.ContainsKey(mesh))
			{
				return;
			}
			MeshPool.meshMetaData[mesh] = new MeshPool.MeshMetaData(size, flipped);
		}

		// Token: 0x04001B6F RID: 7023
		public const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x04001B70 RID: 7024
		public const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x04001B71 RID: 7025
		private static readonly Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x04001B72 RID: 7026
		private static readonly Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x04001B73 RID: 7027
		private static readonly Dictionary<Mesh, MeshPool.MeshMetaData> meshMetaData = new Dictionary<Mesh, MeshPool.MeshMetaData>();

		// Token: 0x04001B74 RID: 7028
		private static readonly Dictionary<Vector2, GraphicMeshSet> humanlikeMeshSet_Custom = new Dictionary<Vector2, GraphicMeshSet>();

		// Token: 0x04001B75 RID: 7029
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x04001B76 RID: 7030
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x04001B77 RID: 7031
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x04001B78 RID: 7032
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x04001B79 RID: 7033
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x04001B7A RID: 7034
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x04001B7B RID: 7035
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x04001B7C RID: 7036
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x04001B7D RID: 7037
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x04001B7E RID: 7038
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x04001B7F RID: 7039
		public static readonly Mesh[] pies = new Mesh[361];

		// Token: 0x04001B80 RID: 7040
		public static readonly Mesh wholeMapPlane;

		// Token: 0x02002752 RID: 10066
		public struct MeshMetaData
		{
			// Token: 0x0600F7BF RID: 63423 RVA: 0x0059EECC File Offset: 0x0059D0CC
			public MeshMetaData(Vector2 size, bool flipped)
			{
				this.size = size;
				this.flipped = flipped;
			}

			// Token: 0x0400A0EC RID: 41196
			public Vector2 size;

			// Token: 0x0400A0ED RID: 41197
			public bool flipped;
		}
	}
}
