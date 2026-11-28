using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B3 RID: 6067
	public class ScenPart_PlayerFaction : ScenPart
	{
		// Token: 0x0600973C RID: 38716 RVA: 0x0036DD84 File Offset: 0x0036BF84
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.factionDef == null)
			{
				this.Randomize();
				Log.Error("ScenPart had null faction after loading. Changing to " + this.factionDef.ToStringSafe<FactionDef>());
			}
		}

		// Token: 0x0600973D RID: 38717 RVA: 0x0036DDD8 File Offset: 0x0036BFD8
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.factionDef.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef localFd2 in from d in DefDatabase<FactionDef>.AllDefs
				where d.isPlayer
				select d)
				{
					FactionDef localFd = localFd2;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.factionDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600973E RID: 38718 RVA: 0x0036DED8 File Offset: 0x0036C0D8
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(this.factionDef.label);
		}

		// Token: 0x0600973F RID: 38719 RVA: 0x0036DEF9 File Offset: 0x0036C0F9
		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		// Token: 0x06009740 RID: 38720 RVA: 0x0036DF30 File Offset: 0x0036C130
		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(this.factionDef, default(IdeoGenerationParms), null));
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
		}

		// Token: 0x06009741 RID: 38721 RVA: 0x0036DF80 File Offset: 0x0036C180
		public override void PreMapGenerate()
		{
			PlanetTile startingTile = Find.GameInitData.startingTile;
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(startingTile.LayerDef.SettlementWorldObjectDef);
			settlement.SetFaction(Find.GameInitData.playerFaction);
			settlement.Tile = startingTile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
		}

		// Token: 0x06009742 RID: 38722 RVA: 0x0036DFF1 File Offset: 0x0036C1F1
		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
			TaleRecorder.RecordTale(TaleDefOf.TileSettled, Array.Empty<object>()).customLabel = "NewSettlement".Translate();
		}

		// Token: 0x06009743 RID: 38723 RVA: 0x0036E021 File Offset: 0x0036C221
		public override IEnumerable<string> ConfigErrors()
		{
			ScenPart_PlayerFaction.<ConfigErrors>d__8 <ConfigErrors>d__ = new ScenPart_PlayerFaction.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06009744 RID: 38724 RVA: 0x0036E031 File Offset: 0x0036C231
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.factionDef != null) ? this.factionDef.GetHashCode() : 0);
		}

		// Token: 0x04005A9D RID: 23197
		internal FactionDef factionDef;
	}
}
