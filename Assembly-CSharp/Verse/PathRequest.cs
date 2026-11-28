using System;
using System.Runtime.CompilerServices;
using RimWorld;
using Unity.Collections;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200036B RID: 875
	public class PathRequest : IDisposable
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x000A4E18 File Offset: 0x000A3018
		public IntVec3 Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x000A4E20 File Offset: 0x000A3020
		public LocalTargetInfo Target
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x000A4E28 File Offset: 0x000A3028
		public IntVec3 ExactDestination
		{
			get
			{
				IntVec3? intVec = this.exactDestination;
				if (intVec == null)
				{
					return this.dest.Cell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x000A4E58 File Offset: 0x000A3058
		public TraverseParms TraverseParms
		{
			get
			{
				return this.traverseParms;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x000A4E60 File Offset: 0x000A3060
		public PathFinderCostTuning Tuning
		{
			get
			{
				return this.tuning;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x000A4E68 File Offset: 0x000A3068
		public PathEndMode EndMode
		{
			get
			{
				return this.endMode;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x000A4E70 File Offset: 0x000A3070
		public int TickStart
		{
			get
			{
				return this.tickStart;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x000A4E78 File Offset: 0x000A3078
		public bool ResultIsReady
		{
			get
			{
				return this.found != null;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x000A4E85 File Offset: 0x000A3085
		public bool Cancelled
		{
			get
			{
				return this.cancelled;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x000A4E8D File Offset: 0x000A308D
		public bool? Found
		{
			get
			{
				return this.found;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x000A4E98 File Offset: 0x000A3098
		public Faction RequesterFaction
		{
			get
			{
				if (this.pawn == null)
				{
					return null;
				}
				Pawn_GuestTracker guest = this.pawn.guest;
				if (guest == null)
				{
					return this.pawn.Faction;
				}
				return guest.HostFaction ?? this.pawn.Faction;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x000A4EDF File Offset: 0x000A30DF
		public Lord RequesterLord
		{
			get
			{
				if (this.pawn == null)
				{
					return null;
				}
				return this.pawn.lord;
			}
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x000A4EF8 File Offset: 0x000A30F8
		public PathRequest(Map map, IntVec3 start, LocalTargetInfo dest, IntVec3? exactDestination, TraverseParms traverseParms, PathFinderCostTuning tuning, PathEndMode peMode, Pawn pawn, int tickCreated, int tickStart, int tickDeadline, PathRequest.IPathGridCustomizer customizer = null)
		{
			this.map = map;
			this.start = start;
			this.dest = dest;
			this.exactDestination = exactDestination;
			this.traverseParms = traverseParms;
			this.tuning = tuning;
			this.endMode = peMode;
			this.pawn = pawn;
			this.tickCreated = tickCreated;
			this.tickStart = tickStart;
			this.tickDeadline = tickDeadline;
			this.customizer = customizer;
			this.area = PathUtility.GetAllowedArea(pawn);
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000A4F75 File Offset: 0x000A3175
		public bool Validate()
		{
			bool flag = this.ValidateInt();
			if (!flag)
			{
				this.found = new bool?(false);
			}
			return flag;
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x000A4F8C File Offset: 0x000A318C
		private bool ValidateInt()
		{
			Pawn pawn = this.traverseParms.pawn;
			if (pawn != null && pawn.Map != this.map)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to FindPath for pawn which is spawned in another map. Its map PathFinder should have been used, not this one. pawn=",
					pawn.ToStringSafe<Pawn>(),
					" pawn.Map=",
					pawn.Map.ToStringSafe<Map>(),
					" map=",
					this.map.ToStringSafe<Map>()
				}));
				return false;
			}
			if (!this.start.IsValid)
			{
				Log.Error("Tried to FindPath with invalid start " + this.start.ToStringSafe<IntVec3>() + ", pawn= " + pawn.ToStringSafe<Pawn>());
				return false;
			}
			if (!this.dest.IsValid)
			{
				return false;
			}
			using (ProfilerBlock.Scope("Reachability"))
			{
				if (this.traverseParms.mode == TraverseMode.ByPawn)
				{
					if (!pawn.CanReach(this.dest, this.endMode, Danger.Deadly, this.traverseParms.canBashDoors, this.traverseParms.canBashFences, this.traverseParms.mode))
					{
						return false;
					}
				}
				else if (!this.map.reachability.CanReach(this.start, this.dest, this.endMode, this.traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x000A50EC File Offset: 0x000A32EC
		public void Resolve(PawnPath p)
		{
			if (p != null && p.Found)
			{
				this.path = p;
				this.found = new bool?(true);
				return;
			}
			if (p != null)
			{
				p.Dispose();
			}
			this.found = new bool?(false);
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x000A5122 File Offset: 0x000A3322
		public bool TryGetPath(out PawnPath outPath)
		{
			if (this.ResultIsReady)
			{
				outPath = this.path;
				return true;
			}
			outPath = null;
			return false;
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000A513A File Offset: 0x000A333A
		public void ClaimCalculatedPath()
		{
			this.path = null;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000A5144 File Offset: 0x000A3344
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 5);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.start);
			defaultInterpolatedStringHandler.AppendLiteral(" -> ");
			defaultInterpolatedStringHandler.AppendFormatted<LocalTargetInfo>(this.dest);
			string value;
			if (!this.dest.HasThing)
			{
				value = "";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" @ ");
				defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(this.dest.Cell);
				value = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			defaultInterpolatedStringHandler.AppendFormatted(value);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<PathEndMode>(this.endMode);
			defaultInterpolatedStringHandler.AppendLiteral(", for ");
			defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000A521B File Offset: 0x000A341B
		public void Dispose()
		{
			this.cancelled = true;
			if (this.path != null)
			{
				this.path.Dispose();
				this.path = null;
			}
		}

		// Token: 0x0400160F RID: 5647
		public Map map;

		// Token: 0x04001610 RID: 5648
		private IntVec3 start;

		// Token: 0x04001611 RID: 5649
		private LocalTargetInfo dest;

		// Token: 0x04001612 RID: 5650
		private IntVec3? exactDestination;

		// Token: 0x04001613 RID: 5651
		private TraverseParms traverseParms;

		// Token: 0x04001614 RID: 5652
		private PathFinderCostTuning tuning;

		// Token: 0x04001615 RID: 5653
		private PathEndMode endMode;

		// Token: 0x04001616 RID: 5654
		private int tickCreated;

		// Token: 0x04001617 RID: 5655
		private int tickStart;

		// Token: 0x04001618 RID: 5656
		private int tickDeadline;

		// Token: 0x04001619 RID: 5657
		public Pawn pawn;

		// Token: 0x0400161A RID: 5658
		public PathRequest.IPathGridCustomizer customizer;

		// Token: 0x0400161B RID: 5659
		public Area area;

		// Token: 0x0400161C RID: 5660
		private bool cancelled;

		// Token: 0x0400161D RID: 5661
		private PawnPath path;

		// Token: 0x0400161E RID: 5662
		private bool? found;

		// Token: 0x0200269D RID: 9885
		public interface IPathGridCustomizer
		{
			// Token: 0x0600F433 RID: 62515
			NativeArray<ushort> GetOffsetGrid();
		}
	}
}
