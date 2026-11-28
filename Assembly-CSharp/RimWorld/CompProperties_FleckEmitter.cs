using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001886 RID: 6278
	public class CompProperties_FleckEmitter : CompProperties
	{
		// Token: 0x06009D28 RID: 40232 RVA: 0x0038834F File Offset: 0x0038654F
		public CompProperties_FleckEmitter()
		{
			this.compClass = typeof(CompFleckEmitter);
		}

		// Token: 0x06009D29 RID: 40233 RVA: 0x0038836E File Offset: 0x0038656E
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_FleckEmitter.<ConfigErrors>d__6 <ConfigErrors>d__ = new CompProperties_FleckEmitter.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005D23 RID: 23843
		public FleckDef fleck;

		// Token: 0x04005D24 RID: 23844
		public Vector3 offset;

		// Token: 0x04005D25 RID: 23845
		public int emissionInterval = -1;

		// Token: 0x04005D26 RID: 23846
		public SoundDef soundOnEmission;

		// Token: 0x04005D27 RID: 23847
		[NoTranslate]
		public string saveKeysPrefix;
	}
}
