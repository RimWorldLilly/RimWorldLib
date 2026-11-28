using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001639 RID: 5689
	public class PreceptComp_SituationalThought : PreceptComp_Thought
	{
		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x06008E91 RID: 36497 RVA: 0x003467EF File Offset: 0x003449EF
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}
	}
}
