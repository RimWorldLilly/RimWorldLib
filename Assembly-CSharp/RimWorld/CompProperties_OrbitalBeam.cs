using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDF RID: 3807
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x060063AA RID: 25514 RVA: 0x0023C179 File Offset: 0x0023A379
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x0023C1A7 File Offset: 0x0023A3A7
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_OrbitalBeam.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_OrbitalBeam.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003804 RID: 14340
		public float width = 8f;

		// Token: 0x04003805 RID: 14341
		public Color color = Color.white;

		// Token: 0x04003806 RID: 14342
		public SoundDef sound;
	}
}
