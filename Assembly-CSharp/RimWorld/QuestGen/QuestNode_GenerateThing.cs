using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B2 RID: 8626
	public class QuestNode_GenerateThing : QuestNode
	{
		// Token: 0x0600D6F0 RID: 55024 RVA: 0x004FCA23 File Offset: 0x004FAC23
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D6F1 RID: 55025 RVA: 0x004FCA28 File Offset: 0x004FAC28
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(this.def.GetValue(slate), null);
			thing.stackCount = this.stackCount.GetValue(slate).GetValueOrDefault(1);
			if (this.contents.GetValue(slate) != null)
			{
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null)
				{
					thingOwner.TryAddRangeOrTransfer(this.contents.GetValue(slate), true, false);
				}
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(slate, this.addToList.GetValue(slate), thing);
			}
		}

		// Token: 0x0400884C RID: 34892
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400884D RID: 34893
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x0400884E RID: 34894
		public SlateRef<ThingDef> def;

		// Token: 0x0400884F RID: 34895
		public SlateRef<int?> stackCount;

		// Token: 0x04008850 RID: 34896
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
