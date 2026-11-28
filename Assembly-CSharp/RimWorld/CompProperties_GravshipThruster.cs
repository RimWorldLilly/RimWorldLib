using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E9 RID: 6633
	public class CompProperties_GravshipThruster : CompProperties_GravshipFacility
	{
		// Token: 0x17001D40 RID: 7488
		// (get) Token: 0x0600A701 RID: 42753 RVA: 0x003B7FFE File Offset: 0x003B61FE
		public ShaderTypeDef FlameShaderType
		{
			get
			{
				return this.flameShaderType ?? ShaderTypeDefOf.MoteGlow;
			}
		}

		// Token: 0x0600A702 RID: 42754 RVA: 0x003B8010 File Offset: 0x003B6210
		public CompProperties_GravshipThruster()
		{
			this.compClass = typeof(CompGravshipThruster);
		}

		// Token: 0x0600A703 RID: 42755 RVA: 0x003B8080 File Offset: 0x003B6280
		public void GetExclusionZone(IntVec3 pos, Rot4 rot, ref List<IntVec3> exclusionCells)
		{
			if (exclusionCells == null)
			{
				exclusionCells = new List<IntVec3>();
			}
			else
			{
				exclusionCells.Clear();
			}
			for (int i = 0; i < this.exclusionAreaSize.x; i++)
			{
				for (int j = 0; j < this.exclusionAreaSize.z; j++)
				{
					exclusionCells.Add(pos + new IntVec3(i, 0, j).RotatedBy(rot) + this.exclusionAreaOffset.RotatedBy(rot));
				}
			}
		}

		// Token: 0x0600A704 RID: 42756 RVA: 0x003B80F9 File Offset: 0x003B62F9
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_GravshipThruster.<ConfigErrors>d__13 <ConfigErrors>d__ = new CompProperties_GravshipThruster.<ConfigErrors>d__13(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400625E RID: 25182
		public IntVec3 exclusionAreaSize;

		// Token: 0x0400625F RID: 25183
		public IntVec3 exclusionAreaOffset;

		// Token: 0x04006260 RID: 25184
		public int directionInfluence;

		// Token: 0x04006261 RID: 25185
		public List<Vector3> flameOffsetsPerDirection = new List<Vector3>
		{
			Vector3.zero,
			Vector3.zero,
			Vector3.zero,
			Vector3.zero
		};

		// Token: 0x04006262 RID: 25186
		public float flameSize;

		// Token: 0x04006263 RID: 25187
		private ShaderTypeDef flameShaderType;

		// Token: 0x04006264 RID: 25188
		public List<ShaderParameter> flameShaderParameters = new List<ShaderParameter>();

		// Token: 0x04006265 RID: 25189
		public CompProperties_GravshipThruster.ExhaustSettings exhaustSettings = new CompProperties_GravshipThruster.ExhaustSettings();

		// Token: 0x0200372D RID: 14125
		public class ExhaustSettings
		{
			// Token: 0x17003463 RID: 13411
			// (get) Token: 0x0601429A RID: 82586 RVA: 0x00680D1B File Offset: 0x0067EF1B
			public FleckDef ExhaustFleckDef
			{
				get
				{
					return this.exhaustFleckDef ?? FleckDefOf.GravshipThrusterExhaust;
				}
			}

			// Token: 0x0400DC59 RID: 56409
			public bool enabled = true;

			// Token: 0x0400DC5A RID: 56410
			private FleckDef exhaustFleckDef;

			// Token: 0x0400DC5B RID: 56411
			public float emissionsPerSecond = 10f;

			// Token: 0x0400DC5C RID: 56412
			public Vector3 spawnOffset = new Vector3(0f, 0f, 0f);

			// Token: 0x0400DC5D RID: 56413
			public FloatRange spawnRadiusRange = new FloatRange(0f);

			// Token: 0x0400DC5E RID: 56414
			public bool inheritGravshipRotation = true;

			// Token: 0x0400DC5F RID: 56415
			public bool inheritThrusterRotation = true;

			// Token: 0x0400DC60 RID: 56416
			public Vector3 velocity = new Vector3(0f, 0f, -1f);

			// Token: 0x0400DC61 RID: 56417
			public FloatRange velocityRotationRange = new FloatRange(0f);

			// Token: 0x0400DC62 RID: 56418
			public FloatRange velocityMultiplierRange = new FloatRange(1f);

			// Token: 0x0400DC63 RID: 56419
			public FloatRange rotationOverTimeRange = new FloatRange(0f);

			// Token: 0x0400DC64 RID: 56420
			public FloatRange scaleRange = new FloatRange(1f);
		}
	}
}
