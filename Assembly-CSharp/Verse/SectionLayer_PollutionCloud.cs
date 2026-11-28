using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BA RID: 698
	[StaticConstructorOnStartup]
	public class SectionLayer_PollutionCloud : SectionLayer_Gas
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0007E425 File Offset: 0x0007C625
		protected override FloatRange VertexScaleOffsetRange
		{
			get
			{
				return new FloatRange(7f, 11f);
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x0007E436 File Offset: 0x0007C636
		protected override FloatRange VertexPositionOffsetRange
		{
			get
			{
				return new FloatRange(-1f, 1f);
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0007E448 File Offset: 0x0007C648
		public override Material Mat
		{
			get
			{
				if (SectionLayer_PollutionCloud.matCached == null)
				{
					SectionLayer_PollutionCloud.matCached = MaterialPool.MatFrom(SectionLayer_PollutionCloud.PollutionTex.Texture, ShaderDatabase.PollutionCloud, Color.white, 3000);
					SectionLayer_PollutionCloud.matCached.SetTexture(ShaderPropertyIDs.FadeTex, SectionLayer_PollutionCloud.PollutionFadeTex.Texture);
					SectionLayer_PollutionCloud.matCached.SetVector(ShaderPropertyIDs.TexScrollSpeed, SectionLayer_PollutionCloud.FadeTexScrollSpeed);
					SectionLayer_PollutionCloud.matCached.SetVector(ShaderPropertyIDs.TexScale, SectionLayer_PollutionCloud.FadeTexScale);
				}
				return SectionLayer_PollutionCloud.matCached;
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0007E4D8 File Offset: 0x0007C6D8
		public SectionLayer_PollutionCloud(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Pollution;
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0007E525 File Offset: 0x0007C725
		public override bool Visible
		{
			get
			{
				return base.Visible && ModsConfig.BiotechActive;
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0007E538 File Offset: 0x0007C738
		public override Color ColorAt(IntVec3 cell)
		{
			float depth = base.Map.snowGrid.GetDepth(cell);
			cell.GetSandDepth(base.Map);
			TerrainDef terrainDef = base.Map.terrainGrid.TerrainAt(cell);
			if (depth >= 0.4f)
			{
				return SectionLayer_PollutionCloud.SnowPollutionCloudColor;
			}
			return terrainDef.pollutionCloudColor;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0007E588 File Offset: 0x0007C788
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			LayerSubMesh subMesh = base.GetSubMesh(this.Mat);
			float altitude = AltitudeLayer.Gas.AltitudeFor();
			int num = this.section.botLeft.x;
			foreach (IntVec3 c in this.AffectedCells(this.section.CellRect))
			{
				int count = subMesh.verts.Count;
				base.AddCell(c, num, count, subMesh, altitude);
				num++;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.All);
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0007E640 File Offset: 0x0007C840
		private IEnumerable<IntVec3> AffectedCells(CellRect rect)
		{
			SectionLayer_PollutionCloud.<AffectedCells>d__23 <AffectedCells>d__ = new SectionLayer_PollutionCloud.<AffectedCells>d__23(-2);
			<AffectedCells>d__.<>4__this = this;
			<AffectedCells>d__.<>3__rect = rect;
			return <AffectedCells>d__;
		}

		// Token: 0x04001296 RID: 4758
		private static Material matCached;

		// Token: 0x04001297 RID: 4759
		private float MinCellDistance = 5f;

		// Token: 0x04001298 RID: 4760
		private int AttemptsPerCell = 15;

		// Token: 0x04001299 RID: 4761
		private float MinPercentage = 0.35f;

		// Token: 0x0400129A RID: 4762
		private static readonly SimpleCurve NumCellsPerSectionPollutionLevel = new SimpleCurve
		{
			{
				new CurvePoint(10f, 3f),
				true
			},
			{
				new CurvePoint(20f, 6f),
				true
			}
		};

		// Token: 0x0400129B RID: 4763
		private static readonly Vector2 FadeTexScrollSpeed = new Vector2(0.035f, 0.0125f);

		// Token: 0x0400129C RID: 4764
		private static readonly Vector2 FadeTexScale = new Vector2(0.15f, 0.15f);

		// Token: 0x0400129D RID: 4765
		private static readonly CachedTexture PollutionTex = new CachedTexture("Other/PollutionCloud");

		// Token: 0x0400129E RID: 4766
		private static readonly CachedTexture PollutionFadeTex = new CachedTexture("Other/PollutionCloudFade");

		// Token: 0x0400129F RID: 4767
		private static readonly Color SnowPollutionCloudColor = new Color(1f, 1f, 1f, 0.66f);

		// Token: 0x040012A0 RID: 4768
		private const float SnowPollutionColorThreshold = 0.4f;

		// Token: 0x040012A1 RID: 4769
		private List<IntVec3> cellsTmp = new List<IntVec3>();
	}
}
