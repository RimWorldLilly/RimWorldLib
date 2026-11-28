using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002199 RID: 8601
	public class QuestNode_GenerateTransportShip : QuestNode
	{
		// Token: 0x0600D69E RID: 54942 RVA: 0x004FB448 File Offset: 0x004F9648
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			TransportShip transportShip = TransportShipMaker.MakeTransportShip(this.def.GetValue(slate), null, this.shipThing.GetValue(slate));
			if (!this.storeAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<TransportShip>(this.storeAs.GetValue(slate), transportShip, false);
			}
			QuestPart_SetupTransportShip questPart_SetupTransportShip = new QuestPart_SetupTransportShip();
			questPart_SetupTransportShip.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetupTransportShip.transportShip = transportShip;
			List<Thing> items;
			if (!(this.contents != null))
			{
				items = null;
			}
			else
			{
				items = (from c in this.contents.GetValue(slate)
				where !(c is Pawn)
				select c).ToList<Thing>();
			}
			questPart_SetupTransportShip.items = items;
			questPart_SetupTransportShip.pawns = ((this.contents != null) ? this.contents.GetValue(slate).OfType<Pawn>().ToList<Pawn>() : null);
			QuestPart_SetupTransportShip part = questPart_SetupTransportShip;
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x0600D69F RID: 54943 RVA: 0x004FB568 File Offset: 0x004F9768
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040087EB RID: 34795
		public SlateRef<TransportShipDef> def;

		// Token: 0x040087EC RID: 34796
		public SlateRef<Thing> shipThing;

		// Token: 0x040087ED RID: 34797
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x040087EE RID: 34798
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040087EF RID: 34799
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
