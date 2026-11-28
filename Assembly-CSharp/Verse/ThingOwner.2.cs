using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005A3 RID: 1443
	public abstract class ThingOwner : IExposable, IList<Thing>, ICollection<Thing>, IEnumerable<Thing>, IEnumerable
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x001065B1 File Offset: 0x001047B1
		public IThingHolder Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002B12 RID: 11026
		public abstract int Count { get; }

		// Token: 0x170008C5 RID: 2245
		public Thing this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x001065C2 File Offset: 0x001047C2
		public bool Any
		{
			get
			{
				return this.Count > 0;
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06002B15 RID: 11029 RVA: 0x001065D0 File Offset: 0x001047D0
		// (remove) Token: 0x06002B16 RID: 11030 RVA: 0x00106608 File Offset: 0x00104808
		public event Action OnContentsChanged;

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002B17 RID: 11031 RVA: 0x00106640 File Offset: 0x00104840
		public int TotalStackCount
		{
			get
			{
				int num = 0;
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					num += this.GetAt(i).stackCount;
				}
				return num;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x00106672 File Offset: 0x00104872
		public string ContentsString
		{
			get
			{
				if (this.Any)
				{
					return GenThing.ThingsToCommaList(this, false, true, -1);
				}
				return "NothingLower".Translate();
			}
		}

		// Token: 0x170008C9 RID: 2249
		Thing IList<Thing>.this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x001066AA File Offset: 0x001048AA
		bool ICollection<Thing>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x001066AD File Offset: 0x001048AD
		public ThingOwner()
		{
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x001066CE File Offset: 0x001048CE
		public ThingOwner(IThingHolder owner)
		{
			this.owner = owner;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x001066F6 File Offset: 0x001048F6
		public ThingOwner(IThingHolder owner, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true)
		{
			this.owner = owner;
			this.contentsLookMode = contentsLookMode;
			this.removeContentsIfDestroyed = removeContentsIfDestroyed;
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x0010672C File Offset: 0x0010492C
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true) : this(owner)
		{
			this.maxStacks = (oneStackOnly ? 1 : 999999);
			this.contentsLookMode = contentsLookMode;
			this.removeContentsIfDestroyed = removeContentsIfDestroyed;
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x00106758 File Offset: 0x00104958
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.maxStacks, "maxStacks", 999999, false);
			Scribe_Values.Look<LookMode>(ref this.contentsLookMode, "contentsLookMode", LookMode.Deep, false);
			Scribe_Values.Look<bool>(ref this.removeContentsIfDestroyed, "removeContentsIfDestroyed", true, false);
			Scribe_Values.Look<bool>(ref this.dontTickContents, "dontTickContents", false, false);
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x001067B4 File Offset: 0x001049B4
		public void DoTick()
		{
			if (this.dontTickContents)
			{
				return;
			}
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing at = this.GetAt(i);
				at.DoTick();
				if (at.Destroyed && this.removeContentsIfDestroyed)
				{
					this.Remove(at);
				}
			}
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00106804 File Offset: 0x00104A04
		public void Clear()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.Remove(this.GetAt(i));
			}
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x00106834 File Offset: 0x00104A34
		public void ClearAndDestroyContents(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int i = this.Count - 1; i >= 0; i--)
				{
					Thing at = this.GetAt(i);
					at.Destroy(mode);
					this.Remove(at);
				}
			}
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x00106878 File Offset: 0x00104A78
		public void ClearAndDestroyContentsOrPassToWorld(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int i = this.Count - 1; i >= 0; i--)
				{
					Thing at = this.GetAt(i);
					at.DestroyOrPassToWorld(mode);
					this.Remove(at);
				}
			}
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x001068B9 File Offset: 0x00104AB9
		public bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
		{
			return this.GetCountCanAccept(item, canMergeWithExistingStacks) > 0;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x001068C8 File Offset: 0x00104AC8
		public virtual int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null || item.stackCount <= 0)
			{
				return 0;
			}
			if (this.maxStacks == 999999)
			{
				return item.stackCount;
			}
			int num = 0;
			if (this.Count < this.maxStacks)
			{
				num += (this.maxStacks - this.Count) * item.def.stackLimit;
			}
			if (num >= item.stackCount)
			{
				return Mathf.Min(num, item.stackCount);
			}
			if (canMergeWithExistingStacks)
			{
				int i = 0;
				int count = this.Count;
				while (i < count)
				{
					Thing at = this.GetAt(i);
					if (at.stackCount < at.def.stackLimit && at.CanStackWith(item))
					{
						num += at.def.stackLimit - at.stackCount;
						if (num >= item.stackCount)
						{
							return Mathf.Min(num, item.stackCount);
						}
					}
					i++;
				}
			}
			return Mathf.Min(num, item.stackCount);
		}

		// Token: 0x06002B27 RID: 11047
		public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

		// Token: 0x06002B28 RID: 11048
		public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

		// Token: 0x06002B29 RID: 11049
		public abstract int IndexOf(Thing item);

		// Token: 0x06002B2A RID: 11050
		public abstract bool Remove(Thing item);

		// Token: 0x06002B2B RID: 11051
		protected abstract Thing GetAt(int index);

		// Token: 0x06002B2C RID: 11052 RVA: 0x001069AA File Offset: 0x00104BAA
		public bool Contains(Thing item)
		{
			return item != null && item.holdingOwner == this;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x001069BA File Offset: 0x00104BBA
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.GetAt(index));
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x001069E2 File Offset: 0x00104BE2
		public int TryAddOrTransfer(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				return 0;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, count, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, count, canMergeWithExistingStacks);
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x00106A14 File Offset: 0x00104C14
		public bool TryAddOrTransfer(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, canMergeWithExistingStacks);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x00106A44 File Offset: 0x00104C44
		public void TryAddRangeOrTransfer(IEnumerable<Thing> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
		{
			if (things == this)
			{
				return;
			}
			ThingOwner thingOwner = things as ThingOwner;
			if (thingOwner != null)
			{
				thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
				if (destroyLeftover)
				{
					thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				IList<Thing> list = things as IList<Thing>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!this.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					return;
				}
				foreach (Thing thing in things)
				{
					if (!this.TryAddOrTransfer(thing, canMergeWithExistingStacks) && destroyLeftover)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x00106B04 File Offset: 0x00104D04
		public int RemoveAll(Predicate<Thing> predicate)
		{
			int num = 0;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				if (predicate(this.GetAt(i)))
				{
					this.Remove(this.GetAt(i));
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x00106B48 File Offset: 0x00104D48
		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, bool canMergeWithExistingStacks = true)
		{
			return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks) == item.stackCount;
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x00106B64 File Offset: 0x00104D64
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x00106B80 File Offset: 0x00104D80
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, out Thing resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			if (!this.Contains(item))
			{
				Log.Error("Can't transfer item " + ((item != null) ? item.ToString() : null) + " because it's not here. owner=" + this.owner.ToStringSafe<IThingHolder>());
				resultingTransferredItem = null;
				return 0;
			}
			if (otherContainer == this && count > 0)
			{
				resultingTransferredItem = item;
				return item.stackCount;
			}
			if (!otherContainer.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				resultingTransferredItem = null;
				return 0;
			}
			if (count <= 0)
			{
				resultingTransferredItem = null;
				return 0;
			}
			if (this.owner is Map || otherContainer.owner is Map)
			{
				Log.Warning("Can't transfer items to or from Maps directly. They must be spawned or despawned manually. Use TryAdd(item.SplitOff(count))");
				resultingTransferredItem = null;
				return 0;
			}
			int num = Mathf.Min(item.stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.Contains(thing))
			{
				this.Remove(thing);
			}
			if (otherContainer.TryAdd(thing, canMergeWithExistingStacks))
			{
				resultingTransferredItem = thing;
				Map mapHeld = item.MapHeld;
				if (mapHeld != null)
				{
					ResourceCounter resourceCounter = mapHeld.resourceCounter;
					if (resourceCounter != null)
					{
						resourceCounter.CheckUpdateResource(thing);
					}
				}
				return thing.stackCount;
			}
			resultingTransferredItem = null;
			if (otherContainer.Contains(thing) || thing.stackCount <= 0 || thing.Destroyed)
			{
				return thing.stackCount;
			}
			int result = num - thing.stackCount;
			if (item != thing)
			{
				item.TryAbsorbStack(thing, false);
			}
			else
			{
				this.TryAdd(thing, false);
			}
			Map mapHeld2 = item.MapHeld;
			if (mapHeld2 == null)
			{
				return result;
			}
			ResourceCounter resourceCounter2 = mapHeld2.resourceCounter;
			if (resourceCounter2 == null)
			{
				return result;
			}
			resourceCounter2.CheckUpdateResource(thing);
			return result;
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x00106CDC File Offset: 0x00104EDC
		public void TryTransferAllToContainer(ThingOwner other, bool canMergeWithExistingStacks = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.TryTransferToContainer(this.GetAt(i), other, canMergeWithExistingStacks);
			}
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x00106D0C File Offset: 0x00104F0C
		public Thing Take(Thing thing, int count)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to take " + thing.ToStringSafe<Thing>() + " but it's not here.");
				return null;
			}
			if (count > thing.stackCount)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get ",
					count.ToString(),
					" of ",
					thing.ToStringSafe<Thing>(),
					" while only having ",
					thing.stackCount.ToString()
				}));
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				this.Remove(thing);
				return thing;
			}
			Thing thing2 = thing.SplitOff(count);
			thing2.holdingOwner = null;
			return thing2;
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x00106DB9 File Offset: 0x00104FB9
		public Thing Take(Thing thing)
		{
			return this.Take(thing, thing.stackCount);
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x00106DC8 File Offset: 0x00104FC8
		public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + ((thing != null) ? thing.ToString() : null) + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction, nearPlaceValidator);
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x00106E34 File Offset: 0x00105034
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to drop " + thing.ToStringSafe<Thing>() + " but it's not here.");
				resultingThing = null;
				return false;
			}
			if (thing.stackCount < count)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to drop ",
					count.ToString(),
					" of ",
					(thing != null) ? thing.ToString() : null,
					" while only having ",
					thing.stackCount.ToString()
				}));
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					this.Remove(thing);
					return true;
				}
				return false;
			}
			else
			{
				Thing thing2 = thing.SplitOff(count);
				if (GenDrop.TryDropSpawn(thing2, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					return true;
				}
				thing.TryAbsorbStack(thing2, false);
				return false;
			}
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x00106F20 File Offset: 0x00105120
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + ((thing != null) ? thing.ToString() : null) + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x00106F88 File Offset: 0x00105188
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (!this.Contains(thing))
			{
				Log.Error(this.owner.ToStringSafe<IThingHolder>() + " container tried to drop  " + thing.ToStringSafe<Thing>() + " which it didn't contain.");
				lastResultingThing = null;
				return false;
			}
			if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, playDropSound))
			{
				this.Remove(thing);
				return true;
			}
			return false;
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x00106FE8 File Offset: 0x001051E8
		public bool TryDropAll(IntVec3 dropLoc, Map map, ThingPlaceMode mode, Action<Thing, int> placeAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			bool result = true;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing thing;
				if (!this.TryDrop(this.GetAt(i), dropLoc, map, mode, out thing, placeAction, nearPlaceValidator, playDropSound))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x00107027 File Offset: 0x00105227
		public bool Contains(ThingDef def)
		{
			return this.Contains(def, 1);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x00107034 File Offset: 0x00105234
		public bool Contains(ThingDef def, int minCount)
		{
			if (minCount <= 0)
			{
				return true;
			}
			int num = 0;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.GetAt(i).def == def)
				{
					num += this.GetAt(i).stackCount;
				}
				if (num >= minCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x00107084 File Offset: 0x00105284
		public int TotalStackCountOfDef(ThingDef def)
		{
			int num = 0;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.GetAt(i).def == def)
				{
					num += this.GetAt(i).stackCount;
				}
			}
			return num;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x001070C5 File Offset: 0x001052C5
		public void Notify_ContainedItemDestroyed(Thing t)
		{
			if (ThingOwnerUtility.ShouldAutoRemoveDestroyedThings(this.owner))
			{
				this.Remove(t);
			}
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x001070DC File Offset: 0x001052DC
		protected virtual void NotifyAdded(Thing item)
		{
			if (ThingOwnerUtility.ShouldAutoExtinguishInnerThings(this.owner) && item.HasAttachment(ThingDefOf.Fire))
			{
				item.GetAttachment(ThingDefOf.Fire).Destroy(DestroyMode.Vanish);
			}
			if (ThingOwnerUtility.ShouldRemoveDesignationsOnAddedThings(this.owner))
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					maps[i].designationManager.RemoveAllDesignationsOn(item, false);
				}
			}
			Thing thing = this.owner as Thing;
			if (thing == null || !thing.Faction.IsPlayerSafe())
			{
				WorldObject worldObject = this.owner as WorldObject;
				if (worldObject == null || !worldObject.Faction.IsPlayerSafe())
				{
					Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
					if (pawn_InventoryTracker == null || !pawn_InventoryTracker.pawn.Faction.IsPlayerSafe())
					{
						goto IL_CA;
					}
				}
			}
			item.EverSeenByPlayer = true;
			IL_CA:
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAdded(item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnAdded((Pawn)item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelAdded((Apparel)item);
				if (pawn_ApparelTracker.pawn.Faction.IsPlayerSafe())
				{
					item.EverSeenByPlayer = true;
				}
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentAdded((ThingWithComps)item);
				if (pawn_EquipmentTracker.pawn.Faction.IsPlayerSafe())
				{
					item.EverSeenByPlayer = true;
				}
			}
			this.NotifyColonistBarIfColonistCorpse(item);
			Action onContentsChanged = this.OnContentsChanged;
			if (onContentsChanged == null)
			{
				return;
			}
			onContentsChanged();
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x00107270 File Offset: 0x00105470
		protected void NotifyAddedAndMergedWith(Thing item, int mergedCount)
		{
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAddedAndMergedWith(item, mergedCount);
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x00107294 File Offset: 0x00105494
		protected virtual void NotifyRemoved(Thing item)
		{
			Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
			if (pawn_InventoryTracker != null)
			{
				pawn_InventoryTracker.Notify_ItemRemoved(item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelRemoved((Apparel)item);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentRemoved((ThingWithComps)item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnRemoved((Pawn)item);
			}
			this.NotifyColonistBarIfColonistCorpse(item);
			Action onContentsChanged = this.OnContentsChanged;
			if (onContentsChanged == null)
			{
				return;
			}
			onContentsChanged();
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x00107320 File Offset: 0x00105520
		private void NotifyColonistBarIfColonistCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse != null && !corpse.Bugged && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x00107370 File Offset: 0x00105570
		void IList<Thing>.Insert(int index, Thing item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x0010737C File Offset: 0x0010557C
		void ICollection<Thing>.Add(Thing item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x00107388 File Offset: 0x00105588
		void ICollection<Thing>.CopyTo(Thing[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this.GetAt(i);
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x001073B2 File Offset: 0x001055B2
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			ThingOwner.<System-Collections-Generic-IEnumerable<Verse-Thing>-GetEnumerator>d__70 <System-Collections-Generic-IEnumerable<Verse-Thing>-GetEnumerator>d__ = new ThingOwner.<System-Collections-Generic-IEnumerable<Verse-Thing>-GetEnumerator>d__70(0);
			<System-Collections-Generic-IEnumerable<Verse-Thing>-GetEnumerator>d__.<>4__this = this;
			return <System-Collections-Generic-IEnumerable<Verse-Thing>-GetEnumerator>d__;
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x001073C1 File Offset: 0x001055C1
		IEnumerator IEnumerable.GetEnumerator()
		{
			ThingOwner.<System-Collections-IEnumerable-GetEnumerator>d__71 <System-Collections-IEnumerable-GetEnumerator>d__ = new ThingOwner.<System-Collections-IEnumerable-GetEnumerator>d__71(0);
			<System-Collections-IEnumerable-GetEnumerator>d__.<>4__this = this;
			return <System-Collections-IEnumerable-GetEnumerator>d__;
		}

		// Token: 0x04001D77 RID: 7543
		protected IThingHolder owner;

		// Token: 0x04001D78 RID: 7544
		protected int maxStacks = 999999;

		// Token: 0x04001D79 RID: 7545
		public LookMode contentsLookMode = LookMode.Deep;

		// Token: 0x04001D7A RID: 7546
		public bool removeContentsIfDestroyed = true;

		// Token: 0x04001D7B RID: 7547
		public bool dontTickContents;

		// Token: 0x04001D7C RID: 7548
		private const int InfMaxStacks = 999999;
	}
}
