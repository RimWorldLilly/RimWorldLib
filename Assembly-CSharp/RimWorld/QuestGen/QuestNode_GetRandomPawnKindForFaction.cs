using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002213 RID: 8723
	public class QuestNode_GetRandomPawnKindForFaction : QuestNode
	{
		// Token: 0x0600D860 RID: 55392 RVA: 0x005037A3 File Offset: 0x005019A3
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x0600D861 RID: 55393 RVA: 0x005037AC File Offset: 0x005019AC
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D862 RID: 55394 RVA: 0x005037BC File Offset: 0x005019BC
		private bool SetVars(Slate slate)
		{
			Thing value = this.factionOf.GetValue(slate);
			if (value == null)
			{
				return false;
			}
			Faction faction = value.Faction;
			if (faction == null)
			{
				return false;
			}
			List<QuestNode_GetRandomPawnKindForFaction.Choice> value2 = this.choices.GetValue(slate);
			for (int i = 0; i < value2.Count; i++)
			{
				PawnKindDef var;
				if (((value2[i].factionDef != null && faction.def == value2[i].factionDef) || (!value2[i].categoryTag.NullOrEmpty() && value2[i].categoryTag == faction.def.categoryTag)) && value2[i].pawnKinds.TryRandomElement(out var))
				{
					slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
					return true;
				}
			}
			if (this.fallback.GetValue(slate) != null)
			{
				slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.fallback.GetValue(slate), false);
				return true;
			}
			return false;
		}

		// Token: 0x040089D6 RID: 35286
		public SlateRef<Thing> factionOf;

		// Token: 0x040089D7 RID: 35287
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040089D8 RID: 35288
		public SlateRef<List<QuestNode_GetRandomPawnKindForFaction.Choice>> choices;

		// Token: 0x040089D9 RID: 35289
		public SlateRef<PawnKindDef> fallback;

		// Token: 0x02003D45 RID: 15685
		public class Choice
		{
			// Token: 0x0400F1B7 RID: 61879
			public FactionDef factionDef;

			// Token: 0x0400F1B8 RID: 61880
			public string categoryTag;

			// Token: 0x0400F1B9 RID: 61881
			public List<PawnKindDef> pawnKinds;
		}
	}
}
