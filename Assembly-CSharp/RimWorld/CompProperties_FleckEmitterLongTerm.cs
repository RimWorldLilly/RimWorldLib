using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DF RID: 6367
	public class CompProperties_FleckEmitterLongTerm : CompProperties
	{
		// Token: 0x06009FA3 RID: 40867 RVA: 0x00393B30 File Offset: 0x00391D30
		public CompProperties_FleckEmitterLongTerm()
		{
			this.compClass = typeof(CompFleckEmitterLongTerm);
		}

		// Token: 0x06009FA4 RID: 40868 RVA: 0x00393B7F File Offset: 0x00391D7F
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_FleckEmitterLongTerm.<ConfigErrors>d__14 <ConfigErrors>d__ = new CompProperties_FleckEmitterLongTerm.<ConfigErrors>d__14(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005E61 RID: 24161
		public FleckDef fleckDef;

		// Token: 0x04005E62 RID: 24162
		public float spawnRadius;

		// Token: 0x04005E63 RID: 24163
		public float spawnChance;

		// Token: 0x04005E64 RID: 24164
		public Vector3 spawnOffsetFromCenter = Vector3.zero;

		// Token: 0x04005E65 RID: 24165
		public float prewarmCycles;

		// Token: 0x04005E66 RID: 24166
		public float fleckScale = 1f;

		// Token: 0x04005E67 RID: 24167
		public float minRotationRate;

		// Token: 0x04005E68 RID: 24168
		public float maxRotationRate;

		// Token: 0x04005E69 RID: 24169
		public float minVelocityAngle;

		// Token: 0x04005E6A RID: 24170
		public float maxVelocityAngle;

		// Token: 0x04005E6B RID: 24171
		public float minVelocitySpeed = 1f;

		// Token: 0x04005E6C RID: 24172
		public float maxVelocitySpeed = 1f;

		// Token: 0x04005E6D RID: 24173
		public bool forceEnabled;
	}
}
