using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5C RID: 7004
	public abstract class ThingSetMaker_Conditional : ThingSetMaker
	{
		// Token: 0x0600B3DC RID: 46044 RVA: 0x003F67F8 File Offset: 0x003F49F8
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.Condition(parms) && this.thingSetMaker.CanGenerate(parms);
		}

		// Token: 0x0600B3DD RID: 46045 RVA: 0x003F6811 File Offset: 0x003F4A11
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.thingSetMaker.Generate(parms));
		}

		// Token: 0x0600B3DE RID: 46046 RVA: 0x003F6825 File Offset: 0x003F4A25
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.thingSetMaker.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x0600B3DF RID: 46047 RVA: 0x003F6833 File Offset: 0x003F4A33
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thingSetMaker.ResolveReferences();
		}

		// Token: 0x0600B3E0 RID: 46048
		protected abstract bool Condition(ThingSetMakerParams parms);

		// Token: 0x0600B3E1 RID: 46049 RVA: 0x003F6846 File Offset: 0x003F4A46
		public override IEnumerable<string> ConfigErrors()
		{
			ThingSetMaker_Conditional.<ConfigErrors>d__6 <ConfigErrors>d__ = new ThingSetMaker_Conditional.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040067B5 RID: 26549
		public ThingSetMaker thingSetMaker;
	}
}
