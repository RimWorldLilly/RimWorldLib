using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016FD RID: 5885
	public abstract class RitualPosition_ThingDef : RitualPosition_NearbyThing
	{
		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x06009304 RID: 37636
		protected abstract ThingDef ThingDef { get; }

		// Token: 0x06009305 RID: 37637 RVA: 0x0035B707 File Offset: 0x00359907
		public override IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual)
		{
			RitualPosition_ThingDef.<CandidateThings>d__2 <CandidateThings>d__ = new RitualPosition_ThingDef.<CandidateThings>d__2(-2);
			<CandidateThings>d__.<>4__this = this;
			<CandidateThings>d__.<>3__ritual = ritual;
			return <CandidateThings>d__;
		}

		// Token: 0x06009306 RID: 37638 RVA: 0x0035B720 File Offset: 0x00359920
		public override IntVec3 PositionForThing(Thing t)
		{
			return t.Position + t.Rotation.Opposite.FacingCell;
		}

		// Token: 0x06009307 RID: 37639 RVA: 0x0035B750 File Offset: 0x00359950
		public override bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
		{
			IntVec3 c = this.PositionForThing(thing);
			Map map = ritualTarget.Map;
			Building edifice = c.GetEdifice(map);
			return c.Standable(map) && (edifice == null || edifice.def.Fillage == FillCategory.None) && base.IsUsableThing(thing, spot, ritualTarget);
		}

		// Token: 0x06009308 RID: 37640 RVA: 0x0035B797 File Offset: 0x00359997
		protected override Rot4 FacingDir(Thing t)
		{
			return t.Rotation;
		}
	}
}
