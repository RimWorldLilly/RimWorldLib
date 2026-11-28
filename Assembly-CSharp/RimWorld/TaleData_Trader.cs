using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D4 RID: 6100
	public class TaleData_Trader : TaleData
	{
		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x06009832 RID: 38962 RVA: 0x00372E5A File Offset: 0x0037105A
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		// Token: 0x06009833 RID: 38963 RVA: 0x00372E68 File Offset: 0x00371068
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		// Token: 0x06009834 RID: 38964 RVA: 0x00372EA0 File Offset: 0x003710A0
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			TaleData_Trader.<GetRules>d__6 <GetRules>d__ = new TaleData_Trader.<GetRules>d__6(-2);
			<GetRules>d__.<>4__this = this;
			<GetRules>d__.<>3__prefix = prefix;
			return <GetRules>d__;
		}

		// Token: 0x06009835 RID: 38965 RVA: 0x00372EB8 File Offset: 0x003710B8
		public static TaleData_Trader GenerateFrom(ITrader trader)
		{
			TaleData_Trader taleData_Trader = new TaleData_Trader();
			taleData_Trader.name = trader.TraderName;
			Pawn pawn = trader as Pawn;
			if (pawn != null)
			{
				taleData_Trader.pawnID = pawn.thingIDNumber;
				taleData_Trader.gender = pawn.gender;
			}
			return taleData_Trader;
		}

		// Token: 0x06009836 RID: 38966 RVA: 0x00372EFC File Offset: 0x003710FC
		public static TaleData_Trader GenerateRandom()
		{
			PawnKindDef pawnKindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.trader
			select d).RandomElement<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionDef), null);
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			return TaleData_Trader.GenerateFrom(pawn);
		}

		// Token: 0x04005B1B RID: 23323
		public string name;

		// Token: 0x04005B1C RID: 23324
		public int pawnID = -1;

		// Token: 0x04005B1D RID: 23325
		public Gender gender = Gender.Male;
	}
}
