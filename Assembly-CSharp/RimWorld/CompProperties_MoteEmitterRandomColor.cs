using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001912 RID: 6418
	public class CompProperties_MoteEmitterRandomColor : CompProperties_MoteEmitter
	{
		// Token: 0x0600A0DC RID: 41180 RVA: 0x003984FA File Offset: 0x003966FA
		public CompProperties_MoteEmitterRandomColor()
		{
			this.compClass = typeof(CompMoteEmitterRandomColor);
		}

		// Token: 0x0600A0DD RID: 41181 RVA: 0x00398512 File Offset: 0x00396712
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_MoteEmitterRandomColor.<ConfigErrors>d__2 <ConfigErrors>d__ = new CompProperties_MoteEmitterRandomColor.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005EFC RID: 24316
		public List<Color> colors;
	}
}
