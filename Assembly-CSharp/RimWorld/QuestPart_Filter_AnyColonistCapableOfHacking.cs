using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001063 RID: 4195
	public class QuestPart_Filter_AnyColonistCapableOfHacking : QuestPart_Filter
	{
		// Token: 0x0600696C RID: 26988 RVA: 0x002530EC File Offset: 0x002512EC
		protected override bool Pass(SignalArgs args)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.mapParent.Map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (HackUtility.IsCapableOfHacking(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x0025316C File Offset: 0x0025136C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0400430F RID: 17167
		public MapParent mapParent;
	}
}
