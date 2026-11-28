using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B62 RID: 7010
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x0600B3F2 RID: 46066 RVA: 0x003F6B62 File Offset: 0x003F4D62
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x0600B3F3 RID: 46067 RVA: 0x003F6B75 File Offset: 0x003F4D75
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x0600B3F4 RID: 46068 RVA: 0x003F6B8E File Offset: 0x003F4D8E
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x040067C0 RID: 26560
		public ThingSetMakerDef def;
	}
}
