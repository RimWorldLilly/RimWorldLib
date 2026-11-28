using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017DA RID: 6106
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x0600986C RID: 39020 RVA: 0x003742FA File Offset: 0x003724FA
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x0600986D RID: 39021 RVA: 0x00374308 File Offset: 0x00372508
		public override string ShortSummary
		{
			get
			{
				string str = this.def.LabelCap + ": ";
				Name name = this.firstPawnData.name;
				string text = str + ((name != null) ? name.ToString() : null);
				if (this.secondPawnData != null)
				{
					string str2 = text;
					string str3 = ", ";
					Name name2 = this.secondPawnData.name;
					text = str2 + str3 + ((name2 != null) ? name2.ToString() : null);
				}
				return text;
			}
		}

		// Token: 0x0600986E RID: 39022 RVA: 0x00374378 File Offset: 0x00372578
		public Tale_DoublePawn()
		{
		}

		// Token: 0x0600986F RID: 39023 RVA: 0x00374380 File Offset: 0x00372580
		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		// Token: 0x06009870 RID: 39024 RVA: 0x003743CD File Offset: 0x003725CD
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x06009871 RID: 39025 RVA: 0x00374400 File Offset: 0x00372600
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.firstPawnData.faction == faction)
			{
				this.firstPawnData.faction = null;
			}
			if (this.secondPawnData != null && this.secondPawnData.faction == faction)
			{
				this.secondPawnData.faction = null;
			}
		}

		// Token: 0x06009872 RID: 39026 RVA: 0x0037443E File Offset: 0x0037263E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", Array.Empty<object>());
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", Array.Empty<object>());
		}

		// Token: 0x06009873 RID: 39027 RVA: 0x00374470 File Offset: 0x00372670
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants = null)
		{
			Tale_DoublePawn.<SpecialTextGenerationRules>d__11 <SpecialTextGenerationRules>d__ = new Tale_DoublePawn.<SpecialTextGenerationRules>d__11(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x06009874 RID: 39028 RVA: 0x00374487 File Offset: 0x00372687
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom(false);
			this.secondPawnData = TaleData_Pawn.GenerateRandom(false);
		}

		// Token: 0x04005B29 RID: 23337
		public TaleData_Pawn firstPawnData;

		// Token: 0x04005B2A RID: 23338
		public TaleData_Pawn secondPawnData;
	}
}
