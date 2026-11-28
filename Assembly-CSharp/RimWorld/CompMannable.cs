using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E6 RID: 6374
	public class CompMannable : ThingComp
	{
		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x06009FBD RID: 40893 RVA: 0x00394019 File Offset: 0x00392219
		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x06009FBE RID: 40894 RVA: 0x00394044 File Offset: 0x00392244
		public Pawn ManningPawn
		{
			get
			{
				if (!this.MannedNow)
				{
					return null;
				}
				return this.lastManPawn;
			}
		}

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x06009FBF RID: 40895 RVA: 0x00394056 File Offset: 0x00392256
		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		// Token: 0x06009FC0 RID: 40896 RVA: 0x00394063 File Offset: 0x00392263
		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		// Token: 0x06009FC1 RID: 40897 RVA: 0x00394090 File Offset: 0x00392290
		public override string CompInspectStringExtra()
		{
			if (this.parent.Spawned && !this.Props.planetLayerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.Props.planetLayerWhitelist.Contains(this.parent.Map.Tile.LayerDef))
			{
				return "CannotFunctionOnLayer".Translate(this.parent.Map.Tile.LayerDef.label).CapitalizeFirst().Colorize(ColoredText.WarningColor);
			}
			return null;
		}

		// Token: 0x06009FC2 RID: 40898 RVA: 0x00394126 File Offset: 0x00392326
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			CompMannable.<CompFloatMenuOptions>d__10 <CompFloatMenuOptions>d__ = new CompMannable.<CompFloatMenuOptions>d__10(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__pawn = pawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x04005E78 RID: 24184
		private int lastManTick = -1;

		// Token: 0x04005E79 RID: 24185
		private Pawn lastManPawn;
	}
}
