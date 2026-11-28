using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200030A RID: 778
	public class RoofCollapseBufferResolver
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x0008F82E File Offset: 0x0008DA2E
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0008F854 File Offset: 0x0008DA54
		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Any<IntVec3>())
			{
				this.tmpCrushedThings.Clear();
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map, this.tmpCrushedThings);
				if (this.tmpCrushedThings.Any<Thing>())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					this.tmpCrushedNames.Clear();
					for (int i = 0; i < this.tmpCrushedThings.Count; i++)
					{
						Thing thing = this.tmpCrushedThings[i];
						Corpse corpse = thing as Corpse;
						if (corpse == null || !corpse.Bugged)
						{
							string item = thing.LabelShortCap;
							if (thing.def.category == ThingCategory.Pawn)
							{
								item = thing.LabelCap;
							}
							if (!this.tmpCrushedNames.Contains(item))
							{
								this.tmpCrushedNames.Add(item);
							}
						}
					}
					foreach (string str in this.tmpCrushedNames)
					{
						stringBuilder.AppendLine("    -" + str);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), null, null, null, null, 0, true);
				}
				else
				{
					Messages.Message("RoofCollapsed".Translate(), new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.SilentInput, true);
				}
				this.tmpCrushedThings.Clear();
				roofCollapseBuffer.Clear();
			}
		}

		// Token: 0x04001427 RID: 5159
		private Map map;

		// Token: 0x04001428 RID: 5160
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x04001429 RID: 5161
		private HashSet<string> tmpCrushedNames = new HashSet<string>();
	}
}
