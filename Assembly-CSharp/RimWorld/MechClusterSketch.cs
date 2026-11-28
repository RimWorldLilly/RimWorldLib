using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001393 RID: 5011
	public class MechClusterSketch : IExposable
	{
		// Token: 0x06007BE2 RID: 31714 RVA: 0x002C277B File Offset: 0x002C097B
		public MechClusterSketch()
		{
		}

		// Token: 0x06007BE3 RID: 31715 RVA: 0x002C2783 File Offset: 0x002C0983
		public MechClusterSketch(Sketch buildingsSketch, List<MechClusterSketch.Mech> pawns, bool startDormant)
		{
			this.buildingsSketch = buildingsSketch;
			this.pawns = pawns;
			this.startDormant = startDormant;
		}

		// Token: 0x06007BE4 RID: 31716 RVA: 0x002C27A0 File Offset: 0x002C09A0
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.buildingsSketch, "buildingsSketch", Array.Empty<object>());
			Scribe_Collections.Look<MechClusterSketch.Mech>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startDormant, "startDormant", false, false);
		}

		// Token: 0x04004E17 RID: 19991
		public Sketch buildingsSketch;

		// Token: 0x04004E18 RID: 19992
		public List<MechClusterSketch.Mech> pawns;

		// Token: 0x04004E19 RID: 19993
		public bool startDormant;

		// Token: 0x02003275 RID: 12917
		public struct Mech : IExposable
		{
			// Token: 0x06012BB6 RID: 76726 RVA: 0x0063F704 File Offset: 0x0063D904
			public Mech(PawnKindDef kindDef)
			{
				this.kindDef = kindDef;
				this.position = IntVec3.Invalid;
			}

			// Token: 0x06012BB7 RID: 76727 RVA: 0x0063F718 File Offset: 0x0063D918
			public void ExposeData()
			{
				Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
				Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			}

			// Token: 0x0400CAC1 RID: 51905
			public PawnKindDef kindDef;

			// Token: 0x0400CAC2 RID: 51906
			public IntVec3 position;
		}
	}
}
