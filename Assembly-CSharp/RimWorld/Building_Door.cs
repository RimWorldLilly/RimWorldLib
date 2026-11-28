using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AA9 RID: 6825
	public class Building_Door : Building
	{
		// Token: 0x17001EB3 RID: 7859
		// (get) Token: 0x0600AC71 RID: 44145 RVA: 0x003CD451 File Offset: 0x003CB651
		private int CloseDelayAdjusted
		{
			get
			{
				return Mathf.FloorToInt(110f * (this.DoorPowerOn ? this.def.building.poweredDoorCloseSpeedFactor : this.def.building.unpoweredDoorCloseSpeedFactor));
			}
		}

		// Token: 0x17001EB4 RID: 7860
		// (get) Token: 0x0600AC72 RID: 44146 RVA: 0x003CD488 File Offset: 0x003CB688
		private int WillCloseSoonThreshold
		{
			get
			{
				return this.CloseDelayAdjusted + 1;
			}
		}

		// Token: 0x17001EB5 RID: 7861
		// (get) Token: 0x0600AC73 RID: 44147 RVA: 0x003CD492 File Offset: 0x003CB692
		public bool Open
		{
			get
			{
				return this.openInt;
			}
		}

		// Token: 0x17001EB6 RID: 7862
		// (get) Token: 0x0600AC74 RID: 44148 RVA: 0x003CD49A File Offset: 0x003CB69A
		public bool HoldOpen
		{
			get
			{
				return this.holdOpenInt;
			}
		}

		// Token: 0x17001EB7 RID: 7863
		// (get) Token: 0x0600AC75 RID: 44149 RVA: 0x003CD4A2 File Offset: 0x003CB6A2
		protected virtual bool CheckFaction
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EB8 RID: 7864
		// (get) Token: 0x0600AC76 RID: 44150 RVA: 0x003CD4A5 File Offset: 0x003CB6A5
		protected virtual bool CanDrawMovers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EB9 RID: 7865
		// (get) Token: 0x0600AC77 RID: 44151 RVA: 0x003CD4A8 File Offset: 0x003CB6A8
		public override bool ExchangeVacuum
		{
			get
			{
				return base.ExchangeVacuum || this.Open;
			}
		}

		// Token: 0x17001EBA RID: 7866
		// (get) Token: 0x0600AC78 RID: 44152 RVA: 0x003CD4BA File Offset: 0x003CB6BA
		protected virtual float TempEqualizeRate
		{
			get
			{
				return this.def.building.doorTempEqualizeRate;
			}
		}

		// Token: 0x17001EBB RID: 7867
		// (get) Token: 0x0600AC79 RID: 44153 RVA: 0x003CD4CC File Offset: 0x003CB6CC
		public virtual bool FreePassage
		{
			get
			{
				return this.openInt && (this.holdOpenInt || !this.WillCloseSoon);
			}
		}

		// Token: 0x17001EBC RID: 7868
		// (get) Token: 0x0600AC7A RID: 44154 RVA: 0x003CD4EC File Offset: 0x003CB6EC
		public int TicksTillFullyOpened
		{
			get
			{
				int num = this.TicksToOpenNow - this.ticksSinceOpen;
				if (num < 0)
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x0600AC7B RID: 44155 RVA: 0x003CD50E File Offset: 0x003CB70E
		public void SnapshotWillCloseSoonForJobs()
		{
			this.willCloseSoonForJobs = this.WillCloseSoon;
		}

		// Token: 0x17001EBD RID: 7869
		// (get) Token: 0x0600AC7C RID: 44156 RVA: 0x003CD51C File Offset: 0x003CB71C
		public bool WillCloseSoon
		{
			get
			{
				if (!UnityData.IsInMainThread)
				{
					return this.willCloseSoonForJobs;
				}
				if (!base.Spawned)
				{
					return true;
				}
				if (!this.openInt)
				{
					return true;
				}
				if (this.holdOpenInt)
				{
					return false;
				}
				if (this.ticksUntilClose > 0 && this.ticksUntilClose <= this.WillCloseSoonThreshold && !this.BlockedOpenMomentary)
				{
					return true;
				}
				if (this.CanTryCloseAutomatically && !this.BlockedOpenMomentary)
				{
					return true;
				}
				foreach (IntVec3 intVec in this.OccupiedRect())
				{
					for (int i = 0; i < 5; i++)
					{
						IntVec3 c = intVec + GenAdj.CardinalDirectionsAndInside[i];
						if (c.InBounds(base.Map))
						{
							List<Thing> thingList = c.GetThingList(base.Map);
							for (int j = 0; j < thingList.Count; j++)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.HostileTo(this) && !pawn.Downed && this.PawnCanOpen(pawn) && (pawn.Position == intVec || (pawn.pather.Moving && pawn.pather.nextCell == intVec)))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001EBE RID: 7870
		// (get) Token: 0x0600AC7D RID: 44157 RVA: 0x003CD698 File Offset: 0x003CB898
		public bool ContainmentBreached
		{
			get
			{
				return this.openInt && this.ticksOpen >= 600;
			}
		}

		// Token: 0x17001EBF RID: 7871
		// (get) Token: 0x0600AC7E RID: 44158 RVA: 0x003CD6B4 File Offset: 0x003CB8B4
		public bool BlockedOpenMomentary
		{
			get
			{
				if (this.StuckOpen)
				{
					return true;
				}
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing thing = thingList[i];
						if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || thing is Corpse)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001EC0 RID: 7872
		// (get) Token: 0x0600AC7F RID: 44159 RVA: 0x003CD764 File Offset: 0x003CB964
		protected virtual bool AlwaysOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001EC1 RID: 7873
		// (get) Token: 0x0600AC80 RID: 44160 RVA: 0x003CD768 File Offset: 0x003CB968
		protected bool StuckOpen
		{
			get
			{
				if (this.AlwaysOpen)
				{
					return true;
				}
				if (!base.Spawned || this.def.size == IntVec2.One)
				{
					return false;
				}
				using (IEnumerator<IntVec3> enumerator = DoorUtility.WallRequirementCells(this.def, base.Position, base.Rotation).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!DoorUtility.EncapsulatingWallAt(enumerator.Current, base.Map, false))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001EC2 RID: 7874
		// (get) Token: 0x0600AC81 RID: 44161 RVA: 0x003CD800 File Offset: 0x003CBA00
		public bool DoorPowerOn
		{
			get
			{
				return this.powerComp != null && this.powerComp.PowerOn;
			}
		}

		// Token: 0x17001EC3 RID: 7875
		// (get) Token: 0x0600AC82 RID: 44162 RVA: 0x003CD817 File Offset: 0x003CBA17
		public bool SlowsPawns
		{
			get
			{
				return !this.DoorPowerOn || this.TicksToOpenNow > 20;
			}
		}

		// Token: 0x17001EC4 RID: 7876
		// (get) Token: 0x0600AC83 RID: 44163 RVA: 0x003CD830 File Offset: 0x003CBA30
		public int TicksToOpenNow
		{
			get
			{
				float num = 45f / this.GetStatValue(StatDefOf.DoorOpenSpeed, true, -1);
				if (this.DoorPowerOn)
				{
					num *= 0.25f * this.def.building.poweredDoorOpenSpeedFactor;
				}
				else
				{
					num *= this.def.building.unpoweredDoorOpenSpeedFactor;
				}
				return Mathf.RoundToInt(num);
			}
		}

		// Token: 0x17001EC5 RID: 7877
		// (get) Token: 0x0600AC84 RID: 44164 RVA: 0x003CD88D File Offset: 0x003CBA8D
		private bool CanTryCloseAutomatically
		{
			get
			{
				return !this.AlwaysOpen && this.FriendlyTouchedRecently && !this.HoldOpen;
			}
		}

		// Token: 0x17001EC6 RID: 7878
		// (get) Token: 0x0600AC85 RID: 44165 RVA: 0x003CD8AA File Offset: 0x003CBAAA
		private bool FriendlyTouchedRecently
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastFriendlyTouchTick + 120;
			}
		}

		// Token: 0x17001EC7 RID: 7879
		// (get) Token: 0x0600AC86 RID: 44166 RVA: 0x003CD8C1 File Offset: 0x003CBAC1
		public override bool FireBulwark
		{
			get
			{
				return !this.Open && base.FireBulwark;
			}
		}

		// Token: 0x17001EC8 RID: 7880
		// (get) Token: 0x0600AC87 RID: 44167 RVA: 0x003CD8D3 File Offset: 0x003CBAD3
		protected virtual float OpenPct
		{
			get
			{
				return Mathf.Clamp01((float)this.ticksSinceOpen / (float)this.TicksToOpenNow);
			}
		}

		// Token: 0x0600AC88 RID: 44168 RVA: 0x003CD8E9 File Offset: 0x003CBAE9
		public override void PostMake()
		{
			base.PostMake();
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x0600AC89 RID: 44169 RVA: 0x003CD8FD File Offset: 0x003CBAFD
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.ClearReachabilityCache(map);
			if (this.BlockedOpenMomentary)
			{
				this.DoorOpen(110);
			}
		}

		// Token: 0x0600AC8A RID: 44170 RVA: 0x003CD92C File Offset: 0x003CBB2C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			this.ClearReachabilityCache(map);
		}

		// Token: 0x0600AC8B RID: 44171 RVA: 0x003CD950 File Offset: 0x003CBB50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openInt, "open", false, false);
			Scribe_Values.Look<bool>(ref this.holdOpenInt, "holdOpen", false, false);
			Scribe_Values.Look<int>(ref this.lastFriendlyTouchTick, "lastFriendlyTouchTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilClose, "ticksUntilClose", 0, false);
			Scribe_References.Look<Pawn>(ref this.approachingPawn, "approachingPawn", false);
			Scribe_Values.Look<int>(ref this.ticksOpen, "ticksOpen", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.openInt)
			{
				this.ticksSinceOpen = this.TicksToOpenNow;
			}
		}

		// Token: 0x0600AC8C RID: 44172 RVA: 0x003CD9EA File Offset: 0x003CBBEA
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				this.ClearReachabilityCache(base.Map);
			}
		}

		// Token: 0x0600AC8D RID: 44173 RVA: 0x003CDA08 File Offset: 0x003CBC08
		protected override void Tick()
		{
			base.Tick();
			if (this.FreePassage != this.freePassageWhenClearedReachabilityCache)
			{
				this.ClearReachabilityCache(base.Map);
			}
			if (!this.openInt)
			{
				if (this.ticksSinceOpen > 0)
				{
					this.ticksSinceOpen--;
				}
				this.ticksOpen = 0;
				if (this.IsHashIntervalTick(this.def.building.doorTempEqualizeIntervalClosed))
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, this.TempEqualizeRate, false);
					return;
				}
			}
			else if (this.openInt)
			{
				if (this.ticksSinceOpen < this.TicksToOpenNow)
				{
					this.ticksSinceOpen++;
				}
				this.ticksOpen++;
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null)
						{
							this.CheckFriendlyTouched(pawn);
						}
					}
				}
				if (this.ticksUntilClose > 0 && !this.AlwaysOpen)
				{
					foreach (IntVec3 c2 in this.OccupiedRect())
					{
						if (base.Map.thingGrid.CellContains(c2, ThingCategory.Pawn))
						{
							this.ticksUntilClose = this.CloseDelayAdjusted;
							break;
						}
					}
					this.ticksUntilClose--;
					if (this.ticksUntilClose <= 0 && !this.holdOpenInt && !this.DoorTryClose())
					{
						this.ticksUntilClose = 1;
					}
				}
				else if (this.CanTryCloseAutomatically)
				{
					this.ticksUntilClose = this.CloseDelayAdjusted;
				}
				if ((Find.TickManager.TicksGame + this.thingIDNumber.HashOffset()) % this.def.building.doorTempEqualizeIntervalOpen == 0)
				{
					GenTemperature.EqualizeTemperaturesThroughBuilding(this, this.TempEqualizeRate, false);
				}
				if (this.OpenPct >= 0.4f && this.approachingPawn != null)
				{
					base.Map.fogGrid.Notify_PawnEnteringDoor(this, this.approachingPawn);
					this.approachingPawn = null;
				}
			}
		}

		// Token: 0x0600AC8E RID: 44174 RVA: 0x003CDC58 File Offset: 0x003CBE58
		public void CheckFriendlyTouched(Pawn p)
		{
			if (!p.HostileTo(this) && this.PawnCanOpen(p))
			{
				this.lastFriendlyTouchTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600AC8F RID: 44175 RVA: 0x003CDC7C File Offset: 0x003CBE7C
		public void Notify_PawnApproaching(Pawn p, float moveCost)
		{
			this.CheckFriendlyTouched(p);
			bool flag = this.PawnCanOpen(p);
			if (flag || this.Open)
			{
				this.approachingPawn = p;
			}
			if (flag && !this.SlowsPawns)
			{
				this.DoorOpen(Mathf.Max(300, Mathf.CeilToInt(moveCost) + 1));
			}
		}

		// Token: 0x0600AC90 RID: 44176 RVA: 0x003CDCCB File Offset: 0x003CBECB
		public bool CanPhysicallyPass(Pawn p)
		{
			return this.FreePassage || this.PawnCanOpen(p) || (this.Open && p.HostileTo(this));
		}

		// Token: 0x0600AC91 RID: 44177 RVA: 0x003CDCF4 File Offset: 0x003CBEF4
		public virtual bool PawnCanOpen(Pawn p)
		{
			if (!p.CanOpenDoors)
			{
				return false;
			}
			Map map = base.Map;
			if (((map != null) ? map.Parent : null) != null && base.Map.Parent.doorsAlwaysOpenForPlayerPawns && p.Faction == Faction.OfPlayer)
			{
				return true;
			}
			if (p.CanOpenAnyDoor)
			{
				return true;
			}
			if (p.FenceBlocked && !this.def.building.roamerCanOpen && (!p.roping.IsRopedByPawn || !this.PawnCanOpen(p.roping.RopedByPawn)))
			{
				return false;
			}
			if (base.Faction == null)
			{
				return p.RaceProps.canOpenFactionlessDoors;
			}
			return (p.guest != null && p.guest.Released) || !this.CheckFaction || GenAI.MachinesLike(base.Faction, p);
		}

		// Token: 0x0600AC92 RID: 44178 RVA: 0x003CDDC7 File Offset: 0x003CBFC7
		public override bool BlocksPawn(Pawn p)
		{
			return !this.openInt && !this.PawnCanOpen(p);
		}

		// Token: 0x0600AC93 RID: 44179 RVA: 0x003CDDE0 File Offset: 0x003CBFE0
		protected virtual void DoorOpen(int ticksToClose = 110)
		{
			if (this.openInt)
			{
				this.ticksUntilClose = ticksToClose;
			}
			else
			{
				this.ticksUntilClose = this.TicksToOpenNow + ticksToClose;
			}
			if (!this.openInt)
			{
				this.openInt = true;
				this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
				if (this.DoorPowerOn)
				{
					this.def.building.soundDoorOpenPowered.PlayOneShot(this);
				}
				else
				{
					this.def.building.soundDoorOpenManual.PlayOneShot(this);
				}
				base.Map.events.Notify_DoorOpened(this);
			}
		}

		// Token: 0x0600AC94 RID: 44180 RVA: 0x003CDE74 File Offset: 0x003CC074
		protected bool DoorTryClose()
		{
			if (this.holdOpenInt || this.BlockedOpenMomentary)
			{
				return false;
			}
			this.openInt = false;
			this.CheckClearReachabilityCacheBecauseOpenedOrClosed();
			if (this.DoorPowerOn)
			{
				this.def.building.soundDoorClosePowered.PlayOneShot(this);
			}
			else
			{
				this.def.building.soundDoorCloseManual.PlayOneShot(this);
			}
			base.Map.events.Notify_DoorClosed(this);
			return true;
		}

		// Token: 0x0600AC95 RID: 44181 RVA: 0x003CDEF2 File Offset: 0x003CC0F2
		public void StartManualOpenBy(Pawn opener)
		{
			this.DoorOpen(110);
		}

		// Token: 0x0600AC96 RID: 44182 RVA: 0x003CDEFC File Offset: 0x003CC0FC
		public void StartManualCloseBy(Pawn closer)
		{
			this.ticksUntilClose = this.CloseDelayAdjusted;
		}

		// Token: 0x0600AC97 RID: 44183 RVA: 0x003CDF0C File Offset: 0x003CC10C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DoorPreDraw();
			if (this.CanDrawMovers)
			{
				float offsetDist = 0f + 0.45f * this.OpenPct;
				this.DrawMovers(drawLoc, offsetDist, this.Graphic, AltitudeLayer.DoorMoveable.AltitudeFor(), Vector3.one, this.Graphic.ShadowGraphic);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x0600AC98 RID: 44184 RVA: 0x003CDF68 File Offset: 0x003CC168
		protected void DrawMovers(Vector3 drawPos, float offsetDist, Graphic graphic, float altitude, Vector3 drawScaleFactor, Graphic_Shadow shadowGraphic)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector;
				Mesh mesh;
				if (i == 0)
				{
					vector = new Vector3(0f, 0f, (float)(-(float)this.def.size.x));
					mesh = MeshPool.plane10;
				}
				else
				{
					vector = new Vector3(0f, 0f, (float)this.def.size.x);
					mesh = MeshPool.plane10Flip;
				}
				Rot4 rotation = base.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				vector = rotation.AsQuat * vector;
				Vector3 vector2 = drawPos;
				vector2.y = altitude;
				vector2 += vector * offsetDist;
				Graphics.DrawMesh(mesh, Matrix4x4.TRS(vector2, base.Rotation.AsQuat, new Vector3((float)this.def.size.x * drawScaleFactor.x, drawScaleFactor.y, (float)this.def.size.z * drawScaleFactor.z)), graphic.MatAt(base.Rotation, this), 0);
				if (shadowGraphic != null)
				{
					shadowGraphic.DrawWorker(vector2, base.Rotation, this.def, this, 0f);
				}
			}
		}

		// Token: 0x0600AC99 RID: 44185 RVA: 0x003CE09D File Offset: 0x003CC29D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Door.<GetGizmos>d__81 <GetGizmos>d__ = new Building_Door.<GetGizmos>d__81(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AC9A RID: 44186 RVA: 0x003CE0AD File Offset: 0x003CC2AD
		private void ClearReachabilityCache(Map map)
		{
			map.reachability.ClearCache();
			this.freePassageWhenClearedReachabilityCache = this.FreePassage;
		}

		// Token: 0x0600AC9B RID: 44187 RVA: 0x003CE0C6 File Offset: 0x003CC2C6
		private void CheckClearReachabilityCacheBecauseOpenedOrClosed()
		{
			if (base.Spawned)
			{
				base.Map.reachability.ClearCacheForHostile(this);
			}
		}

		// Token: 0x0600AC9C RID: 44188 RVA: 0x003CE0E1 File Offset: 0x003CC2E1
		protected void DoorPreDraw()
		{
			if (this.def.size == IntVec2.One)
			{
				base.Rotation = DoorUtility.DoorRotationAt(base.Position, base.Map, this.def.building.preferConnectingToFences);
			}
		}

		// Token: 0x0600AC9D RID: 44189 RVA: 0x003CE124 File Offset: 0x003CC324
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.StuckOpen && !this.AlwaysOpen)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "DoorMustBeEnclosedByWalls".Translate().Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x04006424 RID: 25636
		public CompPowerTrader powerComp;

		// Token: 0x04006425 RID: 25637
		protected bool openInt;

		// Token: 0x04006426 RID: 25638
		protected bool holdOpenInt;

		// Token: 0x04006427 RID: 25639
		private int lastFriendlyTouchTick = -9999;

		// Token: 0x04006428 RID: 25640
		protected int ticksUntilClose;

		// Token: 0x04006429 RID: 25641
		private Pawn approachingPawn;

		// Token: 0x0400642A RID: 25642
		private int ticksOpen;

		// Token: 0x0400642B RID: 25643
		protected int ticksSinceOpen;

		// Token: 0x0400642C RID: 25644
		private bool freePassageWhenClearedReachabilityCache;

		// Token: 0x0400642D RID: 25645
		private bool willCloseSoonForJobs;

		// Token: 0x0400642E RID: 25646
		private const float OpenTicks = 45f;

		// Token: 0x0400642F RID: 25647
		private const int CloseDelayTicks = 110;

		// Token: 0x04006430 RID: 25648
		private const int ApproachCloseDelayTicks = 300;

		// Token: 0x04006431 RID: 25649
		private const int MaxTicksSinceFriendlyTouchToAutoClose = 120;

		// Token: 0x04006432 RID: 25650
		private const float PowerOffDoorOpenSpeedFactor = 0.25f;

		// Token: 0x04006433 RID: 25651
		private const float VisualDoorOffsetStart = 0f;

		// Token: 0x04006434 RID: 25652
		private const float VisualDoorOffsetEnd = 0.45f;

		// Token: 0x04006435 RID: 25653
		private const float NotifyFogGridDoorOpenPct = 0.4f;

		// Token: 0x04006436 RID: 25654
		private const int TicksOpenToBreach = 600;
	}
}
