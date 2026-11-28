using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E10 RID: 7696
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		// Token: 0x170024EC RID: 9452
		// (get) Token: 0x0600C7F4 RID: 51188 RVA: 0x00495D1C File Offset: 0x00493F1C
		protected override float ProgressPercent
		{
			get
			{
				if (this.initialBlueprintsCount < 0)
				{
					this.initialBlueprintsCount = this.ConstructionNeeders().Count<Thing>();
				}
				if (this.initialBlueprintsCount == 0)
				{
					return 1f;
				}
				return 1f - (float)this.ConstructionNeeders().Count<Thing>() / (float)this.initialBlueprintsCount;
			}
		}

		// Token: 0x0600C7F5 RID: 51189 RVA: 0x00495D6C File Offset: 0x00493F6C
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		// Token: 0x0600C7F6 RID: 51190 RVA: 0x00495DC8 File Offset: 0x00493FC8
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (this.ConstructionNeeders().Count<Thing>() < 3)
			{
				foreach (Thing thing in this.ConstructionNeeders())
				{
					GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
				}
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040071DE RID: 29150
		private int initialBlueprintsCount = -1;
	}
}
