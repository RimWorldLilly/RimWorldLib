using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002367 RID: 9063
	[Obsolete]
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x17002784 RID: 10116
		// (get) Token: 0x0600E0B7 RID: 57527 RVA: 0x00538304 File Offset: 0x00536504
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x17002785 RID: 10117
		// (get) Token: 0x0600E0B8 RID: 57528 RVA: 0x00538312 File Offset: 0x00536512
		public string ForceExitAndRemoveMapCountdownTimeLeftString
		{
			get
			{
				if (!this.ForceExitAndRemoveMapCountdownActive)
				{
					return "";
				}
				return TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(this.ticksLeftToForceExitAndRemoveMap);
			}
		}

		// Token: 0x0600E0B9 RID: 57529 RVA: 0x0053832D File Offset: 0x0053652D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x0600E0BA RID: 57530 RVA: 0x00538347 File Offset: 0x00536547
		public void ResetForceExitAndRemoveMapCountdown()
		{
			this.ticksLeftToForceExitAndRemoveMap = -1;
		}

		// Token: 0x0600E0BB RID: 57531 RVA: 0x00538350 File Offset: 0x00536550
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x0600E0BC RID: 57532 RVA: 0x00538359 File Offset: 0x00536559
		public override string CompInspectStringExtra()
		{
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				return "ForceExitAndRemoveMapCountdown".Translate(this.ForceExitAndRemoveMapCountdownTimeLeftString) + ".";
			}
			return null;
		}

		// Token: 0x0600E0BD RID: 57533 RVA: 0x0053838C File Offset: 0x0053658C
		public override void CompTickInterval(int delta)
		{
			MapParent mapParent = (MapParent)this.parent;
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (mapParent.HasMap)
				{
					this.ticksLeftToForceExitAndRemoveMap--;
					if (this.ticksLeftToForceExitAndRemoveMap <= 0)
					{
						TimedForcedExit.ForceReform(mapParent);
						return;
					}
				}
				else
				{
					this.ticksLeftToForceExitAndRemoveMap = -1;
				}
			}
		}

		// Token: 0x0600E0BE RID: 57534 RVA: 0x005383DA File Offset: 0x005365DA
		public static string GetForceExitAndRemoveMapCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true, false);
		}

		// Token: 0x0600E0BF RID: 57535 RVA: 0x005383F4 File Offset: 0x005365F4
		public static void ForceReform(MapParent mapParent)
		{
			if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
			{
				Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
				Current.Game.CurrentMap = mapParent.Map;
				Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
				{
					if (mapParent.HasMap)
					{
						mapParent.Destroy();
					}
				}, true, null);
				Find.WindowStack.Add(window);
				return;
			}
			TimedForcedExit.tmpPawns.Clear();
			TimedForcedExit.tmpPawns.AddRange(from x in mapParent.Map.mapPawns.AllPawns
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			if (TimedForcedExit.tmpPawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				CaravanExitMapUtility.ExitMapAndCreateCaravan(TimedForcedExit.tmpPawns, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, PlanetTile.Invalid, true);
			}
			TimedForcedExit.tmpPawns.Clear();
			mapParent.Destroy();
		}

		// Token: 0x04009014 RID: 36884
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x04009015 RID: 36885
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
