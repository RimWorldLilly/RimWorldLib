using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B0 RID: 8624
	public class QuestNode_GenerateShuttle : QuestNode
	{
		// Token: 0x0600D6EA RID: 55018 RVA: 0x004FC79D File Offset: 0x004FA99D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D6EB RID: 55019 RVA: 0x004FC7A0 File Offset: 0x004FA9A0
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Shuttle"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
			if (this.owningFaction.GetValue(slate) != null)
			{
				thing.SetFaction(this.owningFaction.GetValue(slate), null);
			}
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (this.requiredPawns.GetValue(slate) != null)
			{
				compShuttle.requiredPawns.AddRange(this.requiredPawns.GetValue(slate));
			}
			if (this.requiredItems.GetValue(slate) != null)
			{
				compShuttle.requiredItems.AddRange(this.requiredItems.GetValue(slate));
			}
			compShuttle.acceptColonists = this.acceptColonists.GetValue(slate);
			compShuttle.acceptChildren = this.acceptChildren.GetValue(slate).GetValueOrDefault(true);
			compShuttle.onlyAcceptColonists = this.onlyAcceptColonists.GetValue(slate);
			compShuttle.onlyAcceptHealthy = this.onlyAcceptHealthy.GetValue(slate);
			compShuttle.requiredColonistCount = this.requireColonistCount.GetValue(slate);
			compShuttle.permitShuttle = this.permitShuttle.GetValue(slate);
			compShuttle.minAge = this.minAge.GetValue(slate).GetValueOrDefault();
			float num;
			if (this.overrideMass.TryGetValue(slate, out num) && num > 0f)
			{
				compShuttle.Transporter.massCapacityOverride = num;
			}
			QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
		}

		// Token: 0x04008838 RID: 34872
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008839 RID: 34873
		public SlateRef<IEnumerable<Pawn>> requiredPawns;

		// Token: 0x0400883A RID: 34874
		public SlateRef<IEnumerable<ThingDefCount>> requiredItems;

		// Token: 0x0400883B RID: 34875
		public SlateRef<int> requireColonistCount;

		// Token: 0x0400883C RID: 34876
		public SlateRef<bool> acceptColonists;

		// Token: 0x0400883D RID: 34877
		public SlateRef<bool?> acceptChildren;

		// Token: 0x0400883E RID: 34878
		public SlateRef<bool> onlyAcceptColonists;

		// Token: 0x0400883F RID: 34879
		public SlateRef<bool> onlyAcceptHealthy;

		// Token: 0x04008840 RID: 34880
		public SlateRef<Faction> owningFaction;

		// Token: 0x04008841 RID: 34881
		public SlateRef<bool> permitShuttle;

		// Token: 0x04008842 RID: 34882
		public SlateRef<float> overrideMass;

		// Token: 0x04008843 RID: 34883
		public SlateRef<float?> minAge;
	}
}
