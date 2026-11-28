using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D6 RID: 4566
	public class StorytellerComp_ImportantQuest : StorytellerComp
	{
		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x060071E7 RID: 29159 RVA: 0x0027E699 File Offset: 0x0027C899
		private static int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x060071E8 RID: 29160 RVA: 0x0027E6AB File Offset: 0x0027C8AB
		private StorytellerCompProperties_ImportantQuest Props
		{
			get
			{
				return (StorytellerCompProperties_ImportantQuest)this.props;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x060071E9 RID: 29161 RVA: 0x0027E6B8 File Offset: 0x0027C8B8
		private bool BeenGivenQuest
		{
			get
			{
				return Find.QuestManager.QuestsListForReading.Any((Quest q) => q.root == this.Props.questDef);
			}
		}

		// Token: 0x060071EA RID: 29162 RVA: 0x0027E6D5 File Offset: 0x0027C8D5
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_ImportantQuest.<MakeIntervalIncidents>d__6 <MakeIntervalIncidents>d__ = new StorytellerComp_ImportantQuest.<MakeIntervalIncidents>d__6(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}
	}
}
