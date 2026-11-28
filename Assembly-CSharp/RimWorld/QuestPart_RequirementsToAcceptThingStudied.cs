using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EC RID: 4332
	public class QuestPart_RequirementsToAcceptThingStudied : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06006C34 RID: 27700 RVA: 0x0025FB5A File Offset: 0x0025DD5A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_RequirementsToAcceptThingStudied.<get_QuestLookTargets>d__2 <get_QuestLookTargets>d__ = new QuestPart_RequirementsToAcceptThingStudied.<get_QuestLookTargets>d__2(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x0025FB6C File Offset: 0x0025DD6C
		public override AcceptanceReport CanAccept()
		{
			Thing thing = this.thing;
			CompStudiable compStudiable = (thing != null) ? thing.TryGetComp<CompStudiable>() : null;
			if (compStudiable != null && !compStudiable.Completed)
			{
				return new AcceptanceReport("QuestRequiredThingStudied".Translate(this.thing));
			}
			return true;
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x0025FBBD File Offset: 0x0025DDBD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x040044E5 RID: 17637
		public Thing thing;
	}
}
