using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011CC RID: 4556
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x060071CF RID: 29135 RVA: 0x0027E3F0 File Offset: 0x0027C5F0
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x060071D0 RID: 29136 RVA: 0x0027E400 File Offset: 0x0027C600
		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				Difficulty difficulty = Find.Storyteller.difficulty;
				if (difficulty.deepDrillInfestationChanceFactor <= 0f)
				{
					return -1f;
				}
				return this.Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
			}
		}

		// Token: 0x060071D1 RID: 29137 RVA: 0x0027E43D File Offset: 0x0027C63D
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_DeepDrillInfestation.<MakeIntervalIncidents>d__5 <MakeIntervalIncidents>d__ = new StorytellerComp_DeepDrillInfestation.<MakeIntervalIncidents>d__5(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x040047F5 RID: 18421
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
