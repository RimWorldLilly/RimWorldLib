using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023DF RID: 9183
	public class TravellingTransporters : WorldObject, IThingHolder
	{
		// Token: 0x17002845 RID: 10309
		// (get) Token: 0x0600E4E8 RID: 58600 RVA: 0x0055044F File Offset: 0x0054E64F
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17002846 RID: 10310
		// (get) Token: 0x0600E4E9 RID: 58601 RVA: 0x0055045E File Offset: 0x0054E65E
		private Vector3 Start
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.initialTile);
			}
		}

		// Token: 0x17002847 RID: 10311
		// (get) Token: 0x0600E4EA RID: 58602 RVA: 0x00550470 File Offset: 0x0054E670
		private Vector3 End
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.destinationTile);
			}
		}

		// Token: 0x17002848 RID: 10312
		// (get) Token: 0x0600E4EB RID: 58603 RVA: 0x00550482 File Offset: 0x0054E682
		public override Vector3 DrawPos
		{
			get
			{
				return Vector3.Slerp(this.Start, this.End, this.traveledPct);
			}
		}

		// Token: 0x17002849 RID: 10313
		// (get) Token: 0x0600E4EC RID: 58604 RVA: 0x0055049B File Offset: 0x0054E69B
		public override bool ExpandingIconFlipHorizontal
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(this.Start).x > GenWorldUI.WorldToUIPosition(this.End).x;
			}
		}

		// Token: 0x1700284A RID: 10314
		// (get) Token: 0x0600E4ED RID: 58605 RVA: 0x005504BF File Offset: 0x0054E6BF
		private Thing Shuttle
		{
			get
			{
				ActiveTransporterInfo activeTransporterInfo = this.transporters.FirstOrDefault<ActiveTransporterInfo>();
				if (activeTransporterInfo == null)
				{
					return null;
				}
				return activeTransporterInfo.GetShuttle();
			}
		}

		// Token: 0x1700284B RID: 10315
		// (get) Token: 0x0600E4EE RID: 58606 RVA: 0x005504D7 File Offset: 0x0054E6D7
		public override Color ExpandingIconColor
		{
			get
			{
				return base.Faction.Color;
			}
		}

		// Token: 0x1700284C RID: 10316
		// (get) Token: 0x0600E4EF RID: 58607 RVA: 0x005504E4 File Offset: 0x0054E6E4
		public override float ExpandingIconRotation
		{
			get
			{
				if (!this.def.rotateGraphicWhenTraveling)
				{
					return base.ExpandingIconRotation;
				}
				Vector2 vector = GenWorldUI.WorldToUIPosition(this.Start);
				Vector2 vector2 = GenWorldUI.WorldToUIPosition(this.End);
				float num = Mathf.Atan2(vector2.y - vector.y, vector2.x - vector.x) * 57.29578f;
				if (num > 180f)
				{
					num -= 180f;
				}
				return num + 90f;
			}
		}

		// Token: 0x1700284D RID: 10317
		// (get) Token: 0x0600E4F0 RID: 58608 RVA: 0x0055055C File Offset: 0x0054E75C
		private float TraveledPctStepPerTick
		{
			get
			{
				Vector3 start = this.Start;
				Vector3 end = this.End;
				if (start == end)
				{
					return 1f;
				}
				float num = GenMath.SphericalDistance(start.normalized, end.normalized);
				if (num == 0f)
				{
					return 1f;
				}
				return 0.00025f / num;
			}
		}

		// Token: 0x1700284E RID: 10318
		// (get) Token: 0x0600E4F1 RID: 58609 RVA: 0x005505B0 File Offset: 0x0054E7B0
		private bool PodsHaveAnyPotentialCaravanOwner
		{
			get
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					ThingOwner innerContainer = this.transporters[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && CaravanUtility.IsOwner(pawn, base.Faction))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700284F RID: 10319
		// (get) Token: 0x0600E4F2 RID: 58610 RVA: 0x00550618 File Offset: 0x0054E818
		public bool PodsHaveAnyFreeColonist
		{
			get
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					ThingOwner innerContainer = this.transporters[i].innerContainer;
					for (int j = 0; j < innerContainer.Count; j++)
					{
						Pawn pawn = innerContainer[j] as Pawn;
						if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17002850 RID: 10320
		// (get) Token: 0x0600E4F3 RID: 58611 RVA: 0x00550681 File Offset: 0x0054E881
		public IEnumerable<Pawn> Pawns
		{
			get
			{
				TravellingTransporters.<get_Pawns>d__30 <get_Pawns>d__ = new TravellingTransporters.<get_Pawns>d__30(-2);
				<get_Pawns>d__.<>4__this = this;
				return <get_Pawns>d__;
			}
		}

		// Token: 0x0600E4F4 RID: 58612 RVA: 0x00550694 File Offset: 0x0054E894
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ActiveTransporterInfo>(ref this.transporters, "pods", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<bool>(ref this.arrived, "arrived", false, false);
			Scribe_Values.Look<PlanetTile>(ref this.initialTile, "initialTile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					this.transporters[i].parent = this;
				}
			}
		}

		// Token: 0x0600E4F5 RID: 58613 RVA: 0x0055075E File Offset: 0x0054E95E
		public override void PostAdd()
		{
			base.PostAdd();
			this.initialTile = base.Tile;
		}

		// Token: 0x0600E4F6 RID: 58614 RVA: 0x00550772 File Offset: 0x0054E972
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.traveledPct += this.TraveledPctStepPerTick * (float)delta;
			if (this.traveledPct >= 1f)
			{
				this.traveledPct = 1f;
				this.Arrived();
			}
		}

		// Token: 0x0600E4F7 RID: 58615 RVA: 0x005507B0 File Offset: 0x0054E9B0
		public void AddTransporter(ActiveTransporterInfo contents, bool justLeftTheMap)
		{
			contents.parent = this;
			this.transporters.Add(contents);
			ThingOwner innerContainer = contents.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null && !pawn.IsWorldPawn())
				{
					if (!base.Spawned)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(140, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Passing pawn ");
						defaultInterpolatedStringHandler.AppendFormatted<Pawn>(pawn);
						defaultInterpolatedStringHandler.AppendLiteral(" to world, but the TravelingTransportPod is not spawned. This means that WorldPawns can discard this pawn which can cause bugs.");
						Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					if (justLeftTheMap)
					{
						pawn.ExitMap(false, Rot4.Invalid);
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			contents.savePawnsWithReferenceMode = true;
		}

		// Token: 0x0600E4F8 RID: 58616 RVA: 0x00550868 File Offset: 0x0054EA68
		public bool ContainsPawn(Pawn p)
		{
			for (int i = 0; i < this.transporters.Count; i++)
			{
				if (this.transporters[i].innerContainer.Contains(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E4F9 RID: 58617 RVA: 0x005508A8 File Offset: 0x0054EAA8
		private void Arrived()
		{
			if (this.arrived)
			{
				return;
			}
			this.arrived = true;
			if (this.arrivalAction == null || !this.arrivalAction.StillValid(this.transporters, this.destinationTile))
			{
				this.arrivalAction = null;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].Tile == this.destinationTile)
					{
						this.arrivalAction = new TransportersArrivalAction_LandInSpecificCell(maps[i].Parent, DropCellFinder.RandomDropSpot(maps[i], true));
						break;
					}
				}
				if (this.arrivalAction == null)
				{
					if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(this.transporters, this.destinationTile))
					{
						this.arrivalAction = new TransportersArrivalAction_FormCaravan();
					}
					else
					{
						List<Caravan> caravans = Find.WorldObjects.Caravans;
						for (int j = 0; j < caravans.Count; j++)
						{
							if (caravans[j].Tile == this.destinationTile && TransportersArrivalAction_GiveToCaravan.CanGiveTo(this.transporters, caravans[j]))
							{
								this.arrivalAction = new TransportersArrivalAction_GiveToCaravan(caravans[j]);
								break;
							}
						}
					}
				}
			}
			if (this.arrivalAction != null && this.arrivalAction.ShouldUseLongEvent(this.transporters, this.destinationTile))
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoArrivalAction), "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			this.DoArrivalAction();
		}

		// Token: 0x0600E4FA RID: 58618 RVA: 0x00550A1C File Offset: 0x0054EC1C
		private void DoArrivalAction()
		{
			for (int i = 0; i < this.transporters.Count; i++)
			{
				this.transporters[i].savePawnsWithReferenceMode = false;
				this.transporters[i].parent = null;
			}
			if (this.arrivalAction != null)
			{
				try
				{
					this.arrivalAction.Arrived(this.transporters, this.destinationTile);
				}
				catch (Exception ex)
				{
					string str = "Exception in transport pods arrival action: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				this.arrivalAction = null;
			}
			else
			{
				for (int j = 0; j < this.transporters.Count; j++)
				{
					for (int k = 0; k < this.transporters[j].innerContainer.Count; k++)
					{
						Pawn pawn = this.transporters[j].innerContainer[k] as Pawn;
						if (pawn != null && (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer))
						{
							PawnBanishUtility.Banish(pawn, this.destinationTile, true);
						}
					}
				}
				bool flag = true;
				if (ModsConfig.BiotechActive)
				{
					flag = false;
					int num = 0;
					while (num < this.transporters.Count && !flag)
					{
						for (int l = 0; l < this.transporters[num].innerContainer.Count; l++)
						{
							if (this.transporters[num].innerContainer[l].def != ThingDefOf.Wastepack)
							{
								flag = true;
								break;
							}
						}
						num++;
					}
				}
				for (int m = 0; m < this.transporters.Count; m++)
				{
					for (int n = 0; n < this.transporters[m].innerContainer.Count; n++)
					{
						this.transporters[m].innerContainer[n].Notify_AbandonedAtTile(this.destinationTile);
					}
				}
				for (int num2 = 0; num2 < this.transporters.Count; num2++)
				{
					this.transporters[num2].innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
				}
				if (flag)
				{
					string key = "MessageTransportPodsArrivedAndLost";
					if (this.def == WorldObjectDefOf.TravelingShuttle)
					{
						key = "MessageShuttleArrivedContentsLost";
					}
					Messages.Message(key.Translate(), new GlobalTargetInfo(this.destinationTile), MessageTypeDefOf.NegativeEvent, true);
				}
			}
			this.transporters.Clear();
			this.Destroy();
		}

		// Token: 0x0600E4FB RID: 58619 RVA: 0x00550CB0 File Offset: 0x0054EEB0
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600E4FC RID: 58620 RVA: 0x00550CB4 File Offset: 0x0054EEB4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			for (int i = 0; i < this.transporters.Count; i++)
			{
				outChildren.Add(this.transporters[i]);
			}
		}

		// Token: 0x04009175 RID: 37237
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x04009176 RID: 37238
		public TransportersArrivalAction arrivalAction;

		// Token: 0x04009177 RID: 37239
		private List<ActiveTransporterInfo> transporters = new List<ActiveTransporterInfo>();

		// Token: 0x04009178 RID: 37240
		private bool arrived;

		// Token: 0x04009179 RID: 37241
		private PlanetTile initialTile = PlanetTile.Invalid;

		// Token: 0x0400917A RID: 37242
		private float traveledPct;

		// Token: 0x0400917B RID: 37243
		private const float TravelSpeed = 0.00025f;
	}
}
