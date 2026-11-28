using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D3 RID: 9171
	public class SpaceMapParent : MapParent, INameableWorldObject, IResourceWorldObject
	{
		// Token: 0x17002838 RID: 10296
		// (get) Token: 0x0600E486 RID: 58502 RVA: 0x0054EA7D File Offset: 0x0054CC7D
		public override bool GravShipCanLandOn
		{
			get
			{
				return !base.HasMap;
			}
		}

		// Token: 0x17002839 RID: 10297
		// (get) Token: 0x0600E487 RID: 58503 RVA: 0x0054EA88 File Offset: 0x0054CC88
		public override string Label
		{
			get
			{
				if (!string.IsNullOrEmpty(this.nameInt))
				{
					return this.nameInt;
				}
				return base.Label;
			}
		}

		// Token: 0x1700283A RID: 10298
		// (get) Token: 0x0600E488 RID: 58504 RVA: 0x0054EAA4 File Offset: 0x0054CCA4
		// (set) Token: 0x0600E489 RID: 58505 RVA: 0x0054EAAC File Offset: 0x0054CCAC
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x1700283B RID: 10299
		// (get) Token: 0x0600E48A RID: 58506 RVA: 0x0054EAB5 File Offset: 0x0054CCB5
		// (set) Token: 0x0600E48B RID: 58507 RVA: 0x0054EABD File Offset: 0x0054CCBD
		public ThingDef PreciousResource
		{
			get
			{
				return this.preciousResource;
			}
			set
			{
				this.preciousResource = value;
			}
		}

		// Token: 0x1700283C RID: 10300
		// (get) Token: 0x0600E48C RID: 58508 RVA: 0x0054EAC6 File Offset: 0x0054CCC6
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return this.def.mapGenerator ?? MapGeneratorDefOf.Space;
			}
		}

		// Token: 0x0600E48D RID: 58509 RVA: 0x0054EADC File Offset: 0x0054CCDC
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			SpaceMapParent.<GetTransportersFloatMenuOptions>d__14 <GetTransportersFloatMenuOptions>d__ = new SpaceMapParent.<GetTransportersFloatMenuOptions>d__14(-2);
			<GetTransportersFloatMenuOptions>d__.<>4__this = this;
			<GetTransportersFloatMenuOptions>d__.<>3__pods = pods;
			<GetTransportersFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetTransportersFloatMenuOptions>d__;
		}

		// Token: 0x0600E48E RID: 58510 RVA: 0x0054EAFA File Offset: 0x0054CCFA
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			SpaceMapParent.<GetShuttleFloatMenuOptions>d__15 <GetShuttleFloatMenuOptions>d__ = new SpaceMapParent.<GetShuttleFloatMenuOptions>d__15(-2);
			<GetShuttleFloatMenuOptions>d__.<>4__this = this;
			<GetShuttleFloatMenuOptions>d__.<>3__pods = pods;
			<GetShuttleFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetShuttleFloatMenuOptions>d__;
		}

		// Token: 0x0600E48F RID: 58511 RVA: 0x0054EB18 File Offset: 0x0054CD18
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps.ToList<PocketMapParent>())
			{
				if (pocketMapParent.sourceMap == base.Map && pocketMapParent.Map.mapPawns.AnyPawnBlockingMapRemoval)
				{
					return false;
				}
			}
			if (base.Map.AnyBuildingBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x0600E490 RID: 58512 RVA: 0x0054EBCC File Offset: 0x0054CDCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousResource, "preciousResource");
		}

		// Token: 0x04009163 RID: 37219
		public ThingDef preciousResource;

		// Token: 0x04009164 RID: 37220
		public string nameInt;
	}
}
