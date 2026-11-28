using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B5 RID: 693
	[StaticConstructorOnStartup]
	public class SectionLayer_Gas : SectionLayer
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x0007CB7F File Offset: 0x0007AD7F
		protected virtual FloatRange VertexScaleOffsetRange
		{
			get
			{
				return new FloatRange(0.4f, 0.6f);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x0007CB90 File Offset: 0x0007AD90
		protected virtual FloatRange VertexPositionOffsetRange
		{
			get
			{
				return new FloatRange(-0.2f, 0.2f);
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x0007CBA1 File Offset: 0x0007ADA1
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawGas;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x0007CBA8 File Offset: 0x0007ADA8
		public virtual Material Mat
		{
			get
			{
				return SectionLayer_Gas.GasMat;
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0007CBB0 File Offset: 0x0007ADB0
		public SectionLayer_Gas(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Gas;
			this.propertyBlock = new MaterialPropertyBlock();
			if (!SectionLayer_Gas.gasMatSet)
			{
				SectionLayer_Gas.gasMatSet = true;
				SectionLayer_Gas.GasMat.SetTexture(ShaderPropertyIDs.ToxGasTex, ContentFinder<Texture2D>.Get("Things/Gas/GasCloudThickA", true));
				SectionLayer_Gas.GasMat.SetTexture(ShaderPropertyIDs.RotGasTex, ContentFinder<Texture2D>.Get("Things/Gas/GasCloudThickA", true));
				SectionLayer_Gas.GasMat.SetTexture(ShaderPropertyIDs.DeadlifeDustTex, ContentFinder<Texture2D>.Get("Things/Gas/DeadlifeDust", true));
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0007CC3C File Offset: 0x0007AE3C
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			this.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, RealTime.UnpausedRealTime);
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0, null, 0, this.propertyBlock);
				}
			}
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0007CCC0 File Offset: 0x0007AEC0
		public virtual Color ColorAt(IntVec3 cell)
		{
			return this.DensityAt(cell);
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0007CCCE File Offset: 0x0007AECE
		public virtual Vector4 DensityAt(IntVec3 cell)
		{
			return base.Map.gasGrid.DensitiesAt(cell);
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0007CCE4 File Offset: 0x0007AEE4
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			LayerSubMesh subMesh = base.GetSubMesh(this.Mat);
			float altitude = AltitudeLayer.Gas.AltitudeFor();
			int num = this.section.botLeft.x;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				if (base.Map.gasGrid.AnyGasAt(intVec))
				{
					int count = subMesh.verts.Count;
					this.AddCell(intVec, num, count, subMesh, altitude);
				}
				num++;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.All);
			}
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0007CDB0 File Offset: 0x0007AFB0
		protected void AddCell(IntVec3 c, int index, int startVertIndex, LayerSubMesh sm, float altitude)
		{
			Rand.PushState(index);
			Color c2 = this.ColorAt(c);
			float randomInRange = this.VertexScaleOffsetRange.RandomInRange;
			float randomInRange2 = this.VertexPositionOffsetRange.RandomInRange;
			float randomInRange3 = this.VertexPositionOffsetRange.RandomInRange;
			float x = (float)c.x - randomInRange + randomInRange2;
			float x2 = (float)(c.x + 1) + randomInRange + randomInRange2;
			float z = (float)c.z - randomInRange + randomInRange3;
			float z2 = (float)(c.z + 1) + randomInRange + randomInRange3;
			float y = altitude + Rand.Range(-0.01f, 0.01f);
			sm.verts.Add(new Vector3(x, y, z));
			sm.verts.Add(new Vector3(x, y, z2));
			sm.verts.Add(new Vector3(x2, y, z2));
			sm.verts.Add(new Vector3(x2, y, z));
			sm.uvs.Add(new Vector3(0f, 0f, (float)index));
			sm.uvs.Add(new Vector3(0f, 1f, (float)index));
			sm.uvs.Add(new Vector3(1f, 1f, (float)index));
			sm.uvs.Add(new Vector3(1f, 0f, (float)index));
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.tris.Add(startVertIndex);
			sm.tris.Add(startVertIndex + 1);
			sm.tris.Add(startVertIndex + 2);
			sm.tris.Add(startVertIndex);
			sm.tris.Add(startVertIndex + 2);
			sm.tris.Add(startVertIndex + 3);
			Rand.PopState();
		}

		// Token: 0x0400128E RID: 4750
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x0400128F RID: 4751
		private static Material GasMat = MaterialPool.MatFrom("Things/Gas/GasCloudThickA", ShaderDatabase.GasRotating, 3000);

		// Token: 0x04001290 RID: 4752
		private static bool gasMatSet = false;
	}
}
