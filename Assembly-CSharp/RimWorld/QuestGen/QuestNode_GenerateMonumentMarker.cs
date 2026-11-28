using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021AC RID: 8620
	public class QuestNode_GenerateMonumentMarker : QuestNode
	{
		// Token: 0x0600D6DE RID: 55006 RVA: 0x004FC285 File Offset: 0x004FA485
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D6DF RID: 55007 RVA: 0x004FC288 File Offset: 0x004FA488
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker monumentMarker = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
			monumentMarker.sketch = this.sketch.GetValue(slate);
			slate.Set<MonumentMarker>(this.storeAs.GetValue(slate), monumentMarker, false);
		}

		// Token: 0x0400881E RID: 34846
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400881F RID: 34847
		public SlateRef<Sketch> sketch;
	}
}
