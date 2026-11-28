using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022EF RID: 8943
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		// Token: 0x17002672 RID: 9842
		// (get) Token: 0x0600DCD2 RID: 56530 RVA: 0x005236E8 File Offset: 0x005218E8
		public bool IsValid
		{
			get
			{
				if (this.thingInt != null)
				{
					Pawn pawn = this.Pawn;
					if (pawn == null || !pawn.IsHiddenFromPlayer())
					{
						return true;
					}
				}
				if (!this.cellInt.IsValid && this.worldObjectInt == null)
				{
					return this.tileInt.Valid;
				}
				return true;
			}
		}

		// Token: 0x17002673 RID: 9843
		// (get) Token: 0x0600DCD3 RID: 56531 RVA: 0x00523736 File Offset: 0x00521936
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		// Token: 0x17002674 RID: 9844
		// (get) Token: 0x0600DCD4 RID: 56532 RVA: 0x0052374D File Offset: 0x0052194D
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt.Valid;
			}
		}

		// Token: 0x17002675 RID: 9845
		// (get) Token: 0x0600DCD5 RID: 56533 RVA: 0x00523764 File Offset: 0x00521964
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17002676 RID: 9846
		// (get) Token: 0x0600DCD6 RID: 56534 RVA: 0x0052376F File Offset: 0x0052196F
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17002677 RID: 9847
		// (get) Token: 0x0600DCD7 RID: 56535 RVA: 0x00523777 File Offset: 0x00521977
		public Pawn Pawn
		{
			get
			{
				return this.thingInt as Pawn;
			}
		}

		// Token: 0x17002678 RID: 9848
		// (get) Token: 0x0600DCD8 RID: 56536 RVA: 0x00523784 File Offset: 0x00521984
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17002679 RID: 9849
		// (get) Token: 0x0600DCD9 RID: 56537 RVA: 0x0052379B File Offset: 0x0052199B
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		// Token: 0x1700267A RID: 9850
		// (get) Token: 0x0600DCDA RID: 56538 RVA: 0x005237A6 File Offset: 0x005219A6
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		// Token: 0x1700267B RID: 9851
		// (get) Token: 0x0600DCDB RID: 56539 RVA: 0x005237AE File Offset: 0x005219AE
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x1700267C RID: 9852
		// (get) Token: 0x0600DCDC RID: 56540 RVA: 0x005237BC File Offset: 0x005219BC
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x1700267D RID: 9853
		// (get) Token: 0x0600DCDD RID: 56541 RVA: 0x005237F5 File Offset: 0x005219F5
		public IntVec3 Cell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.PositionHeld;
				}
				return this.cellInt;
			}
		}

		// Token: 0x1700267E RID: 9854
		// (get) Token: 0x0600DCDE RID: 56542 RVA: 0x00523811 File Offset: 0x00521A11
		public Map Map
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.MapHeld;
				}
				return this.mapInt;
			}
		}

		// Token: 0x1700267F RID: 9855
		// (get) Token: 0x0600DCDF RID: 56543 RVA: 0x00523830 File Offset: 0x00521A30
		public PlanetTile Tile
		{
			get
			{
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.Tile;
				}
				if (this.tileInt.Valid)
				{
					return this.tileInt;
				}
				if (this.thingInt != null && this.thingInt.Tile.Valid)
				{
					return this.thingInt.Tile;
				}
				if (this.cellInt.IsValid && this.mapInt != null)
				{
					return this.mapInt.Tile;
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x0600DCE0 RID: 56544 RVA: 0x005238B4 File Offset: 0x00521AB4
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = PlanetTile.Invalid;
		}

		// Token: 0x0600DCE1 RID: 56545 RVA: 0x005238E4 File Offset: 0x00521AE4
		public GlobalTargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				string str = "Constructed GlobalTargetInfo with cell=";
				IntVec3 intVec = cell;
				Log.Warning(str + intVec.ToString() + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
			this.worldObjectInt = null;
			this.tileInt = PlanetTile.Invalid;
		}

		// Token: 0x0600DCE2 RID: 56546 RVA: 0x0052394A File Offset: 0x00521B4A
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = PlanetTile.Invalid;
		}

		// Token: 0x0600DCE3 RID: 56547 RVA: 0x00523977 File Offset: 0x00521B77
		public GlobalTargetInfo(PlanetTile tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		// Token: 0x0600DCE4 RID: 56548 RVA: 0x005239A0 File Offset: 0x00521BA0
		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			if (target.HasThing)
			{
				return new GlobalTargetInfo(target.Thing);
			}
			return new GlobalTargetInfo(target.Cell, target.Map, false);
		}

		// Token: 0x0600DCE5 RID: 56549 RVA: 0x005239CC File Offset: 0x00521BCC
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		// Token: 0x0600DCE6 RID: 56550 RVA: 0x005239D4 File Offset: 0x00521BD4
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		// Token: 0x0600DCE7 RID: 56551 RVA: 0x005239DC File Offset: 0x00521BDC
		public static explicit operator LocalTargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				string str = "Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str + ((worldObject != null) ? worldObject.ToString() : null), 134566);
				return LocalTargetInfo.Invalid;
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt.ToString(), 7833122);
				return LocalTargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return LocalTargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new LocalTargetInfo(targ.thingInt);
			}
			return new LocalTargetInfo(targ.cellInt);
		}

		// Token: 0x0600DCE8 RID: 56552 RVA: 0x00523A88 File Offset: 0x00521C88
		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				string str = "Casted GlobalTargetInfo to TargetInfo but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str + ((worldObject != null) ? worldObject.ToString() : null), 134566);
				return TargetInfo.Invalid;
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt.ToString(), 7833122);
				return TargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new TargetInfo(targ.thingInt);
			}
			return new TargetInfo(targ.cellInt, targ.mapInt, false);
		}

		// Token: 0x0600DCE9 RID: 56553 RVA: 0x00523B38 File Offset: 0x00521D38
		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted GlobalTargetInfo to IntVec3 but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			if (targ.worldObjectInt != null)
			{
				string str2 = "Casted GlobalTargetInfo to IntVec3 but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str2 + ((worldObject != null) ? worldObject.ToString() : null), 134566);
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt.ToString(), 7833122);
			}
			return targ.Cell;
		}

		// Token: 0x0600DCEA RID: 56554 RVA: 0x00523BDC File Offset: 0x00521DDC
		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				string str = "Casted GlobalTargetInfo to Thing but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str + intVec.ToString(), 631672);
			}
			if (targ.worldObjectInt != null)
			{
				string str2 = "Casted GlobalTargetInfo to Thing but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str2 + ((worldObject != null) ? worldObject.ToString() : null), 134566);
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt.ToString(), 7833122);
			}
			return targ.thingInt;
		}

		// Token: 0x0600DCEB RID: 56555 RVA: 0x00523C88 File Offset: 0x00521E88
		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted GlobalTargetInfo to WorldObject but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			if (targ.cellInt.IsValid)
			{
				string str2 = "Casted GlobalTargetInfo to WorldObject but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str2 + intVec.ToString(), 631672);
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt.ToString(), 7833122);
			}
			return targ.worldObjectInt;
		}

		// Token: 0x0600DCEC RID: 56556 RVA: 0x00523D34 File Offset: 0x00521F34
		public static bool operator ==(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			if (a.cellInt.IsValid || b.cellInt.IsValid)
			{
				return a.cellInt == b.cellInt && a.mapInt == b.mapInt;
			}
			if (a.WorldObject != null || b.WorldObject != null)
			{
				return a.WorldObject == b.WorldObject;
			}
			return (!a.tileInt.Valid && !b.tileInt.Valid) || a.tileInt == b.tileInt;
		}

		// Token: 0x0600DCED RID: 56557 RVA: 0x00523DF6 File Offset: 0x00521FF6
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x0600DCEE RID: 56558 RVA: 0x00523E04 File Offset: 0x00522004
		public override bool Equals(object obj)
		{
			if (obj is GlobalTargetInfo)
			{
				GlobalTargetInfo other = (GlobalTargetInfo)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600DCEF RID: 56559 RVA: 0x00523E2B File Offset: 0x0052202B
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x0600DCF0 RID: 56560 RVA: 0x00523E3C File Offset: 0x0052203C
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			if (this.cellInt.IsValid)
			{
				return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
			}
			if (this.worldObjectInt != null)
			{
				return this.worldObjectInt.GetHashCode();
			}
			if (this.tileInt.Valid)
			{
				return this.tileInt.GetHashCode();
			}
			return -1;
		}

		// Token: 0x0600DCF1 RID: 56561 RVA: 0x00523EBC File Offset: 0x005220BC
		public override string ToString()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetUniqueLoadID();
			}
			if (this.cellInt.IsValid)
			{
				return this.cellInt.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			if (this.worldObjectInt != null)
			{
				return "@" + this.worldObjectInt.GetUniqueLoadID();
			}
			if (this.tileInt.Valid)
			{
				return this.tileInt.ToString();
			}
			return "null";
		}

		// Token: 0x04008D7C RID: 36220
		private Thing thingInt;

		// Token: 0x04008D7D RID: 36221
		private IntVec3 cellInt;

		// Token: 0x04008D7E RID: 36222
		private Map mapInt;

		// Token: 0x04008D7F RID: 36223
		private WorldObject worldObjectInt;

		// Token: 0x04008D80 RID: 36224
		private PlanetTile tileInt;

		// Token: 0x04008D81 RID: 36225
		public const char WorldObjectLoadIDMarker = '@';
	}
}
