using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02002295 RID: 8853
	public class QuestPart_SightstealerWave : QuestPart_MakeLord
	{
		// Token: 0x0600DA9E RID: 55966 RVA: 0x005161A3 File Offset: 0x005143A3
		public QuestPart_SightstealerWave()
		{
		}

		// Token: 0x0600DA9F RID: 55967 RVA: 0x005161AB File Offset: 0x005143AB
		public QuestPart_SightstealerWave(string inSignal, MapParent mapParent, IEnumerable<Pawn> sightstealers, string waveTag)
		{
			this.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			this.mapParent = mapParent;
			this.pawns.AddRange(sightstealers);
			this.waveTag = waveTag;
		}

		// Token: 0x0600DAA0 RID: 55968 RVA: 0x005161EA File Offset: 0x005143EA
		protected override Lord MakeLord()
		{
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_SightstealerAssault(), this.mapParent.Map, null);
			QuestUtility.AddQuestTag(lord, this.waveTag);
			return lord;
		}

		// Token: 0x0600DAA1 RID: 55969 RVA: 0x00516213 File Offset: 0x00514413
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.waveTag, "waveTag", null, false);
		}

		// Token: 0x04008BC6 RID: 35782
		public string waveTag;
	}
}
